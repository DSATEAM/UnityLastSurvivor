using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablesScript : MonoBehaviour
{

    //Explosion GamePrefab
    public GameObject explosionEffect;
    public int health = 2;
    public int exp = 20;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            PlayerStatsController.instance.addExperience(exp);
            GameObject explosionObj = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosionObj, 0.33f);
            Destroy(gameObject);
        }
    }
}
