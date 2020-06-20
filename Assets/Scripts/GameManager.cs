using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using System.Linq;
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int mapLevel = 0;
    private int[] mapSize;
    private char exitChar = 'X';
    private int remainingKeys=0;//Will change this to a random chest generated in the Generator for each map
    //And will automatically countdown when player opens one chest, when zero player can now exit using the Exit on the map
    private char objectSplitter = ';';
    private char subDataSplitter = ',';
    private int generatedkeyCount = 0;
    protected static int maxGeneratedMapSize = 128;
    protected static int minGeneratedMapSize = 16;
    //PlayerStats String: P,Level,Exp,Kills,Coins;Sword;axe;katana;baton;hammer where weapons has Damage,Hitrange,attackCooldown
    string type3player = "";
    string type3playerBak = "P,0,0,0,0;K,4,2,1.5,1.5;A,8,4,0.8,1.25";
    //Dungeon Wall = E & Floor = F
    //Dessert Wall = C & Floor = L
    //Swamp   Wall = S & Floor = A
    protected static string[] mapsTypes = new string[]{"EF","CL","SA"}; private int currMapTypeNum = 0;
    public List<string> type1MapsList =   new List<string>(); //Never Null Exception
    public List<string> type2ObjectsList = new List<string>();//Never Null Exception
    private bool hasAndroidData = false; //Never Null Exception
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else if( instance !=this)
        {
            Destroy(gameObject);
        }
        //To not delete GameManager as it contains the current state of the Game!
        DontDestroyOnLoad(gameObject);
        if (Application.platform == RuntimePlatform.Android)
        {
            hasAndroidData = getAndroidData();
        }
        else
        {
            initializeTestData();//Initializes with Test Data if not Android
        }
        InitGame(mapLevel);
    }
    private void Start()
    {
        //Get Android Data and put it to player, objects,map if running on android and available
        if (hasAndroidData)
        {
            PlayerStatsInit(type3player, true);
        }
        else
        {
            PlayerStatsInit(type3playerBak, true);
        }
    }
    private bool getAndroidData()
    {
        try { 
            AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            bool hasPlayerDataExtra = false;
            AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
            hasPlayerDataExtra = intent.Call<bool>("hasExtra", "playerData");
            if (hasPlayerDataExtra)
            {
                AndroidJavaObject extras = intent.Call<AndroidJavaObject>("getExtras");
                Debug.LogWarning("Recieved Player Stats and Weapons Before: "+type3player);
                type3player = extras.Call<string>("getString","playerData");
                Debug.LogWarning("Recieved Player Stats and Weapons After: " + type3player);
                Debug.LogWarning("Recieved Player Boolean state: " + hasPlayerDataExtra);
            }
            bool hasObjectDataExtra = false;
            hasObjectDataExtra = intent.Call<bool>("hasExtra", "objectData");
            if (hasObjectDataExtra)
            {
                type2ObjectsList.Clear();
                AndroidJavaObject extras = intent.Call<AndroidJavaObject>("getExtras");
                Debug.LogWarning("Object Type2 Size Before Add: "+type2ObjectsList.Count);
                type2ObjectsList.AddRange(extras.Call<string>("getString", "objectData").Split('/'));
                Debug.LogWarning("Object Type2 Size After Add: " + type2ObjectsList.Count);
            }
            bool hasMapDataExtra = false;
            hasMapDataExtra = intent.Call<bool>("hasExtra", "mapData");
            if (hasMapDataExtra)
            {
                type1MapsList.Clear();
                AndroidJavaObject extras = intent.Call<AndroidJavaObject>("getExtras");
                type1MapsList.AddRange(extras.Call<string>("getString", "mapData").Split('/'));
            }
            return hasPlayerDataExtra;
        }
        catch (Exception e)
        {
            Debug.Log("Exception on Recieving Data in Android--> Error: "+e.Message);
            return false;
        }
    }
    public void closeGame()
    {
        returnPlayerData();
        CloseUnityActivityFromAndroid();
    }
    private void returnPlayerData()
    {
        if (hasAndroidData)
        {
        //PlayerStats String: P,Level,Exp,Kills,Coins
        string playerStatsSendable = "P,"+PlayerStatsController.instance.getLevel()+","+PlayerStatsController.instance.getExperience()+"," + PlayerStatsController.instance.getKills() + "," +
            +PlayerStatsController.instance.getCoins() + "," + mapLevel;
        Debug.Log("Sending Player Stats: " + playerStatsSendable);
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("receivePlayerStats", playerStatsSendable);//recievePlayerStats is the Method in UnityPlayer
        }
    }
    private void CloseUnityActivityFromAndroid()
    {
        if (hasAndroidData)
        {
            //PlayerStats String: P,Level,Exp,Kills,Coins
            string playerStatsSendable = "P," + PlayerStatsController.instance.getLevel() + "," + PlayerStatsController.instance.getExperience() + "," + PlayerStatsController.instance.getKills() + "," +
                +PlayerStatsController.instance.getCoins() + "," +mapLevel;
            Debug.Log("Sending Player Stats: " + playerStatsSendable);
            AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("closeUnityPlayerActivity", playerStatsSendable);//recievePlayerStats is the Method in UnityPlayer
        }
    }
    void FixedUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                closeGame();
            }
        }
    }
    void PlayerStatsInit(string type3PlayerStatsAndWeapons,bool firstTime)
    {   
        //Test if the string is correct, for example last character is not a ; or ,
        if(type3PlayerStatsAndWeapons.EndsWith(";")|| type3PlayerStatsAndWeapons.EndsWith(","))
        {
            //Remove last invalid char
            type3PlayerStatsAndWeapons = type3PlayerStatsAndWeapons.Remove(type3PlayerStatsAndWeapons.Length - 1);
        }
        string[] statsAndWeapons = type3PlayerStatsAndWeapons.Split(objectSplitter);
        //PlayerStats String: P,Level,Exp,Kills,Coins
        if (firstTime)
        {
            string[] playerStats = statsAndWeapons[0].Split(subDataSplitter);
            Debug.Log($"Player Stats Before Conversion:P: {playerStats[0]} level: {playerStats[1]} exp: {playerStats[2]} kills: {playerStats[3]} coins: {playerStats[4]}");
            int level,exp,kills,coins;
            int.TryParse(playerStats[1], out level);
            int.TryParse(playerStats[2], out exp);
            int.TryParse(playerStats[3], out kills);
            int.TryParse(playerStats[4], out coins);
            PlayerStatsController.instance.setLevel(level);
            PlayerHealthController.instance.addDefense(level);
            PlayerStatsController.instance.setExp(exp);
            PlayerStatsController.instance.setKills(kills);
            PlayerStatsController.instance.setCoins(coins);
            Debug.Log("Player Stats After Conversion:"+" level: "+level+" exp: " +exp+" kills: " +kills+" coins: "+coins);
        }
        for (int i = 1; i <= statsAndWeapons.Length - 1; i++)
        {
            //For each weapon in player add them to the controller and assign damage to each weapon
            string[] WeaponStats = statsAndWeapons[i].Split(subDataSplitter);
            _ = char.TryParse(WeaponStats[0], out char WeaponName);
            _ =int.TryParse(WeaponStats[1], out int damage);
            _ = int.TryParse(WeaponStats[2], out int defense);
            _ = float.TryParse(WeaponStats[3], out float range);
            _ = float.TryParse(WeaponStats[4], out float attackCooldown);
            //Debug.Log("Printing vals: "+ statsAndWeapons[i] );
            Debug.Log($"Name {WeaponName}dmg {damage} defense {defense}range {range}cool {attackCooldown}");
            //Add this values to Controller for use and also the weapon
            PlayerController.instance.addWeaponWithStats(WeaponName, damage, range, attackCooldown);
            PlayerHealthController.instance.addDefense(defense);
            Debug.Log($"Weapon Adding to Player Inventory (Char) {WeaponName}");
        }
    }
    private int countRemaingKeys(string type2ObjectStr)
    {
        char[] chars = type2ObjectStr.ToArray();  // Get the Characters(letters) from string

        var result = (from c in chars
                      where c.Equals('K')
                      select c).Count();
        return result;
    }
    void InitGame(int mapIndx)
    {
        boardScript.setMapSeparators(objectSplitter,subDataSplitter);
        boardScript.SetupScene(type1MapsList[mapIndx],type2ObjectsList[mapIndx]);
        remainingKeys = countRemaingKeys(type2ObjectsList[mapIndx]);
        mapSize = checkMapSize(type1MapsList[mapIndx]);
    }
    private int[] checkMapSize(string type1Map)
    {   string[] tmp = type1Map.Split(objectSplitter);
        int y = tmp.Length;
        int x = tmp.Aggregate(string.Empty, (seed, f) => f.Length > seed.Length ? f : seed).Length-1;
        return new int[]{x,y};
    }
    private void initializeTestData()
        //Type2 Object Placement String with Default Player Values(Zeroes)
    {   string type2objects = "P,1,1;C,16,1,5,5,10;T,3,2,10;K,2,2";//Item,X,Y,Value
        string type1map = "EEEEEEEEEEEEEEEEEEEEEEEE;" +
                          "EFFFFFFFFFFFFFFFFFFFFFFE;" +
                          "EFFFFFMMFMFFFFFFFFFFFFFX;" +
                          "EEEEEEEEEEEEEEEEEEEEEEEE";
        //Tutorial Map
        type1MapsList.Add(type1map);
        type2ObjectsList.Add(type2objects);
    }
    public  int getRemainingKeys()
    {
        return remainingKeys;
    }
    public  void subtractRemainingKeys()
    {
        if (remainingKeys > 0)
        {
            remainingKeys--;
            if (remainingKeys == 0)
            {
                DoorScript.instance.setDoorOpen();
            }
        }
        else
        {
            //OpenDoor
            DoorScript.instance.setDoorOpen();
        }
    }
    
    
