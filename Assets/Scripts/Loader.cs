using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject UiCanvas;
    private void Awake()
    {
        if(GameManager.instance ==null)
        {
            Instantiate(gameManager);
        }
        Instantiate(UiCanvas);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
