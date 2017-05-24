using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public string playerName { get; set; }

    public int sideQuestCounter { get; set; }
    public bool sideQuestBool { get; set; }

    public bool facingRight { get; set; }
    public bool jump { get; set; }

    public float attack { get; set; }
    public float moveForce { get; set; }
    public float maxSpeed{ get; set; }
    public float jumpForce{ get; set; }

    //private bool grounded = false;
    private Animator anim;
    public Rigidbody2D rb2d;
    DoorCollider doorInfo;

    //Inventory
    private CanvasGroup InvMenu;
    private CanvasGroup StatsMenu;
    private CanvasGroup StartMenu;
    private CanvasGroup QuestMenu;
    private CanvasGroup NotificationCanvasGroup;

    public Text NotificationTxt;

    //is touching door
    public bool touchingDoor { get; set; }
    public bool touchingQuestDoor { get; set; }
    public bool questTravel { get; set; }
    public string mapSeed { get; set; }
    public int doorRef { get; set; }

    //for respawning
    public Vector3 respawnLocation;

    //for loading
    public bool isGameLoading = false;

    //For boss deaths
    public bool Boss1 = false;

    //for falling
    private bool falling = false;
    private float timer = 0;

    //for weapons
    private static Slot weap;
    private GameObject slot;
    public Item atk;
    public int damage;
    //public GameObject currentWeapon;

    //for godhands fire rate
    public bool GodhandsCanAttack = true;

    //for removing the current items other things on map
    Transform itemlist;
    GameObject playerProjectileList;

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
    }

    // Use this for initialization
    void Start () {

        //if side quest counter is on
        sideQuestBool = false;

        //variables on start
        maxSpeed = 7f;
        moveForce = 5f;
        jumpForce = 500;
        touchingDoor = false;
        facingRight = true;
        jump = false;
        attack = 2;

        NotificationCanvasGroup = GameObject.Find("Notification").GetComponent<CanvasGroup>();
        StatsMenu = GameObject.Find("StatsMenu").GetComponent<CanvasGroup>();
        StartMenu = GameObject.Find("StartMenu").GetComponent<CanvasGroup>();
        InvMenu = GameObject.Find("Inventory").GetComponent<CanvasGroup>();
        QuestMenu = GameObject.Find("QuestPanel").GetComponent<CanvasGroup>();

        //for player
        //StatPageExperienceText = GameObject.Find("Experience");

        //other start stuff
        doorInfo = FindObjectOfType<DoorCollider>();
        rb2d = GetComponent<Rigidbody2D>();
        transform.position = respawnLocation;

        //start player name
        playerName = "fukin nerd";
    }

	
	// Update is called once per frame
	void Update () {

        if (isGameLoading)
        {
            return;
        }


        if (PlayerStats.PlayerStatsSingle.health <= 0)
        {
            transform.position = respawnLocation;
            PlayerStats.PlayerStatsSingle.health = PlayerStats.PlayerStatsSingle.maxHealth;
        }
        
        Resources.UnloadUnusedAssets();
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Debug.Log("GameData.GameDataSingle.isBossRoomOpen.ContainsKey(mapSeed) = " + GameData.GameDataSingle.isBossRoomOpen.ContainsKey(mapSeed));
            if (touchingDoor)
            {
                MapGenerator.MapGeneratorSingle.seed = mapSeed;
                MapGenerator.MapGeneratorSingle.LoadMap();

                Vector2 door;
                door = MapGenerator.MapGeneratorSingle.doorLocations[doorInfo.numVal];
                transform.position = new Vector3(door.x, door.y, 0);

                respawnLocation = door;

                RemoveCurrentMapObjects();
            }

            if (touchingQuestDoor)
            {
                //Debug.Log("GameData.GameDataSingle.isBossRoomOpen[mapSeed]" + GameData.GameDataSingle.isBossRoomOpen[mapSeed]);
                //if (QuestController.QuestControllerSingle.questDoorOpen[QuestController.QuestControllerSingle.currentQuest])
                //{
                //    Debug.Log("Go to boss room");
                //    touchingQuestDoor = false;
                //    RemoveCurrentMapObjects();
                //    questTravel = true;
                //}
                //else
                //{
                    touchingQuestDoor = false;
                    Debug.Log("Door Is Locked");
                //}
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && rb2d.velocity.y < maxSpeed /*&& grounded*/)
        {
            jump = true;
            rb2d.gravityScale = 1;
        }

        // 5 - Shooting
        //attack = 2; //TESTING PURPOSE
        bool shoot = Input.GetMouseButtonDown(1);
        shoot |= Input.GetMouseButtonDown(1);
        // Careful: For Mac users, ctrl + arrow is a bad idea

        //toggle inventory on and off
        if (Input.GetKeyDown(KeyCode.I))
        {
            InvMenu.alpha = (InvMenu.alpha + 1) % 2;
            InvMenu.interactable = !InvMenu.interactable;
            InvMenu.blocksRaycasts = !InvMenu.blocksRaycasts;

            //stats menu is never interactable
            StatsMenu.alpha = (StatsMenu.alpha + 1) % 2;
            QuestMenu.alpha = (QuestMenu.alpha + 1) % 2;
            //StatsMenu.interactable = !StatsMenu.interactable;
            //StatsMenu.blocksRaycasts = !StatsMenu.blocksRaycasts;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartMenu.alpha = (StartMenu.alpha + 1) % 2;
            StartMenu.interactable = !StartMenu.interactable;
            StartMenu.blocksRaycasts = !StartMenu.blocksRaycasts;
        }

        EventSystem eventSystem = EventSystem.current;
        if (eventSystem.IsPointerOverGameObject())
        {
            return;
        }
        else if (shoot)
        {
            //Find the slot named "Weapon"
            slot = GameObject.Find("WEAPON");

            //Looks at slot information in weapon slot to see if empty or not
            Slot tmp = slot.GetComponent<Slot>();

            if (tmp.IsEmpty)
            {
                //THERE IS NO WEAPON IN WEAPON SLOT DO NUFFIN
            }
            //There is a weapon equipped
            else
            {
                //Obtain stack information of item in weapon
                weap = slot.GetComponent<Slot>();
                //Peek at stack information
                atk = weap.Items.Peek();
                //Access scripts of weapon
                switch (atk.weaponName)
                {
                    case "Blowdart":
                        Blowdart BlowdartWeapon = gameObject.GetComponentInChildren<Blowdart>();
                        BlowdartWeapon.Attack();
                        //Debug.Log(atk.weaponName);
                        break;
                    case "ShortSword":
                        ShortSword ShortSwordWeapon = gameObject.GetComponentInChildren<ShortSword>();
                        ShortSwordWeapon.Attack();
                        //Debug.Log(atk.weaponName);
                        break;
                    case "GodHands":
                        if (GodhandsCanAttack)
                        {
                            GodHands GodHandsWeapon = gameObject.GetComponentInChildren<GodHands>();
                            GodHandsWeapon.Attack(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                            //Debug.Log(atk.weaponName);
                        }
                        break;

                }

               // var weapon = gameObject.GetComponent(atk.weaponName) as MonoBehaviour;
               // Debug.Log(atk.weaponName);
               // Debug.Log(weapon);
                //Invoke attack function (INVOKE CANT USE FUNCTIONS THAT HAVE PARAMETERS)
               // weapon.Invoke("Attack", 0.0001f);
            }
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        //anim.SetFloat("Speed", Mathf.Abs(h));
        
        //for changing exp on page
        //StatPageExperienceText.GetComponent<Text>().text = "EXP: " + PlayerStats.playerStatistics.experiencePoints;

        if (h * rb2d.velocity.x < maxSpeed)
            rb2d.velocity = new Vector2(h * maxSpeed, rb2d.velocity.y);

        if (h > 0 && !facingRight)
            Flip();

        if (h < 0 && facingRight)
            Flip();

        if (jump)
        {
            rb2d.gravityScale = 1;
            if (rb2d.velocity.y < 0)
            {
                rb2d.velocity = new Vector3(0, 0, 0);
            }
            if (Mathf.Sign(rb2d.velocity.y) < 1.2f)
                rb2d.AddForce(new Vector2(0f, jumpForce));
            if (Mathf.Sign(rb2d.velocity.y) > 1.2f)
                rb2d.velocity = new Vector2(0, Mathf.Sign(rb2d.velocity.y) * 0.2f);
            jump = false;
        }

        if (Mathf.Sign(rb2d.velocity.x) > 15f)
        {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * 0.995f, rb2d.velocity.y);
        }

        if (PlayerStats.PlayerStatsSingle.experiencePoints >= PlayerStats.PlayerStatsSingle.level*15)
        {
            // text container
            StartCoroutine(ShowMessage("LEVEL UP NERD!", 2));

            //level character up
            PlayerStats.PlayerStatsSingle.level += 1;
            PlayerStats.PlayerStatsSingle.maxHealth += 2;
            PlayerStats.PlayerStatsSingle.health = PlayerStats.PlayerStatsSingle.maxHealth;
        }
        if(Mathf.Abs(rb2d.velocity.y) >= maxSpeed)
        {
            //start timer for falling; take damage after 1 sec
            //delta is is time sinece last frame.
            timer += Time.deltaTime;
            falling = true;
        }
        else
        {
            falling = false;
            timer = 0;
        }
    }

    private void RemoveCurrentMapObjects()
    {
        //def of parent for removing item
        itemlist = WorldObjects.WorldObjectsSingle.transform.FindChild("WorldItems");
        //itemlist = GameObject.Find("WorldItems");

        //remove player projectiles
        playerProjectileList = GameObject.Find("PlayerProjectiles");

        //remove items
        foreach (Transform child in itemlist)
        {
            Destroy(child.gameObject);
        }

        //remove playerProjectiles
        foreach (Transform child in playerProjectileList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    //for quick message aboce character head
    IEnumerator ShowMessage(string message, float delay)
    {
        NotificationTxt.text = message;
        NotificationCanvasGroup.alpha = (NotificationCanvasGroup.alpha + 1) % 2;
        yield return new WaitForSeconds(delay);
        NotificationCanvasGroup.alpha = (NotificationCanvasGroup.alpha + 1) % 2;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(30, -3, 100, 30), "Health: " + PlayerStats.PlayerStatsSingle.health);
        //if(GUI.Button(new Rect(Screen.width  / 2 + Screen.width / 4, Screen.height / 2, 100, 30), "Save"))
        //{
        //    Save();
        //}
        //if (GUI.Button(new Rect(Screen.width / 2 + Screen.width/ 4, Screen.height / 2 - 40, 100, 30), "Load"))
        //{
        //    Load();
        //}
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Inventory.InventorySingle.AddItem(collision.gameObject.GetComponent<Item>());
            Destroy(collision.gameObject);
        }
        if (falling && timer >= 1f)
        {
            PlayerStats.PlayerStatsSingle.health -= 1;
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData playerDat = new PlayerData();

        playerDat.health = PlayerStats.PlayerStatsSingle.health;
        playerDat.maxHealth = PlayerStats.PlayerStatsSingle.maxHealth;
        playerDat.experiencePoints = PlayerStats.PlayerStatsSingle.experiencePoints;

        playerDat.MapInfo = MapGenerator.MapGeneratorSingle.MapInfo;

        GameData gameData = FindObjectOfType<GameData>();
        playerDat.doorConnectionDictionary = gameData.doorConnectionDictionary;
        playerDat.mapSeed = gameData.mapSeed;
        for (int x = 0; x < gameData.mapSets.Count; x++)
        {
            playerDat.mapSetsX.Add(gameData.mapSets[x].x);
            playerDat.mapSetsY.Add(gameData.mapSets[x].y);
        }

        //Debug.Log("gameData.mapSets.Count = " + gameData.mapSets.Count);
        //Debug.Log("gameData.mapSeed.Count = " + gameData.mapSeed.Count);
        //Debug.Log("gameData.doorConnectionDictionary.Count = " + gameData.doorConnectionDictionary.Count);

        //Debug.Log("playerDat.mapSetsX.Count = " + playerDat.mapSetsX.Count);
        //Debug.Log("playerDat.mapSeed.Count = " + playerDat.mapSeed.Count);
        //Debug.Log("playerDat.doorConnectionDictionary.Count = " + playerDat.doorConnectionDictionary.Count);

        bf.Serialize(file, playerDat);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData playerDat = (PlayerData)bf.Deserialize(file);
            file.Close();

            PlayerStats.PlayerStatsSingle.health = playerDat.health;
            PlayerStats.PlayerStatsSingle.maxHealth = playerDat.maxHealth;
            PlayerStats.PlayerStatsSingle.experiencePoints = playerDat.experiencePoints;
            MapGenerator.MapGeneratorSingle.MapInfo = playerDat.MapInfo;

            MapGenerator.MapGeneratorSingle.LoadMap();
        }
    }

    public void loadScence(string sceneName)
    {
        foreach (Transform child in WorldObjects.WorldObjectsSingle.transform)
        {
            foreach (Transform child2 in child)
            {
                Destroy(child2.gameObject);
            }
        }
        StartCoroutine(LoadNewScene(sceneName));
    }

    IEnumerator LoadNewScene(string sceneName)
    {
        //stops player from moving during loading
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        //deactivates gamecon to stop player from doing anything while game is loading
        GameController.GameControllerSingle.isGameLoading = true;

        //load functions
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }

        //lets player move after loading
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb2d.transform.rotation = Quaternion.identity;

        //actives game controler for player actions
        isGameLoading = false;
        touchingDoor = false;
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

    //for MapGenerator
    public Dictionary<string, MapInformation> MapInfo;

    //for DameData
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
}
