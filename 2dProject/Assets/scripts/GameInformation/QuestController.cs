using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour {
    
    //quest text prefab
    public Text QuestTxt;

    //main story quest counter
    public float currentQuest;

    //for on start of gatherquest
    //quest giver
    public string questGiver { get; private set; }
    public int gatheramount { get; private set; }
    public string gathertarget { get; private set; }

    public Text MainQuestText; 

    public static QuestController QuestControllerSingle;

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
        GameObject parentQuest = GameObject.Find("QuestPanel");
        MainQuestText = Instantiate(QuestTxt, parentQuest.transform);
        MainQuestText.name = "MainQuest";
        MainQuestText.text = "Complete Quest " + currentQuest;

        if (currentQuest == 2f)
        {
            Debug.Log("quest is 2");
            Debug.Log(currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
            GameObject.Find("Hero").AddComponent<MainQuest2_0>();
        }
        else if (QuestController.QuestControllerSingle.currentQuest == 5f)
        {
            Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
            GameObject.Find("Hero").AddComponent<MainQuest5_0>();
            Debug.Log("quest is 5");
        }
        else if (currentQuest == 6f)
        {
            Debug.Log("quest is 6");
            Debug.Log(currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
            GameObject.Find("Hero").AddComponent<MainQuest6_0>();
        }
        else if (currentQuest == 10f)
        {
            Debug.Log("quest is 10");
            Debug.Log(currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
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
        gameObject.AddComponent<KillQuest>();
        KillQuest temp = GetComponent<KillQuest>();

        //create name of quest
        string questName = "KillQuest";

        //Instantiate quest text
        GameObject parentQuest = GameObject.Find("QuestPanel");
        Text tempText = Instantiate(QuestTxt, parentQuest.transform);
        tempText.name = questName;

        //assign variables
        temp.questGiver = NPCName;
        temp.killamount = 3;
        temp.killtarget = "Enemy";
        temp.KillQuestCounter = 0;
        temp.QuestTxt = tempText;

        //start quest
        temp.KillQuestStarter();
    }

    private void GatherQuest(string NPCName)
    {
        gameObject.AddComponent<GatherQuest>();
        GatherQuest temp = GetComponent<GatherQuest>();

        //create name of quest
        string questName = "ManaPotions";

        //Instantiate quest text
        GameObject parentQuest = GameObject.Find("QuestPanel");
        Text tempText = Instantiate(QuestTxt, parentQuest.transform);
        tempText.name = questName;

        //assign quest vaiables
        temp.questGiver = NPCName;
        temp.gatheramount = 2;
        temp.gathertarget = "ManaPotion";
        temp.GatherQuestCounter = 0;
        temp.QuestTxt = tempText;

        //start quest
        temp.GatherQuestStarter();
    }
}
