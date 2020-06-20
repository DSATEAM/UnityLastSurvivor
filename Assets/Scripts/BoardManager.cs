using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    //Static GameObject for Player
    public GameObject[] FloorTiles;
    public GameObject[] EdgeWalls;
    public GameObject[] MidWalls;
    public GameObject[] Doors;
    //Interactable GameObjects with Players
    public GameObject Chort;
    public GameObject IceZombie;
    public GameObject Slime;
    public GameObject SpikeTrap;
    public GameObject LavaTrap;
    public GameObject Skeleton;
    public GameObject Player;
    //Interactable GameObject with Players just Single Prefabs
    public GameObject MedKit;
    public GameObject Coin;
    public GameObject Bomb;
    public GameObject Key;
    public GameObject TreasureChest;
    public GameObject Barrel;
    public GameObject Cactus;
    public GameObject tree;
    public GameObject log;
    public GameObject Ice;

    //Default unless modified
    private char objectSplitter = ';'; 
    private char subDataSplitter = ',';
    private string type1Map;
    private string type2Objects;

    void InstantiateObjectsFromType2(string objectsLayoutStr)
    {
        string[] mapLayout = objectsLayoutStr.Split(objectSplitter);
        for (int i = 0; i<mapLayout.Length ; i++)
        {
            //Need to get the correct gameObject from the String
            AutoObjectInstantiate(mapLayout[i].Split(subDataSplitter));
        }
    }
    private GameObject returnMapObject(char objChar)
    {
        GameObject toReturnObj = null;
        switch (objChar)
        {
            case 'E':
                toReturnObj = EdgeWalls[UnityEngine.Random.Range(0, 3)];//edge walls of dungeon
                break;
            case 'F':
                toReturnObj = FloorTiles[UnityEngine.Random.Range(0, 13)];//floor tile of dungeon
                break;
            case 'M':
                toReturnObj = MidWalls[0];//dungeon midwalls
                break;
            case 'C':
                toReturnObj = EdgeWalls[UnityEngine.Random.Range(4, 6)];//desert edge walls
                break;
            case 'L':
                toReturnObj = FloorTiles[14];//desert floor tiles
                break;
            case 'R':
                toReturnObj = MidWalls[1];//desert midwall tiles
                break;
            case 'S':
                toReturnObj=EdgeWalls[UnityEngine.Random.Range(6,8)];//swamp edge walls
                break;
            case 'W':
                toReturnObj=MidWalls[2]; //Water swamp
                break;
            case 'A':
                toReturnObj=FloorTiles[15]; //Swamp floor
                break;
            case 'i':
                toReturnObj = Ice;//Ice Floor
                break;
            case 'X':
                toReturnObj=Doors[0];//exit
                break;
            case 'Q':
                toReturnObj = SpikeTrap;//SpikeTrap
                break;
        }
        return toReturnObj;
    }
    void AutoObjectInstantiate(String[] ObjectSplitArr)
    {   
        Char objChar = Convert.ToChar(ObjectSplitArr[0]);//Every Object Must Have a Name, X,Y + Extras
        int x = int.Parse(ObjectSplitArr[1]); int y = int.Parse(ObjectSplitArr[2]); int val1 = 0, val2 = 0, val3 = 0,val4=0;
        float val1f = 0.0f, val2f = 0.0f, val3f = 0.0f, val4f = 0.0f, val5f = 0.0f;
        GameObject toReturnObj = null;
        GameObject currentObject;
        //All of the Objects String must be maintained!
        switch (objChar)
        {
            case 'I':
                toReturnObj = IceZombie;//IceZombie
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                //Now we add the Values to the Enemy such as Health,Damage,Exp
                if (ObjectSplitArr.Length == 6)
                {
                    val2 = int.Parse(ObjectSplitArr[3]);
                    val1 = int.Parse(ObjectSplitArr[4]);
                    val3 = int.Parse(ObjectSplitArr[5]);
                    currentObject.GetComponent<EnemyController>().InitializeEnemy(val1, val2, val3);
                }
                break;
            case 'C':
                toReturnObj = Chort;//Chort
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                //Now we add the Values to the Enemy such as Health,Damage,Exp
                if (ObjectSplitArr.Length == 6)
                {
                    val2 = int.Parse(ObjectSplitArr[3]);
                    val1 = int.Parse(ObjectSplitArr[4]);
                    val3 = int.Parse(ObjectSplitArr[5]);
                    currentObject.GetComponent<EnemyController>().InitializeEnemy(val1, val2, val3);
                }
                break;
            case 'N':
                toReturnObj = Skeleton;//Skeleton
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                //Now we add the Values to the Enemy such as Health,Damage,Exp
                if (ObjectSplitArr.Length == 6)
                {
                    val2 = int.Parse(ObjectSplitArr[3]);
                    val1 = int.Parse(ObjectSplitArr[4]);
                    val3 = int.Parse(ObjectSplitArr[5]);
                    currentObject.GetComponent<EnemyController>().InitializeEnemy(val1, val2, val3);
                }
                break;
            case 'S':
                toReturnObj = Slime;//Slime
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                //Now we add the Values to the Enemy such as Health,Damage,Exp
                if (ObjectSplitArr.Length == 6)
                {
                    val2 = int.Parse(ObjectSplitArr[3]);
                    val1 = int.Parse(ObjectSplitArr[4]);
                    val3 = int.Parse(ObjectSplitArr[5]);
                    currentObject.GetComponent<EnemyController>().InitializeEnemy(val1, val2, val3);
                }
                break;
            case 'x':
                toReturnObj = Cactus;
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                break;
            case 'l':
                toReturnObj = log;
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                break;
            case 't':
                toReturnObj = tree;
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                break;
            case 'c':
                toReturnObj = Coin;
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                //Now we add the Values to the Coin --> Coin Amount
                val1 = int.Parse(ObjectSplitArr[3]);
                currentObject.GetComponent<CoinScript>().InitializeCoin(val1);
                break;
            case 'b':
                toReturnObj = Bomb;
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                //Now we add the Values to the Bomb such as attackRange,activationRange,BombTime(0.23),Damage
                if (ObjectSplitArr.Length == 7)
                {
                    val1 = int.Parse(ObjectSplitArr[3]);
                    val2 = int.Parse(ObjectSplitArr[4]);
                    val1f = float.Parse(ObjectSplitArr[5]);
                    val4 = int.Parse(ObjectSplitArr[6]);
                    currentObject.GetComponent<BombScript>().InitializeBomb(val1, val2, val1f, val4);
                }
                break;
            case 'h':
                toReturnObj = MedKit;
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                //Now we add the Values to the Medkit --> Heal(int)
                if (ObjectSplitArr.Length == 4)
                {
                    val1 = int.Parse(ObjectSplitArr[3]);
                    currentObject.GetComponent<MedKitScript>().InitializeMedKit(val1);
                }
                break;
            case 'T':
                toReturnObj = TreasureChest;
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                //Now we add the Values to the Treasure Check such as ItemAmount lots of it!
                if (ObjectSplitArr.Length == 4)
                {
                    val1 = int.Parse(ObjectSplitArr[3]);
                    currentObject.GetComponent<chestScript>().InitializeChest(val1);
                }
                break;
            case 'W':
                toReturnObj = MidWalls[2]; //Water in swamp
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                break;
            case 'Q':
                toReturnObj = SpikeTrap;//SpikeTrap
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                break;
            case 'L':
                toReturnObj = LavaTrap;//LavaTrap
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                break;
            case 'B':
                toReturnObj = Barrel;
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                break;
            case 'K':
                toReturnObj = Key;
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                break;
            case 'i':
                toReturnObj = Ice;
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                break;
            case 'P':
                toReturnObj = Player;
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                //Now we add the Values to the Player such as
                //float dashSpeed,float dashLength, float dashCooldown, float dashInvincibleTime, float attackRange
                if (ObjectSplitArr.Length == 8)
                {
                    val1f = float.Parse(ObjectSplitArr[3]);
                    val2f = float.Parse(ObjectSplitArr[4]);
                    val3f = float.Parse(ObjectSplitArr[5]);
                    val4f = float.Parse(ObjectSplitArr[6]);
                    val5f = float.Parse(ObjectSplitArr[7]);
                    currentObject.GetComponent<PlayerController>().InitializePlayer(val1f, val2f, val3f, val4f, val5f);
                }
                break;
            case 'X':
                toReturnObj = Doors[0];//exit
                currentObject = Instantiate(toReturnObj, new Vector3(x, y), Quaternion.identity);
                //Now we add the Values to the Exit such as Remaing Chest count in Exit

                break;
        }
    }
   

    private void InstantiateMapFromType1(String mapBaseStr)
    {
        String[] mapLayout = mapBaseStr.Split(objectSplitter);
        String singleCharObjects;
        for (int j = 0; j < mapLayout.Length; j++)
        {
            singleCharObjects = mapLayout[j];
            for (int i=0;i< singleCharObjects.Length; i++)
            {

                //Need to get the correct gameObject from the String if space than don't put anything!
                if (!singleCharObjects[i].Equals(' ')) {
                    GameObject tileChoice = returnMapObject(singleCharObjects[i]);
                    Instantiate(tileChoice, new Vector3(i, mapLayout.Length - j - 1), Quaternion.identity);
                }
                
            }

        }
    }
    public void setMapSeparators(char objectSplitter, char subDataSplitter)
    {
        this.objectSplitter = objectSplitter;
        this.subDataSplitter = subDataSplitter;
    }
    public void SetupScene(string Type1Map, string Type2Objects)
    {
        type1Map = Type1Map;
        type2Objects= Type2Objects;
        InstantiateMapFromType1(type1Map);
        InstantiateObjectsFromType2(type2Objects);

    }
}
