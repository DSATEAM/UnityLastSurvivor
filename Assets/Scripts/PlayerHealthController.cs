using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    public int currentHealth;
    public int maxHealth =100;
    public float damageInvincibilityLength = 0.7f;
    private float invincibilityCounter;
    private bool isAlreadyVisible = true;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString()+"/"+maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibilityCounter >= 0)
        {
            invincibilityCounter -= Time.deltaTime;
        }
        if (!isAlreadyVisible && invincibilityCounter<=0)
        {
            PlayerController.instance.changeTransparency(1f);
            isAlreadyVisible =true;
        }
    }
    public void HealPlayer(int heal)
    {
        currentHealth = currentHealth + heal;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        updateUI();
    }
    public void DamagePlayer(int damage)
    {
        if (invincibilityCounter <= 0)
        {   
            MakeInvincible(damageInvincibilityLength);
            //PlayerController.instance.changeTransparency(0.25f);
            currentHealth = currentHealth - damage;
            updateUI();
            //StartCoroutine(waitCoroutine(damageInvincibilityLength));
        }
        
        
    }

    private void updateUI()
    {
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        if (currentHealth <= 0)
        {
            PlayerController.instance.gameObject.SetActive(false);
            UIController.instance.deathScreen.SetActive(true);
            StartCoroutine(ExecuteAfterTime(2.5f));
        }
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.instance.closeGame();
    }
    public void addDefense(int defense)
    {
        maxHealth = maxHealth + defense;
        updateUI();
    }
   
    public void MakeInvincible(float lengthInvincibility)
    {
        invincibilityCounter = lengthInvincibility;
        PlayerController.instance.changeTransparency(0.25f);
        isAlreadyVisible = false;
        //StartCoroutine(waitCoroutine(lengthInvincibility));
    }
}