public int[] getMapSize()
    {
        return mapSize;
    }
    public void nextMap()
    {   
        //Reload Scene and delete all object except the GameManager Object Prefab Clone & UI Controller
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    private void OnLevelWasLoaded(int level)
    {   
        //Loop Back Maps from the List
        mapLevel++; //Indx is kind off level indicator
        if (mapLevel >= type1MapsList.Count)
        {
            string type1mapGenerated, type2objectsGenerated;
            currMapTypeNum++;
            if(currMapTypeNum >= mapsTypes.Length) { currMapTypeNum = 0;}
            string temp = mapsTypes[currMapTypeNum];
            generateLevel(temp[0], temp[1],out type1mapGenerated, out type2objectsGenerated,mapLevel);
            type1MapsList.Add(type1mapGenerated);
            type2ObjectsList.Add(type2objectsGenerated);
        }
        returnPlayerData();
        InitGame(mapLevel);
        PlayerStatsInit(type3player, false);
        PlayerStatsController.instance.addCoins(mapLevel*10);
        //Update UIs with proper STATS
        PlayerHealthController.instance.addDefense(5);
        PlayerStatsController.instance.updateCoinsUI();
        PlayerStatsController.instance.updateExpLevelUI();
        PlayerStatsController.instance.updateKillsUI();
        PlayerStatsController.instance.updateKeysUI();
        PlayerStatsController.instance.updateFloorUI();
        //Set Player Weapons
        UIController.instance.nextScreen.SetActive(false);
    }

    //Generate the Map with full size empty 
    private string[] generateEmptyDungeon(char Tile, int sizeX, int sizeY)
    {
        string[] map;
        //Create the Map
        string row = new string(Tile, sizeX); //Generates row of size in X direction
        map = Enumerable.Repeat(row, sizeY).ToArray();//Generates the Size of Dungeon in Y direction
        return map;
    }
    private void generateLevel(char Wall,char Floor,out string type1MapGenerated,out string Type2ObjectGenerated, int level)
    {
        //Generate Random Map when no more maps remaining in the list
        int[] randomMapSize = new int[2] { UnityEngine.Random.Range(minGeneratedMapSize, maxGeneratedMapSize), UnityEngine.Random.Range(minGeneratedMapSize, maxGeneratedMapSize) };
        Vector2Int playerPos = new Vector2Int(UnityEngine.Random.Range(2, randomMapSize[0] - 2), UnityEngine.Random.Range(2, randomMapSize[1] - 2));
        type1MapGenerated = generateMap(Wall, Floor,level, randomMapSize[0] * randomMapSize[1], randomMapSize, playerPos,out Type2ObjectGenerated);
        mapSize = randomMapSize;
    }
    public string generateMap(char Edge, char Floor, int level,int steps, int[] mapSize,Vector2Int startingPlayerPos, out string type2ObjectGenerated)
    {
        generatedkeyCount=0;
       // remainingChest = 0;
        List<Vector2> objectsPositions = new List<Vector2>();
        string[] map = generateEmptyDungeon(Edge, mapSize[0], mapSize[1]);
        Vector2Int currPosition = startingPlayerPos; Vector2Int newPos = startingPlayerPos;
        type2ObjectGenerated = "P," + startingPlayerPos.x + "," + startingPlayerPos.y;
        int r; int step = 0;//Starting position 0,0 and only from border to border never outside and 0 to maxSizeX and same for maxSizeY
        while (step <= steps)
        {
            r = UnityEngine.Random.Range(0, 4);
            switch (r)
            {
                case 0: //TOP
                    newPos += new Vector2Int(0, 1);
                    if (newPos.y < mapSize[1] - 1)
                    {
                        //Only move up if we aren't at the Upper Border
                        //Replace the Edge wall with floor at the new Position on the Map
                        map[newPos.y] = new StringBuilder(map[newPos.y]) { [newPos.x] = Floor }.ToString();
                        currPosition = newPos;
                    }
                    else
                    {
                        currPosition = startingPlayerPos;
                    }
                    newPos = currPosition;
                    break;
                case 1: //BOTTOM
                    newPos += new Vector2Int(0, -1);
                    if (newPos.y > 0)
                    {
                        //Only move down if we aren't at the Lower Border
                        //Replace the Edge wall with floor at the new Position on the Map
                        map[newPos.y] = new StringBuilder(map[newPos.y]) { [newPos.x] = Floor }.ToString();
                        currPosition = newPos;
                    }
                    else
                    {
                        currPosition = startingPlayerPos;
                    }
                    newPos = currPosition;
                    break;
                case 2: //LEFT
                    newPos += new Vector2Int(-1, 0);
                    if (newPos.x > 0)
                    {
                        //Only move Left if we aren't at the Left Border
                        map[newPos.y] = new StringBuilder(map[newPos.y]) { [newPos.x] = Floor }.ToString();
                        currPosition = newPos;
                    }
                    else
                    {
                        currPosition = startingPlayerPos;
                    }
                    newPos = currPosition;
                    break;
                case 3: //RIGHT
                    newPos += new Vector2Int(1, 0);
                    if (newPos.x < mapSize[0] - 1)
                    {
                        //Only move Right if we aren't at the Right Border
                        //Replace the Edge wall with floor at the new Position on the Map
                        map[newPos.y] = new StringBuilder(map[newPos.y]) { [newPos.x] = Floor }.ToString();
                        currPosition = newPos;
                    }
                    else
                    {
                        currPosition = startingPlayerPos;
                    }
                    newPos = currPosition;
                    break;
            }
            if(step < steps && !objectsPositions.Contains(currPosition)) {
                type2ObjectGenerated = randomObject(level, currPosition, type2ObjectGenerated);
                objectsPositions.Add(currPosition);
            }
            else if(step == steps) {
                //We are at the last step and so we will add the Exit Here
                type2ObjectGenerated = type2ObjectGenerated + ";" + exitChar + "," + currPosition.x + "," + currPosition.y;
            }
            step++;
        }
        //Border always edge
        map[0] = new StringBuilder(map[0]) { [0] = Edge }.ToString(); map[1] = new StringBuilder(map[1]) { [0] = Edge }.ToString();
        map[0] = new StringBuilder(map[0]) { [1] = Edge }.ToString(); map[0] = new StringBuilder(map[0]) { [2] = Edge }.ToString();
        //Player Starting position (1,1) always floor,Just in Case something bizzare happens
        map[startingPlayerPos.y] = new StringBuilder(map[startingPlayerPos.y]) { [startingPlayerPos.x] = Floor }.ToString();
        Array.Reverse(map);
       // Debug.Log("Objects Generated "+ type2ObjectGenerated);
        return string.Join(objectSplitter.ToString(), map);
    }
    private string randomObject(int level, Vector2Int objectPosition,string appendableType2Object)
    {
        //ObjectChar,PosX,PosY,+Characteristis of each item
        // TODO: FINISH THIS!
        
        Char objChar; string Type2randomObjStr = "";
        int val1, val2, val3;
        float val1f;
        
        bool Objectadded = false;
        int r = UnityEngine.Random.Range(0,75);
        switch (r)
        {
            case 0:
                objChar = 'I';//IceZombie 
                Objectadded = true;
                //Enemy has(int) Damage,(int)Health,(int)Exp
                val1 = UnityEngine.Random.Range(level,(level)+5);//Damage
                val2 = UnityEngine.Random.Range(level, (level + 8));//Health
                val3 = (level + 3) * 2;//Experience
                //Now we add the Values to the Enemy such as Health,Damage,Exp
                Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y + "," + val2 + "," + val1 + "," + val3;
                break;
            case 3:
                objChar = 'C';//Chort
                Objectadded = true;
                //Enemy has(int) Damage,(int)Health,(int)Exp
                val1 = UnityEngine.Random.Range(level, (level)+8);//Damage
                val2 = UnityEngine.Random.Range(level, (level + 5));//Health
                val3 = (level + 2) * 2;//Experience
                //Now we add the Values to the Enemy such as Health,Damage,Exp
                Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y + "," + val2 + "," + val1 + "," + val3;
                break;
            case 6:
                objChar = 'S'; //Slime
                Objectadded = true;
                //Enemy has(int) Damage,(int)Health,(int)Exp
                val1 = UnityEngine.Random.Range(level, (level + 12)); //Damage
                val2 = UnityEngine.Random.Range(level, (level + 2)); //Health
                val3 = (level + 1) * 2;//Experience
                //Now we add the Values to the Enemy such as Health,Damage,Exp
                Type2randomObjStr = objChar+","+objectPosition.x+","+objectPosition.y+","+val2+","+val1+","+val3;
                break;
            case 7:
                objChar = 'N'; //Skeleton
                Objectadded = true;
                //Enemy has(int) Damage,(int)Health,(int)Exp
                val1 = UnityEngine.Random.Range(level, (level)); //Damage
                val2 = UnityEngine.Random.Range(level, (level + 2)); //Health
                val3 = (level + 1) * 2;//Experience
                //Now we add the Values to the Enemy such as Health,Damage,Exp
                Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y + "," + val2 + "," + val1 + "," + val3;
                break;
            case 9:
                objChar = 'x';//Cactus
                if(currMapTypeNum==1) //Dessert Only
                {    Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y;
                    Objectadded = true; 
                }
                break;
            case 12:
                objChar = 'l';//log
                if (currMapTypeNum == 2) //Swamp only
                    {Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y;Objectadded = true;}
                break;
            case 15:
                objChar = 't'; //Tree
                if (currMapTypeNum == 2) //Swamp only
                    {Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y; Objectadded = true;}
                    break;
            case 18:
                objChar = 'c';//Coin
                Objectadded = true;
                //Coin has Amount(int)
                val1 = UnityEngine.Random.Range(level*2, (level*2) * 2); //Coint Amount
                Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y + "," + val1;
                break;
            case 20:
                objChar = 'c';//Coin
                Objectadded = true;
                //Coin has Amount(int)
                val1 = UnityEngine.Random.Range(level * 2, (level * 2) * 2); //Coint Amount
                Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y + "," + val1;
                break;
            case 21:
                objChar = 'b'; //Bomb
                Objectadded = true;
                //Bomb has(int) attackRange,(int)activationRange,(float)Timer,Damage(int)
                val1 = UnityEngine.Random.Range(5, (level));//Damage
                val2 = UnityEngine.Random.Range(4, 10);//attack Range
                val3 = UnityEngine.Random.Range(4, 10);//activationRange
                val1f = 2.0f;//Timer
                //Now we add the Values to the Bomb such as attackRange,activationRange,BombTime(0.23),Damage
                Type2randomObjStr = objChar+","+objectPosition.x+","+objectPosition.y+","+val2+","+val3+","+val1f+","+val1;
                break;
            case 24:
                objChar = 'h';//Medkit
                Objectadded = true;
                //Medkit has heal amount(int)
                val1 = UnityEngine.Random.Range(5, (level+10));//Heal
                Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y + "," + val1;
                break;
            case 26:
                objChar = 'h';//Medkit
                Objectadded = true;
                //Medkit has heal amount(int)
                val1 = UnityEngine.Random.Range(5, (level + 10));//Heal
                Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y + "," + val1;
                break;
            case 27:
                objChar = 'T';
                Objectadded = true;
                //Treasure Chest has(int) Item Amount in our case coins amount more than normal coins
                val1 = UnityEngine.Random.Range(level * 2, (level * 50)); //Item Amount
                //remainingChest++;
                Type2randomObjStr = objChar + "," +objectPosition.x + ","+objectPosition.y + "," + val1;
                break;
            case 29:
                objChar = 'K';
                if (generatedkeyCount <= level) 
                { 
                    Objectadded = true;
                    //Keys just has position
                    Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y;
                    generatedkeyCount++;
                }
                break;
            case 30:
                objChar = 'B';//Barrel
                if (currMapTypeNum != 2) //If dungeon or dessert only
                {Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y; Objectadded = true; }
                break;
            case 33:
                objChar = 'W';//Water with SlowController
                if (currMapTypeNum == 2) //If Swamp only
                { Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y; Objectadded = true; }
                break;
            case 35:
                objChar = 'W';//Water with SlowController
                if (currMapTypeNum == 2) //If Swamp only
                { Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y; Objectadded = true; }
                break;
            case 38:
                objChar = 'i';//Ice with SliderController
                if (currMapTypeNum != 1) //If Swamp or Dungeon
                { Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y; Objectadded = true; }
                break;
            case 41:
                objChar = 'i';//Ice with SliderController
                if (currMapTypeNum != 1) //If Swamp or dungeon
                { Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y; Objectadded = true; }
                break;
            case 43:
                objChar = 'Q';//Spike Trap
                if (currMapTypeNum != 2) //If dungeon or Dessert
                { Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y; Objectadded = true; }
                break;
            case 46:
                objChar = 'L';//Lava Trap
                if (currMapTypeNum != 2) //If dungeon or Dessert
                { Type2randomObjStr = objChar + "," + objectPosition.x + "," + objectPosition.y; Objectadded = true; }
                break;
            default :
                    //Do nothing as nothing shall be added
                    Objectadded = false;
                break;
        }
        if (Objectadded)
        {
            appendableType2Object = appendableType2Object+ ";"+ Type2randomObjStr;
        }
        //Also Remember if chest is added as a item, increase the global remaining chest counter
        return appendableType2Object;
    }
}
