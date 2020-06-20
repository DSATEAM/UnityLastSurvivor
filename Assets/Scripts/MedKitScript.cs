using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKitScript : MonoBehaviour
{   private int heal = 0;
    // Start is called before the first frame update
    private void Awake()
    {//Max Range is always Max+1 cause unity!
        //Give Player Heal Amount Randomly
        heal = UnityEngine.Random.Range(1,11);
    }
    public void InitializeMedKit(int heal) { this.heal = heal;}
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.HealPlayer(heal);
            Destroy(gameObject, 0.1f);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.HealPlayer(heal);
            Destroy(gameObject, 0.1f);
        }
    }
}
