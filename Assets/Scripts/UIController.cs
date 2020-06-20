using SimpleInputNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{   //As just One UI in our game!
    public static UIController instance;
    public Slider healthSlider;
    public Slider xpSlider;
    public Text healthText;
    public Text coinText;
    public Text KillsText;
    public Text levelText;
    public Text remainingKeys;
    public Text floor;
    public GameObject deathScreen;
    public GameObject nextScreen;
    //Money and Kill Text UI
    public GameObject joystick;
    public Button dashButton;
    public Button attackButton;
    public Button changeWeaponButton;
    private int currWeaponPos = 0;
    //Next map Screen
    public Text NSMoneyText;
    public Text NSLevelText;
    public Text NSKillsText;
    public Text NSXpText;


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
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
           joystick.SetActive(true);
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            joystick.SetActive(false);
            //joystick.SetActive(true);
        }

        Button btn = dashButton.GetComponent<Button>();
        btn.onClick.AddListener(DashOnClick);
        Button btn2 = changeWeaponButton.GetComponent<Button>();
        btn2.onClick.AddListener(changeWeaponOnClick);
        Button btn3 = attackButton.GetComponent<Button>();
        btn3.onClick.AddListener(AttackOnClick);
        levelText.text="Lvl. 0";
    }
    void DashOnClick()
    {
        PlayerController.instance.startDash();
    }
    void changeWeaponOnClick()
    {
        if(currWeaponPos< PlayerController.instance.playerWeapons.Count-1)
        {
            currWeaponPos++;
        }
        else { currWeaponPos = 0;}
        PlayerController.instance.nextWeapon();
    }

    void AttackOnClick()
    {
        PlayerAttack.instance.attackEnemy();
    }
}
