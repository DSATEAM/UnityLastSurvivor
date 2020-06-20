using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{   
    public CircleCollider2D myCollider;
    public float bombTimer;
    public float attackRange;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public GameObject explosionEffect;
    public Animator animator;
    public int damage;
    bool isActivated = false;
    public float activationRange = 2;
    // Start is called before the first frame update
    void Start()
    {
        myCollider.radius = activationRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (bombTimer <= 0 && isActivated)
        {
            //Do damage to everybody in the range area
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(gameObject.transform.position, attackRange, enemyLayer);
            foreach (Collider2D enemy in enemiesToDamage)
            {
                //We can't have instance like player due to the fact we will have multiple enemies while only one player
                enemy.GetComponent<EnemyController>().TakeDamage(damage);
            }
            Collider2D[] playersToDamage = Physics2D.OverlapCircleAll(gameObject.transform.position, attackRange, playerLayer);
            foreach (Collider2D player in playersToDamage)
            {
                //We can't have instance like player due to the fact we will have multiple enemies while only one player
                PlayerHealthController.instance.DamagePlayer(damage);
            }
            GameObject obj = Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity);
            Destroy(obj, 0.33f);
            Destroy(gameObject, 0.05f);
            isActivated = false;
        }
        else if(bombTimer > 0 && isActivated)
        {
            bombTimer -= Time.deltaTime;
        }
    }
    public void InitializeBomb(int attackRange,int activationRange,float bombTimer, int damage)
    {
        this.attackRange =attackRange;
        this.activationRange = activationRange;
        this.bombTimer = bombTimer;
        this.damage = damage;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, attackRange);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isActivated = true;
            animator.SetBool("isActivated", true);
            //Activate Bomb Animation from idle to explosion timing
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isActivated = true;
            animator.SetBool("isActivated", true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isActivated = true;
            animator.SetBool("isActivated", true);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isActivated = true;
            animator.SetBool("isActivated", true);
        }
    }
}
