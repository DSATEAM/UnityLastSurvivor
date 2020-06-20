using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private int coinAmount = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        //Max Range is always Max+1 cause unity!
        coinAmount = UnityEngine.Random.Range(1, 51);
    }
    public void InitializeCoin(int coinAmount) {this.coinAmount =coinAmount; }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerStatsController.instance.addCoins(coinAmount);
            Destroy(gameObject, 0.1f);
        }
    }
}
