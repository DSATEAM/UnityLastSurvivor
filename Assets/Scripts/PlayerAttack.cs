using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{   
    public static PlayerAttack instance;
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public Transform hitBoxTransform;
    public float hitBoxRadius;
    public LayerMask enemyLayer;
    public LayerMask propsLayer;

    public GameObject slashEffect;
    public Animator animator;
    public int damage;
    private bool isAndroidPlatform;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            isAndroidPlatform = true;
        }
        else if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            isAndroidPlatform = false;
        }
    }
    public void SetWeaponStats(int damage, float range, float attackCooldown)
    {
        this.damage = damage;
        this.hitBoxRadius = range;
        this.startTimeBtwAttack = attackCooldown;
    }

    void Update()
    {   
        if(!isAndroidPlatform)
        { 
            if (timeBtwAttack <= 0)
            {
                if (SimpleInput.GetKey(KeyCode.Mouse0))
                {
                   attackEnemy();
                }
            }
            
        }
        if(timeBtwAttack>0)
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }
    IEnumerator DelayResetAnim(float delay)
    {
       // Debug.Log(Time.time);
        yield return new WaitForSeconds(delay);
        animator.SetBool("Attack", false);
       // Debug.Log(Time.time);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitBoxTransform.position,hitBoxRadius);
    }
    public void attackEnemy()
    {
        if (timeBtwAttack <= 0)
        { 
            //Angles
            float angle = PlayerController.instance.getAngle();
            GameObject obj = Instantiate(slashEffect, hitBoxTransform.position, Quaternion.Euler(0, 0, angle));
            timeBtwAttack = startTimeBtwAttack;
            animator.SetBool("Attack", true);
            StartCoroutine("DelayResetAnim", 0.15f);
            Destroy(obj, 0.15f);
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(hitBoxTransform.position, hitBoxRadius, enemyLayer);
            Collider2D[] propsToDamage = Physics2D.OverlapCircleAll(hitBoxTransform.position, hitBoxRadius, propsLayer);
            foreach (Collider2D enemy in enemiesToDamage)
            {
                //We can't have Singleton instance like player due to the fact we will have multiple enemies
                if ((enemy != null) && (enemy.GetComponent<EnemyController>() != null))
                    enemy.GetComponent<EnemyController>().TakeDamage(damage);
            }
            foreach (Collider2D prop in propsToDamage)
            {
                //We can't have Singleton instance like player due to the fact we will have multiple enemies
                if ((prop != null) && (prop.GetComponent<BreakablesScript>() != null))
                    prop.GetComponent<BreakablesScript>().TakeDamage(damage);
            }
        }
    }
}
