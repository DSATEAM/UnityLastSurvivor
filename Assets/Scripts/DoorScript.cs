using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public static DoorScript instance = null;
    public BoxCollider2D myCollider;
    public SpriteRenderer myRenderer;
    public Sprite openDoorSprite;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            if (GameManager.instance.getRemainingKeys() <= 0)
            {
                UIController.instance.nextScreen.SetActive(true);
                GameManager.instance.nextMap();
            }
        }
    }
    public void setDoorOpen()
    {
        myRenderer.sprite = openDoorSprite;
    }
 
}
