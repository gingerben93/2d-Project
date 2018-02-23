using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int sideQuestCounter { get; set; }
    public bool sideQuestBool { get; set; }

    //for if game needs to load test
    public bool dontLoadTheGame;

    //all canvas elements
    private CanvasGroup InvMenuCanvas;
    private CanvasGroup StartMenuCanvas;
    private CanvasGroup NotificationCanvas;
    private CanvasGroup SkillMenuCanvas;
    private CanvasGroup QuestMenuCanvas;
    private CanvasGroup SelectBarCanvas;
    private CanvasGroup MagicMenuCanvas;

    //should get ref to this is start // change later
    private Text NotificationTxt;

    public bool questTravel { get; set; }
    public string mapSeed { get; set; }

    public static GameController GameControllerSingle;

    void Awake()
    {
        if (GameControllerSingle == null)
        {
            DontDestroyOnLoad(gameObject);
            GameControllerSingle = this;
        }
        else if (GameControllerSingle != this)
        {
            Destroy(gameObject);
        }

        if (GameLoader.GameLoaderSingle)
        {
            dontLoadTheGame = false;
            Destroy(GameLoader.GameLoaderSingle.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        //don't destroy on load objects
        DontDestroyOnLoad(GameObject.Find("Canvases"));
        //DontDestroyOnLoad(GameObject.Find("WorldObject"));
        //DontDestroyOnLoad(GameObject.Find("GrapplingHookParts"));

        //assign skill functions

        //jump skills
        GameObject.Find("GrapplingHookSkill").GetComponent<Button>().onClick.AddListener(delegate { LearnGrapplingHookSkill(); });
        GameObject.Find("HookResetJumps").GetComponent<Button>().onClick.AddListener(delegate { LearnHookResetJumps(); });

        //grappling hook skills
        GameObject.Find("DoubleJump").GetComponent<Button>().onClick.AddListener(delegate { LearnDoubleJump(); });
        GameObject.Find("TripleJump").GetComponent<Button>().onClick.AddListener(delegate { LearnTripleJump(); });

        //dash
        GameObject.Find("DashSkill").GetComponent<Button>().onClick.AddListener(delegate { LearnDashSkill(); });

        //melee skills
        GameObject.Find("DashSkillMelee").GetComponent<Button>().onClick.AddListener(delegate { LearnDashSkillMelee(); });

        //slide skill
        GameObject.Find("SlideSkillLearn").GetComponent<Button>().onClick.AddListener(delegate { LearnSlideSkill(); });
        GameObject.Find("SlideJumpSkill").GetComponent<Button>().onClick.AddListener(delegate { LearnSlideJumpSkill(); });
        GameObject.Find("SlideSteerSkill").GetComponent<Button>().onClick.AddListener(delegate { LearnSlideSteerSkill(); });

        //attract passive skill
        GameObject.Find("AttractPassive").GetComponent<Button>().onClick.AddListener(delegate { LearnAttractPassive(); });

        //assign buttons functions for menu
        GameObject.Find("InventoryButton").GetComponent<Button>().onClick.AddListener(delegate { InventoryOnButton(); });
        GameObject.Find("SkillsButton").GetComponent<Button>().onClick.AddListener(delegate { SkillMenuOnButton(); });
        GameObject.Find("QuestButton").GetComponent<Button>().onClick.AddListener(delegate { QuestMenuOnButton(); });
        GameObject.Find("MagicButton").GetComponent<Button>().onClick.AddListener(delegate { MagicMenuOnButton(); });

        //if side quest counter is on
        sideQuestBool = false;

        //on all the time 
        NotificationCanvas = GameObject.Find("NotificationCanvas").GetComponent <CanvasGroup>();

        //esc menu
        StartMenuCanvas = GameObject.Find("StartMenuCanvas").GetComponent<CanvasGroup>();

        //menu items
        SelectBarCanvas = GameObject.Find("SelectBarCanvas").GetComponent<CanvasGroup>();
        InvMenuCanvas = GameObject.Find("InvMenuCanvas").GetComponent<CanvasGroup>();
        SkillMenuCanvas = GameObject.Find("SkillMenuCanvas").GetComponent<CanvasGroup>();
        QuestMenuCanvas = GameObject.Find("QuestMenuCanvas").GetComponent<CanvasGroup>();
        MagicMenuCanvas = GameObject.Find("MagicMenuCanvas").GetComponent<CanvasGroup>();

        //set notification text
        NotificationTxt = NotificationCanvas.transform.Find("Notification").GetComponent<Text>();
    }


    // Update is called once per frame
    void Update()
    {
        //this is being called, why in update?
        //Resources.UnloadUnusedAssets();
        // Careful: For Mac users, ctrl + arrow is a bad idea


        //toggle inventory on and off
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryOn();
        }

        //skill menu
        if (Input.GetKeyDown(KeyCode.K))
        {
            SkillMenuOn();
        }

        //back menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartMenuCanvas.alpha = (StartMenuCanvas.alpha + 1) % 2;
            StartMenuCanvas.interactable = !StartMenuCanvas.interactable;
            StartMenuCanvas.blocksRaycasts = !StartMenuCanvas.blocksRaycasts;
        }

        //stop all action after this if the pointer is over the canvas
        //EventSystem eventSystem = EventSystem.current;
        //if (eventSystem.IsPointerOverGameObject())
        //{
        //    return;
        //}
    }

    void FixedUpdate()
    {
        //if (PlayerStats.PlayerStatsSingle.experiencePoints >= PlayerStats.PlayerStatsSingle.level * 15)
        //{
        //    // text container
        //    StartCoroutine(ShowMessage("LEVEL UP NERD!", 2));

        //    //level character up
        //    PlayerStats.PlayerStatsSingle.level += 1;
        //    PlayerStats.PlayerStatsSingle.maxHealth += 2;
        //    PlayerStats.PlayerStatsSingle.health = PlayerStats.PlayerStatsSingle.maxHealth;
        //}
    }

    //player item distance attract skill learn
    public void LearnAttractPassive()
    {
        if (PlayerStats.PlayerStatsSingle.skillPoints > 0)
        {
            PlayerStats.PlayerStatsSingle.DecSkillPoints();

            PlayerStats.PlayerStatsSingle.itemAttractDistance = 1000;

            GameObject.Find("AttractPassive").GetComponent<Button>().image.color = Color.yellow;
            GameObject.Find("AttractPassive").GetComponent<Button>().interactable = false;
        }
    }

    public void LearnGrapplingHookSkill()
    {
        if (PlayerStats.PlayerStatsSingle.skillPoints > 0)
        {
            PlayerStats.PlayerStatsSingle.DecSkillPoints();

            PlayerController.PlayerControllerSingle.grapplingHookSkill = true;

            GameObject.Find("GrapplingHookSkill").GetComponent<Button>().image.color = Color.yellow;
            GameObject.Find("GrapplingHookSkill").GetComponent<Button>().interactable = false;
            GameObject.Find("HookResetJumps").GetComponent<Button>().interactable = true;
        }
    }

    public void LearnHookResetJumps()
    {
        if (PlayerStats.PlayerStatsSingle.skillPoints > 0)
        {
            PlayerStats.PlayerStatsSingle.DecSkillPoints();

            PlayerController.PlayerControllerSingle.hookResetJumpsSkill = true;

            GameObject.Find("HookResetJumps").GetComponent<Button>().image.color = Color.yellow;
            GameObject.Find("HookResetJumps").GetComponent<Button>().interactable = false;
        }
    }

    public void LearnDoubleJump()
    {
        if (PlayerStats.PlayerStatsSingle.skillPoints > 0)
        {
            PlayerStats.PlayerStatsSingle.DecSkillPoints();

            PlayerController.PlayerControllerSingle.jumpCounter = 2;
            PlayerStats.PlayerStatsSingle.maxJumps = 2;

            GameObject.Find("DoubleJump").GetComponent<Button>().image.color = Color.yellow;
            GameObject.Find("DoubleJump").GetComponent<Button>().interactable = false;
            GameObject.Find("TripleJump").GetComponent<Button>().interactable = true;
        }
    }

    public void LearnTripleJump()
    {
        if (PlayerStats.PlayerStatsSingle.skillPoints > 0)
        {
            PlayerStats.PlayerStatsSingle.DecSkillPoints();

            PlayerController.PlayerControllerSingle.jumpCounter = 3;
            PlayerStats.PlayerStatsSingle.maxJumps = 3;

            GameObject.Find("TripleJump").GetComponent<Button>().image.color = Color.yellow;
            GameObject.Find("TripleJump").GetComponent<Button>().interactable = false;
        }
    }

    public void LearnDashSkill()
    {
        if (PlayerStats.PlayerStatsSingle.skillPoints > 0)
        {
            PlayerStats.PlayerStatsSingle.DecSkillPoints();

            PlayerController.PlayerControllerSingle.dashSkill = true;

            GameObject.Find("DashSkill").GetComponent<Button>().image.color = Color.yellow;
            GameObject.Find("DashSkill").GetComponent<Button>().interactable = false;
            GameObject.Find("DashSkillMelee").GetComponent<Button>().interactable = true;
        }
    }

    public void LearnDashSkillMelee()
    {
        if (PlayerStats.PlayerStatsSingle.skillPoints > 0)
        {
            PlayerStats.PlayerStatsSingle.DecSkillPoints();

            PlayerController.PlayerControllerSingle.dashSkillMelee= true;

            GameObject.Find("DashSkillMelee").GetComponent<Button>().image.color = Color.yellow;
            GameObject.Find("DashSkillMelee").GetComponent<Button>().interactable = false;
        }
    }

    public void LearnSlideSkill()
    {
        if (PlayerStats.PlayerStatsSingle.skillPoints > 0)
        {
            PlayerStats.PlayerStatsSingle.DecSkillPoints();

            PlayerController.PlayerControllerSingle.slideSkill = true;

            GameObject.Find("SlideSkillLearn").GetComponent<Button>().image.color = Color.yellow;
            GameObject.Find("SlideSkillLearn").GetComponent<Button>().interactable = false;
            GameObject.Find("SlideJumpSkill").GetComponent<Button>().interactable = true;
        }
    }

    public void LearnSlideJumpSkill()
    {
        if (PlayerStats.PlayerStatsSingle.skillPoints > 0)
        {
            PlayerStats.PlayerStatsSingle.DecSkillPoints();

            PlayerController.PlayerControllerSingle.slideJumpSkill = true;

            GameObject.Find("SlideJumpSkill").GetComponent<Button>().image.color = Color.yellow;
            GameObject.Find("SlideJumpSkill").GetComponent<Button>().interactable = false;
            GameObject.Find("SlideSteerSkill").GetComponent<Button>().interactable = true;
        }
    }

    public void LearnSlideSteerSkill()
    {
        if (PlayerStats.PlayerStatsSingle.skillPoints > 0)
        {
            PlayerStats.PlayerStatsSingle.DecSkillPoints();

            PlayerController.PlayerControllerSingle.slideSteerSKill = true;

            GameObject.Find("SlideSteerSkill").GetComponent<Button>().image.color = Color.yellow;
            GameObject.Find("SlideSteerSkill").GetComponent<Button>().interactable = false;
        }
    }

    public void InventoryOn()
    {
        //turn off other menu
        SkillMenuCanvas.alpha = 0;
        SkillMenuCanvas.interactable = false;
        SkillMenuCanvas.blocksRaycasts = false;

        QuestMenuCanvas.alpha = 0;
        QuestMenuCanvas.interactable = false;
        QuestMenuCanvas.blocksRaycasts = false;

        MagicMenuCanvas.alpha = 0;
        MagicMenuCanvas.interactable = false;
        MagicMenuCanvas.blocksRaycasts = false;

        //toggle on and off
        InvMenuCanvas.alpha = (InvMenuCanvas.alpha + 1) %2;
        InvMenuCanvas.interactable = !InvMenuCanvas.interactable;
        InvMenuCanvas.blocksRaycasts = !InvMenuCanvas.blocksRaycasts;

        //check if any menu item on
        if (SkillMenuCanvas.alpha == 0 && QuestMenuCanvas.alpha == 0 && MagicMenuCanvas.alpha == 0 && InvMenuCanvas.alpha ==0)
        {
            SelectBarCanvas.alpha = 0;
            SelectBarCanvas.interactable = false;
            SelectBarCanvas.blocksRaycasts = false;
        }
        else
        {
            SelectBarCanvas.alpha = 1;
            SelectBarCanvas.interactable = true;
            SelectBarCanvas.blocksRaycasts = true;
        }
    }

    public void SkillMenuOn()
    {
        //turn off other menu
        InvMenuCanvas.alpha = 0;
        InvMenuCanvas.interactable = false;
        InvMenuCanvas.blocksRaycasts = false;

        QuestMenuCanvas.alpha = 0;
        QuestMenuCanvas.interactable = false;
        QuestMenuCanvas.blocksRaycasts = false;

        MagicMenuCanvas.alpha = 0;
        MagicMenuCanvas.interactable = false;
        MagicMenuCanvas.blocksRaycasts = false;

        //toggle on and off
        SkillMenuCanvas.alpha = (SkillMenuCanvas.alpha + 1) % 2;
        SkillMenuCanvas.interactable = !SkillMenuCanvas.interactable;
        SkillMenuCanvas.blocksRaycasts = !SkillMenuCanvas.blocksRaycasts;

        //check if any menu item on
        if (SkillMenuCanvas.alpha == 0 && QuestMenuCanvas.alpha == 0 && MagicMenuCanvas.alpha == 0 && InvMenuCanvas.alpha == 0)
        {
            SelectBarCanvas.alpha = 0;
            SelectBarCanvas.interactable = false;
            SelectBarCanvas.blocksRaycasts = false;
        }
        else
        {
            SelectBarCanvas.alpha = 1;
            SelectBarCanvas.interactable = true;
            SelectBarCanvas.blocksRaycasts = true;
        }
    }

    public void QuestMenuOn()
    {
        //turn off other menu
        SkillMenuCanvas.alpha = 0;
        SkillMenuCanvas.interactable = false;
        SkillMenuCanvas.blocksRaycasts = false;

        InvMenuCanvas.alpha = 0;
        InvMenuCanvas.interactable = false;
        InvMenuCanvas.blocksRaycasts = false;

        MagicMenuCanvas.alpha = 0;
        MagicMenuCanvas.interactable = false;
        MagicMenuCanvas.blocksRaycasts = false;

        //toggle on and off
        QuestMenuCanvas.alpha = (QuestMenuCanvas.alpha + 1) % 2;
        QuestMenuCanvas.interactable = !QuestMenuCanvas.interactable;
        QuestMenuCanvas.blocksRaycasts = !QuestMenuCanvas.blocksRaycasts;

        //check if any menu item on
        if (SkillMenuCanvas.alpha == 0 && QuestMenuCanvas.alpha == 0 && MagicMenuCanvas.alpha == 0 && InvMenuCanvas.alpha == 0)
        {
            SelectBarCanvas.alpha = 0;
            SelectBarCanvas.interactable = false;
            SelectBarCanvas.blocksRaycasts = false;
        }
        else
        {
            SelectBarCanvas.alpha = 1;
            SelectBarCanvas.interactable = true;
            SelectBarCanvas.blocksRaycasts = true;
        }
    }

    public void MagicMenuOn()
    {
        //turn off other menu
        SkillMenuCanvas.alpha = 0;
        SkillMenuCanvas.interactable = false;
        SkillMenuCanvas.blocksRaycasts = false;

        InvMenuCanvas.alpha = 0;
        InvMenuCanvas.interactable = false;
        InvMenuCanvas.blocksRaycasts = false;

        QuestMenuCanvas.alpha = 0;
        QuestMenuCanvas.interactable = false;
        QuestMenuCanvas.blocksRaycasts = false;

        //toggle on and off
        MagicMenuCanvas.alpha = (MagicMenuCanvas.alpha + 1) % 2;
        MagicMenuCanvas.interactable = !MagicMenuCanvas.interactable;
        MagicMenuCanvas.blocksRaycasts = !MagicMenuCanvas.blocksRaycasts;

        //check if any menu item on
        if (SkillMenuCanvas.alpha == 0 && QuestMenuCanvas.alpha == 0 && MagicMenuCanvas.alpha == 0 && InvMenuCanvas.alpha == 0)
        {
            SelectBarCanvas.alpha = 0;
            SelectBarCanvas.interactable = false;
            SelectBarCanvas.blocksRaycasts = false;
        }
        else
        {
            SelectBarCanvas.alpha = 1;
            SelectBarCanvas.interactable = true;
            SelectBarCanvas.blocksRaycasts = true;
        }
    }

    public void InventoryOnButton()
    {
        if (InvMenuCanvas.alpha == 0)
        {
            InventoryOn();
        }
    }

    public void SkillMenuOnButton()
    {
        if (SkillMenuCanvas.alpha == 0)
        {
            SkillMenuOn();
        }
    }

    public void QuestMenuOnButton()
    {
        if (QuestMenuCanvas.alpha == 0)
        {
            QuestMenuOn();
        }
    }

    public void MagicMenuOnButton()
    {
        if (MagicMenuCanvas.alpha == 0)
        {
            MagicMenuOn();
        }
    }

    //for quick message aboce character head
    public IEnumerator ShowMessage(string message, float delay)
    {
        NotificationCanvas.alpha = (NotificationCanvas.alpha + 1) % 2;
        NotificationTxt.text = message;
        yield return new WaitForSeconds(delay);
        NotificationCanvas.alpha = (NotificationCanvas.alpha + 1) % 2;
    }

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(30, -3, 100, 30), "Health: " + PlayerStats.PlayerStatsSingle.health);
    //    if (GUI.Button(new Rect(Screen.width / 2 + Screen.width / 4, Screen.height / 2, 100, 30), "Save"))
    //    {
    //        SavePlayerData();
    //    }
    //    if (GUI.Button(new Rect(Screen.width / 2 + Screen.width / 4, Screen.height / 2 - 40, 100, 30), "Load"))
    //    {
    //        LoadPlayerData();
    //        //MapGenerator.MapGeneratorSingle.LoadMap();
    //    }
    //}

    //have to put in my hand for load on click field in canvas
    public void SavePlayerData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData playerDat = new PlayerData();

        //Player info
        playerDat.health = PlayerStats.PlayerStatsSingle.health;
        playerDat.maxHealth = PlayerStats.PlayerStatsSingle.maxHealth;
        playerDat.experiencePoints = PlayerStats.PlayerStatsSingle.experiencePoints;
        playerDat.level = PlayerStats.PlayerStatsSingle.level;

        bf.Serialize(file, playerDat);
        file.Close();
    }

    public void SaveMapData()
    {
        Scene scene = SceneManager.GetActiveScene();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + scene.name + ".dat");

        levelInformation mapData = new levelInformation();

        //map info
        mapData.numMaps = MapGenerator.MapGeneratorSingle.numMaps;
        mapData.MapInfo = MapGenerator.MapGeneratorSingle.MapInfo;
        mapData.doorConnectionDictionary = GameData.GameDataSingle.doorConnectionDictionary;
        mapData.mapSeed = GameData.GameDataSingle.mapSeed;

        for (int x = 0; x < GameData.GameDataSingle.mapSets.Count; x++)
        {
            mapData.mapSetsX.Add(GameData.GameDataSingle.mapSets[x].x);
            mapData.mapSetsY.Add(GameData.GameDataSingle.mapSets[x].y);
        }

        bf.Serialize(file, mapData);
        file.Close();
    }

    public void LoadPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            Debug.Log("load called");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData playerDat = (PlayerData)bf.Deserialize(file);
            file.Close();

            //player info
            PlayerStats.PlayerStatsSingle.health = playerDat.health;
            PlayerStats.PlayerStatsSingle.maxHealth = playerDat.maxHealth;
            PlayerStats.PlayerStatsSingle.experiencePoints = playerDat.experiencePoints;
            PlayerStats.PlayerStatsSingle.level = playerDat.level;

            ////map info
            //MapGenerator.MapGeneratorSingle.numMaps = playerDat.numMaps;
            //MapGenerator.MapGeneratorSingle.MapInfo = playerDat.MapInfo;
            //PlayerStats.PlayerStatsSingle.MapInfo = playerDat.MapInfo;
            //GameData.GameDataSingle.doorConnectionDictionary = playerDat.doorConnectionDictionary;
            //GameData.GameDataSingle.mapSeed = playerDat.mapSeed;

            ////load map
            //for (int x = 0; x < playerDat.mapSetsX.Count; x++)
            //{

            //    GameData.GameDataSingle.mapSets.Add(new Vector2(playerDat.mapSetsX[x], playerDat.mapSetsY[x]));
            //}
        }
        else
        {
            Debug.Log("Error with player data load");
        }
    }

    public void LoadMapData()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (File.Exists(Application.persistentDataPath + "/" + scene.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + scene.name + ".dat", FileMode.Open);
            levelInformation mapData = (levelInformation)bf.Deserialize(file);
            file.Close();

            //map info
            MapGenerator.MapGeneratorSingle.numMaps = mapData.numMaps;
            MapGenerator.MapGeneratorSingle.MapInfo = mapData.MapInfo;
            PlayerStats.PlayerStatsSingle.MapInfo = mapData.MapInfo;
            GameData.GameDataSingle.doorConnectionDictionary = mapData.doorConnectionDictionary;
            GameData.GameDataSingle.mapSeed = mapData.mapSeed;

            //load map
            for (int x = 0; x < mapData.mapSetsX.Count; x++)
            {

                GameData.GameDataSingle.mapSets.Add(new Vector2(mapData.mapSetsX[x], mapData.mapSetsY[x]));
            }
        }
        else
        {
            Debug.Log("Error with map data load");
        }
    }

    public void loadScence(string sceneName)
    {
        RemoveEveryingOnMap();
        StartCoroutine(LoadNewScene(sceneName));
    }

    public void RemoveEveryingOnMap()
    {
        foreach (Transform child in WorldObjects.WorldObjectsSingle.transform)
        {
            foreach (Transform child2 in child)
            {
                Destroy(child2.gameObject);
            }
        }
    }

    IEnumerator LoadNewScene(string sceneName)
    {
        PlayerController.PlayerControllerSingle.LockPosition();

        //load functions
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }


        //unlock player
        PlayerController.PlayerControllerSingle.UnLockPosition();

        PlayerController.PlayerControllerSingle.touchingDoor = false;
    }
}

[System.Serializable]
public class PlayerData
{
    //for PlayerStats
    public int health;
    public int maxHealth;
    public int experiencePoints;
    public int level;
}

[System.Serializable]
public class levelInformation
{
    //for MapGenerator
    public int numMaps { get; set; }
    public Dictionary<string, MapInformation> MapInfo;

    //for GameData
    public Dictionary<string, string> doorConnectionDictionary;
    public List<string> mapSeed = new List<string>();
    public List<float> mapSetsX = new List<float>();
    public List<float> mapSetsY = new List<float>();
}

[System.Serializable]
public class MapInformation
{
    public int index { get; set; }
    public int mapSet { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public int randomFillPercent { get; set; }
    public int passageLength { get; set; }
    public int smoothness { get; set; }
    public int squareSize { get; set; }
    public int[,] map { get; set; }
    public int[,] borderedMap { get; set; }

    public List<float> possibleDoorLocationsX;
    public List<float> possibleDoorLocationsY;

    public List<float> doorLocationsX;
    public List<float> doorLocationsY;
    public List<int> doorType;

    public List<float> enemyLocationsX;
    public List<float> enemyLocationsY;

    public List<float> turretLocationsX;
    public List<float> turretLocationsY;

}
