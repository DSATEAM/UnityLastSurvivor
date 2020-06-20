using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    float slideForce = 14;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController.instance.setSliding(slideForce, true);

            // ExecuteAfterTime(0.5f);
        }
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().setSliding(slideForce, true);
        }
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        PlayerController.instance.setSliding(0, false);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //PlayerController.instance.setSliding(0, false);
            StartCoroutine(ExecuteAfterTime(0.3f));
            // ExecuteAfterTime(0.5f);
        }
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().setSliding(0, false);
        }
    }
}
