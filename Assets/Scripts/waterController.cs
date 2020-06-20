using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterController : MonoBehaviour
{
    public float slowMoveSpeed = 1.5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController.instance.setMoveSpeed(slowMoveSpeed);

            // ExecuteAfterTime(0.5f);
        }
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().setMoveSpeed(slowMoveSpeed);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController.instance.setMoveSpeed(PlayerController.instance.backedMoveSpeed);
            // ExecuteAfterTime(0.5f);
        }
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().setMoveSpeed(collision.GetComponent<EnemyController>().backedMoveSpeed);
        }
    }
}
