using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{   
    public Rigidbody2D enemyRB;
    public float moveSpeed;
    public float backedMoveSpeed;
    public float rangeToChasePlayer;
    private Vector3 moveDirection;
    //Explosion GamePrefab
    public GameObject explosionEffect;
    //Animation Enemy Controller
    public Animator animator;
    public int health = 10;
    public int damage;
    public int exp = 20;
    private bool isSliding =false;
    private float moveForce = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        backedMoveSpeed = moveSpeed;
    }
    void Start()
    {   
    }
   public void setMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
    // Update is called once per frame
    void Update()
    {   //Only if player is active, means not dead than execute Enemy code
        if (PlayerController.instance.gameObject.activeInHierarchy) 
        { 
            if(Vector3.Distance(transform.position,PlayerController.instance.transform.position) < rangeToChasePlayer){
                moveDirection = PlayerController.instance.transform.position -transform.position;
                animator.SetBool("isIdle", true);
            }
            else{
                moveDirection = Vector3.zero;
                animator.SetBool("isIdle", false);
            }
            moveDirection.Normalize();
            enemyRB.velocity = moveDirection*moveSpeed;
            if (!isSliding) { enemyRB.velocity = moveDirection * moveSpeed; }
            else
            {
                enemyRB.velocity = Vector3.ClampMagnitude(enemyRB.velocity, 20);
                enemyRB.AddForce(moveDirection.normalized * moveForce);
            }
        }
        else
        {
            enemyRB.velocity = Vector2.zero;
        }
    }
    public void setSliding(float force, bool isSliding)
    {
        this.isSliding = isSliding;
        moveForce = force;
    }
    public void InitializeEnemy(int damage,int health,int exp)
    {
        this.damage = damage;
        this.exp = exp;
        this.health = health;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer(damage);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer(damage);
        }
    }
    public void TakeDamage(int damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            PlayerStatsController.instance.addkills(1);
            PlayerStatsController.instance.addExperience(exp);
            GameObject explosionObj = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosionObj,0.33f);
            Destroy(gameObject);
        }
    }
}
