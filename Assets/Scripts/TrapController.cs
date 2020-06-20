using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public int damage = 5;
    public Animator animator;
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("isIdle", true);
    }

    void Start()
    {
        animator.SetBool("isIdle", true);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.SetBool("isIdle", false);
            PlayerHealthController.instance.DamagePlayer(damage);
           // ExecuteAfterTime(0.5f);
            StartCoroutine(ExecuteAfterTime(0.4f));
        }
        if (collision.tag == "Enemy")
        {
            animator.SetBool("isIdle", false);
            collision.GetComponent<EnemyController>().TakeDamage(damage);
            //ExecuteAfterTime(0.5f);
            StartCoroutine(ExecuteAfterTime(0.4f));
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer(damage);
        }
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().TakeDamage(damage);
        }
    }
}
