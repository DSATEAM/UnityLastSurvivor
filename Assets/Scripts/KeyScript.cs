using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.subtractRemainingKeys();
            PlayerStatsController.instance.updateKeysUI();
            Destroy(gameObject, 0.1f);
        }
    }
}
