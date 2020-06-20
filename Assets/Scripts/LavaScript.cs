using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScript : MonoBehaviour
{
    public int damage = 15;
   // public Animator animator;
   private  bool isPlayerInside = false;
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInside)
        {
            PlayerHealthController.instance.DamagePlayer(damage);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
           isPlayerInside = true;
        }
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().TakeDamage(damage);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerInside = false;
        }
    }


}
