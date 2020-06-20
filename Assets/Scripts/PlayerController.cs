using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class PlayerController : MonoBehaviour
{   
    public static PlayerController instance;
    //Move Speed
    public float moveSpeed;
    public float backedMoveSpeed;
    //Dashing Move Speed
    public float dashSpeed = 8f,dashLength = 0.5f,dashCooldown = 1.5f, dashInvincibleTime = 0.5f; 
    public GameObject hitBoxObject;
    //Rigid Body for Player
    public Rigidbody2D rigidBodyPlayer;
    //Sprite Renderer Player Body
    public SpriteRenderer spriteRenderer;
    //Animator
    public Animator animator;
    public float attackRange = 0.75f;
    //Weapons gameObject
    public GameObject Katana;
    public GameObject Baton;
    public GameObject Sword;
    public GameObject Axe;
    public GameObject Hammer;
    // Start is called before the first frame update
    private float camHeight;
    private float camWidth;
    private Vector2 moveInput;
    private Vector2 savedInput;
    private int[] mapSize;
    public List<char> playerWeapons;
    private int currWeaponPos = 0;
    private float activeMoveSpeed;
    private float dashCounter, dashCoolCounter;
    private float radian;
    private List<int> Weaponsdamage;
    private List<float> Weaponsrange;
    private List<float> WeaponsattackCooldown;
    private float moveForce = 0;
    private bool isSliding = false;
    private void Awake()
    {
        instance = this;
        playerWeapons = new List<char>();
        Weaponsdamage = new List<int>();
        Weaponsrange = new List<float>();
        WeaponsattackCooldown = new List<float>();
        backedMoveSpeed = moveSpeed;
        //Basic Weapon Baton all Players
        playerWeapons.Add('B');
        Weaponsdamage.Add(3);
        Weaponsrange.Add(1.33f);
        WeaponsattackCooldown.Add(0.75f);
        
    }
    public void InitializePlayer(float dashSpeed,float dashLength, float dashCooldown, float dashInvincibleTime, float attackRange)
    {
        this.dashSpeed = dashSpeed;
        this.dashLength = dashLength;
        this.dashCooldown = dashCooldown;
        this.dashInvincibleTime = dashInvincibleTime;
        this.attackRange = attackRange;
    }
    
    void Start()
    {
        activeMoveSpeed = moveSpeed;
        //Center the Camera on Player on Start
        camHeight = Camera.main.orthographicSize* 2f;
        camWidth = Camera.main.aspect * camHeight;
        Debug.Log("Camera Width: " + camWidth);
        Debug.Log("Camera Height: " + camHeight);
        Camera.main.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,-10);
        mapSize = GameManager.instance.getMapSize();
        mapSize[0]=mapSize[0]+1;
        mapSize[1]=mapSize[1]+1;
        changeWeaponPlayerTo(0);

    }
    public void setMoveSpeed(float moveSpeed)
    {
        this.activeMoveSpeed = moveSpeed;
    }
    public void addWeaponWithStats(Char weapon,int damage, float range, float attackCooldown)
    {
        if (weapon == 'K')
        {
            Debug.Log("Weapon Added: weapon_katana");
            playerWeapons.Add(weapon);
            Weaponsdamage.Add(damage);
            Weaponsrange.Add(range);
            WeaponsattackCooldown.Add(attackCooldown);
        }
        else if (weapon == 'S')
        {
            Debug.Log("Weapon Added: weapon_knight_sword");
            playerWeapons.Add(weapon);
            Weaponsdamage.Add(damage);
            Weaponsrange.Add(range);
            WeaponsattackCooldown.Add(attackCooldown);
        }
        else if (weapon == 'A')
        {
            Debug.Log("Weapon Added: weapon_axe");
            playerWeapons.Add(weapon);
            Weaponsdamage.Add(damage);
            Weaponsrange.Add(range);
            WeaponsattackCooldown.Add(attackCooldown);
        }
        else if (weapon == 'H')
        {
            Debug.Log("Weapon Added: weapon_big_hammer");
            playerWeapons.Add(weapon);
            Weaponsdamage.Add(damage);
            Weaponsrange.Add(range);
            WeaponsattackCooldown.Add(attackCooldown);
        }
    }

    public void nextWeapon()
    {
        currWeaponPos = autoIncrement(currWeaponPos, playerWeapons.Count);
        changeWeaponPlayerTo(currWeaponPos);
    }
    public void changeWeaponPlayerTo(int weaponPositionList)
    {
        Debug.Log("Changing Weapon pos: "+ weaponPositionList);
            switch (playerWeapons[weaponPositionList])
            {
            case 'K':
                Katana.SetActive(true);
                Hammer.SetActive(false);
                Baton.SetActive(false);
                Axe.SetActive(false);
                Sword.SetActive(false);
                
                break;
            case 'S':
                Katana.SetActive(false);
                Hammer.SetActive(false);
                Baton.SetActive(false);
                Axe.SetActive(false);
                Sword.SetActive(true);
                break;
            case 'A':
                Katana.SetActive(false);
                Hammer.SetActive(false);
                Baton.SetActive(false);
                Axe.SetActive(true);
                Sword.SetActive(false);
                break;
            case 'B':
                Katana.SetActive(false);
                Hammer.SetActive(false);
                Baton.SetActive(true);
                Axe.SetActive(false);
                Sword.SetActive(false);
                break;
            case 'H':
                Katana.SetActive(false);
                Hammer.SetActive(true);
                Baton.SetActive(false);
                Axe.SetActive(false);
                Sword.SetActive(false);
                break;
            default:
                Katana.SetActive(false);
                Hammer.SetActive(false);
                Baton.SetActive(true);
                Axe.SetActive(false);
                Sword.SetActive(false);
                break;
            }
        PlayerAttack.instance.SetWeaponStats(Weaponsdamage[weaponPositionList], Weaponsrange[weaponPositionList], WeaponsattackCooldown[weaponPositionList]);
    }

    public void changeTransparency(float alpha)
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
    }
    private int autoIncrement(int toIncrement,int maxLength)
    {   toIncrement++;
        if (toIncrement >= maxLength)
        {
            toIncrement = 0;
        }
        return toIncrement;
    }
    private int autoDecrement(int toDecrement, int maxLength)
    {
        toDecrement--;
        if (toDecrement < 0)
        {
            toDecrement = maxLength-1;
        }
        return toDecrement;
    }
    // Update is called once per frame
    void Update()
    {
        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        moveInput.x = SimpleInput.GetAxisRaw("Horizontal");
        moveInput.y = SimpleInput.GetAxisRaw("Vertical");
        moveInput.Normalize();
        //Move the player
        if (!isSliding)
        { rigidBodyPlayer.velocity = moveInput * activeMoveSpeed; } 
        else
        {
            rigidBodyPlayer.velocity = Vector3.ClampMagnitude(rigidBodyPlayer.velocity, 15);
            rigidBodyPlayer.AddForce(moveInput * moveForce);
        }
        //Hitbox and Animation
        if (moveInput.magnitude>0){
            animator.SetBool("isMoving",true);
            animator.SetFloat("Horizontal",moveInput.x);
            animator.SetFloat("Vertical",moveInput.y);
            savedInput = moveInput;
            //Attack HitBox Change Direction
            radian = Convert.ToSingle(Math.Atan2(moveInput.y, moveInput.x));
            hitBoxObject.transform.position = gameObject.transform.position + new Vector3(Convert.ToSingle(attackRange * Math.Cos(radian)), Convert.ToSingle(attackRange * Math.Sin(radian)), 0);
        }
        else
        {
            animator.SetBool("isMoving",false);
            animator.SetFloat("Horizontal",savedInput.x);
            animator.SetFloat("Vertical",savedInput.y);
        }
        if(SimpleInput.GetKeyDown(KeyCode.Backspace))
            {
            GameManager.instance.nextMap();
        }
        if (playerWeapons.Count > 0)
        {
            //Weapon change part
            if (SimpleInput.GetKeyDown(KeyCode.E))
            {
                nextWeapon();
            }
            float d = Input.GetAxis("Mouse ScrollWheel");
            if (d > 0f)
            {
                nextWeapon();
            }
            if (d < 0f)
            {
                currWeaponPos=autoDecrement(currWeaponPos, playerWeapons.Count);
                changeWeaponPlayerTo(currWeaponPos);
            }
        }

        //Player Dash Part
        if (SimpleInput.GetKeyDown(KeyCode.Space))
        {
            startDash();
        }
        if (dashCounter > 0)
        {
            dashCounter -=Time.deltaTime;
            if (dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
                dashCoolCounter = dashCooldown;
            }
        }
        if(dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
    }
    public float getAngle() { return radian* 57.2957795131f;}
    public void startDash()
    {
        if (dashCoolCounter <= 0 && dashCounter <= 0)
        {
            activeMoveSpeed = dashSpeed;
            dashCounter = dashLength;
            PlayerHealthController.instance.MakeInvincible(dashInvincibleTime);
        }
    }
    public void setSliding(float force,bool isSliding)
    {
        this.isSliding = isSliding;
        moveForce = force;
    }
    private void FixedUpdate()
    {
        TrackPlayer();
    }
    void TrackPlayer()
    {
        float finalCamX;
        float finalCamY;
        /*finalCamX = transform.position.x;
        finalCamY = transform.position.y;*/
        finalCamX = Mathf.Clamp(transform.position.x, -0.5f+camWidth/2f, mapSize[0] - 0.5f- camWidth / 2f);
        finalCamY = Mathf.Clamp(transform.position.y, -0.5f+camHeight/2f, mapSize[1] - 0.5f- camHeight / 2f);
        
        Camera.main.transform.position = new Vector3(finalCamX, finalCamY, -10f);
    }
}
