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

    public string playerName { get; set; }

    public int sideQuestCounter { get; set; }
    public bool sideQuestBool { get; set; }

    public bool facingRight { get; set; }
    public bool jump { get; set; }

    public float attack { get; set; }
    public float moveForce { get; set; }
    public float maxSpeed { get; set; }
    public float jumpForce { get; set; }

    //dash force
    float dashForce = 1000f;

    //rotate with dash
    private bool rotateRight = false;
    private bool rotateleft = false;
    float rotateSpeed = 20f;
    float lastRotationAngle = 0f;

    //touching anything
    public bool IsColliding = true;

    //for if game needs to load test
    public bool dontLoadTheGame;

    //private bool grounded = false;
    private Animator anim;
    public Rigidbody2D rb2d;
    DoorCollider doorInfo;

    //Inventory
    private Canvas InvMenuCanvas;
    //private GameObject StatsMenuCanvas;
    private Canvas StartMenuCanvas;
    //private GameObject QuestMenuCanvas;
    private Canvas NotificationCanvas;
    private Canvas SkillMenuCanvas;
    private Canvas SelectBarCanvas;

    public Text NotificationTxt;

    //is touching door
    public bool touchingDoor { get; set; }
    public bool questTravel { get; set; }
    public string mapSeed { get; set; }
    public int doorRef { get; set; }

    //for respawning
    public Vector3 respawnLocation;

    //for loading
    public bool isGameLoading = false;

    //for stunned
    public bool stun = false;
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

    //double tap click event
    private float ButtonCooler = 0.5f; // Half a second before reset
    private int ButtonCount = 0;
    private float dashCoolDown = 0.0f;

    //skillBoolCheck
    private bool dashSkill = false;
    private bool dashSkill2 = false;

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

        //assign skill functions
        GameObject.Find("DashSkill").GetComponent<Button>().onClick.AddListener(delegate { LearnDashSkill(); });
        GameObject.Find("DashSkill2").GetComponent<Button>().onClick.AddListener(delegate { LearnDashSkill2(); });

        GameObject.Find("InventoryButton").GetComponent<Button>().onClick.AddListener(delegate { InventoryOnButton(); });
        GameObject.Find("SkillsButton").GetComponent<Button>().onClick.AddListener(delegate { SkillMenuOnButton(); });

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

        NotificationCanvas = GameObject.Find("NotificationCanvas").GetComponent <Canvas>(); ;
        //StatsMenuCanvas = GameObject.Find("StatsMenuCanvas");
        StartMenuCanvas = GameObject.Find("StartMenuCanvas").GetComponent<Canvas>();
        InvMenuCanvas = GameObject.Find("InvMenuCanvas").GetComponent<Canvas>();
        //QuestMenuCanvas = GameObject.Find("QuestMenuCanvas");
        SkillMenuCanvas = GameObject.Find("SkillMenuCanvas").GetComponent<Canvas>();
        SelectBarCanvas = GameObject.Find("SelectBarCanvas").GetComponent<Canvas>();

        //for player
        //StatPageExperienceText = GameObject.Find("Experience");

        //other start stuff
        doorInfo = FindObjectOfType<DoorCollider>();
        rb2d = GetComponent<Rigidbody2D>();

        //for where player is at start of scene
        transform.position = respawnLocation;

        //start player name
        playerName = "fukin nerd";
    }


    // Update is called once per frame
    void Update()
    {
        if (isGameLoading)
        {
            return;
        }

        if (PlayerStats.PlayerStatsSingle.health <= 0)
        {
            transform.position = respawnLocation;
            PlayerStats.PlayerStatsSingle.health = PlayerStats.PlayerStatsSingle.maxHealth;
        }

        if (stun)
        {
            return;
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
            StartMenuCanvas.enabled = !StartMenuCanvas.enabled;
        }

        //for rotating
        if (rotateRight)
        {
            //euler angle start at 360 when going right
            transform.Rotate(Vector3.back, rotateSpeed);
            if (lastRotationAngle < transform.eulerAngles.z)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                rotateRight = false;
            }
            lastRotationAngle = transform.eulerAngles.z;
        }

        if (rotateleft)
        {
            //euler angle start at 0 when going left
            transform.Rotate(Vector3.forward, rotateSpeed);
            if (lastRotationAngle > transform.eulerAngles.z)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                rotateleft = false;
            }
            lastRotationAngle = transform.eulerAngles.z;
        }

        //have to learn dash skill to start using it
        if (dashSkill && dashSkill2)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                //Number of Taps you want Minus One
                if (ButtonCooler > 0.0f && ButtonCount == 1 && dashCoolDown <= 0f)
                {
                    Debug.Log("double tap right");
                    dashCoolDown = 1f;
                    if (rb2d.velocity.x >= 6f)
                    {
                        Debug.Log("worked");
                        rb2d.velocity = new Vector2(-7f, rb2d.velocity.y);
                    }
                    else {
                        rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
                    }
                    rb2d.AddForce(new Vector2(dashForce, 0f));

                    //for rotating teemo
                    rotateleft = false;
                    rotateRight = true;
                    lastRotationAngle = 360f;
                }
                else
                {
                    ButtonCooler = 0.2f;
                    ButtonCount += 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                //Number of Taps you want Minus One
                if (ButtonCooler > 0.0f && ButtonCount == 1 && dashCoolDown <= 0f)
                {
                    Debug.Log("double tap left");
                    dashCoolDown = 1f;
                    if (rb2d.velocity.x <= -6f)
                    {
                        Debug.Log("worked");
                        rb2d.velocity = new Vector2(7f, rb2d.velocity.y);
                    }
                    else {
                        rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
                    }
                    rb2d.AddForce(new Vector2(-dashForce, 0f));

                    //for rotating teemo
                    rotateRight = false;
                    rotateleft = true;
                    lastRotationAngle = 0f;
                }
                else
                {
                    ButtonCooler = 0.2f;
                    ButtonCount += 1;
                }
            }

            if (ButtonCooler > 0.0f)
            {
                ButtonCooler -= 1.0f * Time.deltaTime;
            }
            else
            {
                ButtonCount = 0;
            }

            if (dashCoolDown > 0.0f)
            {
                dashCoolDown -= Time.deltaTime;
            }
        }

        //stop all action after this if the pointer is over the canvas
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
            }
        }
    }

    void FixedUpdate()
    {

        if (stun)
        {
            return;
        }

        //anim.SetFloat("Speed", Mathf.Abs(h));

        //for changing exp on page
        //StatPageExperienceText.GetComponent<Text>().text = "EXP: " + PlayerStats.playerStatistics.experiencePoints;

        if (Input.GetKey(KeyCode.D))
        {
            //flip character sprite
            if (!facingRight)
            {
                Flip();
            }
            //if going other direction, stop
            if (rb2d.velocity.x < 0)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
            //speed up by 1 to the right
            if (Mathf.Abs(rb2d.velocity.x) < maxSpeed)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x + 1, rb2d.velocity.y);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //flip character sprite
            if (facingRight)
            {
                Flip();
            }
            //if going other direction, stop
            if (rb2d.velocity.x > 0)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
            //speed up by 1 to the left
            if (Mathf.Abs(rb2d.velocity.x) < maxSpeed)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x - 1, rb2d.velocity.y);
            }
        }
        //if colliding then slow down (don't slow doing in air)
        else if (IsColliding == true)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x * .5f, rb2d.velocity.y);
        }

        if (Mathf.Abs(rb2d.velocity.y) >= maxSpeed)
        {
            //start timer for falling; take damage after 1 sec
            //deltatime is time sinece last frame.
            timer += Time.deltaTime;
            falling = true;
        }
        else
        {
            falling = false;
            timer = 0;
        }

        if (jump)
        {
            rb2d.gravityScale = 1;
            if (rb2d.velocity.y < 0)
            {
                rb2d.velocity = new Vector3(rb2d.velocity.x, 0, 0);
            }
            if (Mathf.Sign(rb2d.velocity.y) < 1.2f)
            {
                rb2d.AddForce(new Vector2(0f, jumpForce));
            }
            if (Mathf.Sign(rb2d.velocity.y) > 1.2f)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Sign(rb2d.velocity.y) * 0.2f);
            }
            jump = false;
        }

        if (PlayerStats.PlayerStatsSingle.experiencePoints >= PlayerStats.PlayerStatsSingle.level * 15)
        {
            // text container
            StartCoroutine(ShowMessage("LEVEL UP NERD!", 2));

            //level character up
            PlayerStats.PlayerStatsSingle.level += 1;
            PlayerStats.PlayerStatsSingle.maxHealth += 2;
            PlayerStats.PlayerStatsSingle.health = PlayerStats.PlayerStatsSingle.maxHealth;
        }
    }

    public void LearnDashSkill()
    {
        dashSkill = true;
        GameObject.Find("DashSkill").GetComponent<Button>().image.color = Color.yellow;
        GameObject.Find("DashSkill").GetComponent<Button>().interactable = false;
        GameObject.Find("DashSkill2").GetComponent<Button>().interactable = true;
    }

    public void LearnDashSkill2()
    {
        GameObject.Find("DashSkill2").GetComponent<Button>().image.color = Color.yellow;
        GameObject.Find("DashSkill2").GetComponent<Button>().interactable = false;
        dashSkill2 = true;
    }

    public void InventoryOn()
    {
        //turn off other menu
        SkillMenuCanvas.enabled = false;

        //toggle on and off
        InvMenuCanvas.enabled = !InvMenuCanvas.enabled;

        //check if any menu item on
        if (!SkillMenuCanvas.enabled && !InvMenuCanvas.enabled)
        {
            SelectBarCanvas.enabled = false;
        }
        else
        {
            SelectBarCanvas.enabled = true;
        }
    }

    public void InventoryOnButton()
    {
        if(InvMenuCanvas.enabled == false)
        {
            InventoryOn();
        }
    }

    public void SkillMenuOn()
    {
        //turn off other menu
        InvMenuCanvas.enabled = false;

        //toggle this menu
        SkillMenuCanvas.enabled = !SkillMenuCanvas.enabled;

        //check if any menu item on; else turn off select bar
        if (!SkillMenuCanvas.enabled && !InvMenuCanvas.enabled)
        {
            SelectBarCanvas.enabled = false;
        }
        else
        {
            SelectBarCanvas.enabled = true;
        }
    }

    public void SkillMenuOnButton()
    {
        if (SkillMenuCanvas.enabled == false)
        {
            SkillMenuOn();
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

        NotificationCanvas.enabled = !NotificationCanvas.enabled;
        NotificationTxt.text = message;
        yield return new WaitForSeconds(delay);
        NotificationCanvas.enabled = !NotificationCanvas.enabled;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(30, -3, 100, 30), "Health: " + PlayerStats.PlayerStatsSingle.health);
        if (GUI.Button(new Rect(Screen.width / 2 + Screen.width / 4, Screen.height / 2, 100, 30), "Save"))
        {
            SavePlayerData();
        }
        if (GUI.Button(new Rect(Screen.width / 2 + Screen.width / 4, Screen.height / 2 - 40, 100, 30), "Load"))
        {
            LoadPlayerData();
            //MapGenerator.MapGeneratorSingle.LoadMap();
        }
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
        IsColliding = true;
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        IsColliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        IsColliding = false;
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
            Debug.Log("load called");
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
}
