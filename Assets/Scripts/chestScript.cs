using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestScript : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer spriteRenderer;
    public Sprite openChestSprite;
    public int itemAmount = 50;
    bool isClosed=true;

    void Start()
    {
        
    }

    public void InitializeChest(int itemAmount) {this.itemAmount = itemAmount; }
    private void OnTriggerStay2D(Collider2D colision)
    {
      if (colision.gameObject.tag == "Player" && isClosed)
      {
        spriteRenderer.sprite = openChestSprite;
          PlayerStatsController.instance.addCoins(itemAmount);
          isClosed=false;
      }

    }
}
