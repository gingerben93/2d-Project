using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour {
    
    //quest text prefab
    public Text QuestTxt;

    //main story quest counter
    public float currentMainQuest;

    public Text MainQuestText; 

    public static QuestController QuestControllerSingle;

    //names of enemies for kill quests
    string[] EnemyNames = new string[] 
    {
        "Enemy",
        "SliderEnemy",
    };

    //names of items to gather // needs to be same name as prefabs object in resource folder
    string[] GatherTargets = new string[]
    {
        "ManaPotion",
        "HealthPotion",
    };

    void Awake()
    {
        if (QuestControllerSingle == null)
        {
            DontDestroyOnLoad(gameObject);
            QuestControllerSingle = this;
        }
        else if (QuestControllerSingle != this)
        {
            Destroy(gameObject);
        }

        //currentQuest = 0f;
    }

    void Start()
    {
        //main quest text box
        GameObject parentQuest = GameObject.Find("QuestPanel");
        MainQuestText = Instantiate(QuestTxt, parentQuest.transform);
        MainQuestText.name = "MainQuest";
        MainQuestText.text = "Complete Quest " + currentMainQuest;

        //check if player is on main quest
        if (currentMainQuest == 2f)
        {
            Debug.Log("quest is 2");
            Debug.Log(currentMainQuest + " = QuestController.QuestControllerSingle.currentQuest");
            GameObject.Find("Hero").AddComponent<MainQuest2_0>();
        }
        else if (QuestController.QuestControllerSingle.currentMainQuest == 5f)
        {
            Debug.Log(QuestController.QuestControllerSingle.currentMainQuest + " = QuestController.QuestControllerSingle.currentQuest");
            GameObject.Find("Hero").AddComponent<MainQuest5_0>();
            Debug.Log("quest is 5");
        }
        else if (currentMainQuest == 6f)
        {
            Debug.Log("quest is 6");
            Debug.Log(currentMainQuest + " = QuestController.QuestControllerSingle.currentQuest");
            GameObject.Find("Hero").AddComponent<MainQuest6_0>();
        }
        else if (currentMainQuest == 10f)
        {
            Debug.Log("quest is 10");
            Debug.Log(currentMainQuest + " = QuestController.QuestControllerSingle.currentQuest");
            GameObject.Find("Hero").AddComponent<MainQuest10_0>();
        }
    }

    public void PickQuest(string NPCName, int questType)
    {
        switch (questType)
        {
            case 1:
                Debug.Log("Quest 1");
                //Instantiate(GatherQuestPrefab, GameController.GameControllerSingle.transform);
                KillQuest(NPCName);
                break;
            case 2:
                Debug.Log("Quest 2");
                //Instantiate(GatherQuestPrefab, GameController.GameControllerSingle.transform);
                GatherQuest(NPCName);
                break;
            case 3:
                Debug.Log("Quest 3");
                break;
            default:
                Debug.Log("No Quest");
                break;
        }
    }

    public void UpdateKillQuest(string EnemyName)
    {
        //Text updateQuest = GameObject.Find("QuestPanel/Enemy").GetComponent<Text>();
        //updateQuest.text = "kill " + killamount + " of " + killtarget + "\n Current kills: " + KillQuestCounter;

        //if (KillQuestCounter == killamount)
        //{
        //    questList[killQuestList[EnemyName]] = true;
        //    killQuestList.Remove(EnemyName);
        //    updateQuest.text = "Quest complete";
        //}
    }

    private void KillQuest(string NPCName)
    {
        //gameObject.AddComponent<KillQuest>();
        //KillQuest KillQuestClass = GetComponent<KillQuest>();

        //would rather do the method above if possible
        GameObject QuestGameobject = new GameObject(NPCName + " Kill Quest");
        QuestGameobject.GetComponent<Transform>().SetParent(transform);
        KillQuest KillQuestClass = QuestGameobject.AddComponent<KillQuest>();

        //Instantiate quest text
        GameObject parentQuest = GameObject.Find("QuestPanel");

        Text QuestTextBox = Instantiate(QuestTxt, parentQuest.transform);
        //Text QuestTextBox = Object.Instantiate<Text>(QuestTxt, parentQuest.transform);

        //gather target random number
        int killTargetNumber = Random.Range(0, EnemyNames.Length);

        //assign quest vaiables
        Debug.Log("NPCName = " + NPCName);
        KillQuestClass.questGiver = NPCName;
        KillQuestClass.killAmount = Random.Range(2, 5);
        KillQuestClass.killTarget = EnemyNames[killTargetNumber];
        //KillQuestClass.killItemPrefab = Resources.Load("Prefabs/Items/" + GatherTargets[killTargetNumber]) as GameObject;
        KillQuestClass.killQuestCounter = 0;
        KillQuestClass.QuestTxt = QuestTextBox;

        //name of text box for quest; happens in gatherQuestStarter()
        QuestTextBox.name = NPCName + " Kill ";

        //start quest
        KillQuestClass.KillQuestStarter();
    }

    private void GatherQuest(string NPCName)
    {
        //this puts them on gamedata; multiple of same type is giving weird error for now; might be solvable
        //gameObject.AddComponent<GatherQuest>();
        //GatherQuest GatherQuestClass = GetComponent<GatherQuest>();

        //would rather do the method above if possible
        GameObject QuestGameobject = new GameObject(NPCName + " Gather Quest");
        QuestGameobject.GetComponent<Transform>().SetParent(transform);
        GatherQuest GatherQuestClass = QuestGameobject.AddComponent<GatherQuest>();

        //Instantiate quest text
        GameObject parentQuest = GameObject.Find("QuestPanel");

        Text QuestTextBox = Instantiate(QuestTxt, parentQuest.transform);
        //Text QuestTextBox = Object.Instantiate<Text>(QuestTxt, parentQuest.transform);

        //gather target random number
        int gatherTargetNumber = Random.Range(0, GatherTargets.Length);

        //assign quest vaiables
        Debug.Log("NPCName = " + NPCName);
        GatherQuestClass.questGiver = NPCName;
        GatherQuestClass.gatherAmount = Random.Range(2, 5);
        GatherQuestClass.gatherTarget = GatherTargets[gatherTargetNumber];
        GatherQuestClass.gatherItemPrefab = Resources.Load("Prefabs/Items/" + GatherTargets[gatherTargetNumber]) as GameObject;
        GatherQuestClass.gatherQuestCounter = 0;
        GatherQuestClass.QuestTxt = QuestTextBox;

        //name of text box for quest; happens in gatherQuestStarter()
        QuestTextBox.name = NPCName + " Gather ";

        //start quest
        GatherQuestClass.GatherQuestStarter();
    }
}
