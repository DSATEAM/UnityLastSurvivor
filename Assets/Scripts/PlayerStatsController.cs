using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{   public static PlayerStatsController instance;
    //Private stats variables
    private int Kills;
    private int Exp;
    private int Coins;
    public int maxExp = 25;
    private int level=0;
    protected static int Lvl1MaxExp = 50;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        updateExpLevelUI();
        updateKillsUI();
        updateCoinsUI();
        updateKeysUI();
        updateFloorUI();
    }
    void Update()
    {
       
    }
    public void setKills(int kill)
    {
        Kills =kill;
        updateKillsUI();
    }
    public void setExp(int experience)
    {
       
        Exp = experience;
        updateExpLevelUI();
    }
    public void setCoins(int coins)
    {
        Coins = coins;
        updateCoinsUI();
    }
    public int getCoins()
    {
        return Coins;
    }
    public void addCoins(int coins)
    {
        Coins += coins;
        updateCoinsUI();
    }
    public void addkills(int kills)
    {
        Kills += kills;
        updateKillsUI();
    }
    public int getKills()
    {
        return Kills;
    }
    public void addExperience(int exp)
    {
        Exp += exp;
        if (Exp >= maxExp)
        {
            Exp = Exp-maxExp;
            maxExp = maxExp*2;
            addLevel();
        }
        updateExpLevelUI();
    }
    public int getExperience()
    {
        return Exp;
    }
    
    public void addLevel()
    {
        level+=1;
        updateExpLevelUI();
    }
    public int getLevel()
    {
        return level;
    }
    public void setLevel(int level)
    {
        this.level = level;
        //Set Max EXP for the current 
        //maxExp =Convert.ToInt32(Lvl1MaxExp*Math.Pow(2,level+1));
        maxExp = (level+1)* Lvl1MaxExp;
        updateExpLevelUI();
    }
    public int factorial_Recursion(int number)
    {
        if (number == 1)
            return 1;
        else
            return number * factorial_Recursion(number - 1);
    }
    public void updateKillsUI()
    {
        UIController.instance.KillsText.text = "x " + Kills;
        UIController.instance.NSKillsText.text=Kills.ToString();
    }
    public void updateExpLevelUI()
    {
        UIController.instance.xpSlider.value = Exp;
        UIController.instance.xpSlider.maxValue = maxExp;
        UIController.instance.levelText.text = "Lvl. " + level;
        UIController.instance.NSXpText.text="Lvl. "+level;
    }
    public void updateCoinsUI()
    {
        UIController.instance.coinText.text = "x " + Coins;
        UIController.instance.NSMoneyText.text=Coins.ToString();
    }
    public void updateKeysUI()
    {
        UIController.instance.remainingKeys.text="Left: "+GameManager.instance.getRemainingKeys();
    }
    public void updateFloorUI()
    {
        UIController.instance.floor.text="Map Floor "+GameManager.instance.mapLevel;
        UIController.instance.NSLevelText.text=Convert.ToString(GameManager.instance.mapLevel+1);
    }
}
