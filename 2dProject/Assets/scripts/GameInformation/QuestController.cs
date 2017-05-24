using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour {
    
    public Text QuestTxt;

    //Generic quest variables
    int amount;
    string target;

    //Kill Quest
    public int KillQuestCounter = 0;
    int killamount;
    string killtarget;
    public Dictionary<string, string> killQuestList = new Dictionary<string, string>();

    //Gather Quest
    public int GatherQuestCounter = 0;
    int gatheramount;
    string gathertarget;
    public Dictionary<string, string> gatherQuestList = new Dictionary<string, string>();

    public Dictionary<string, bool> questList = new Dictionary<string, bool>();

    //for opening quest doors
    public bool[] questDoorOpen;
    //public List<bool> QuestDoorOpen = new List<bool>();

    //main story quest counter
    public float currentQuest{ get; set; }

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

        currentQuest = 0f;
        //questDoorOpen = new bool[10];
    }

    public void PickQuest(string NPCName, int questType)
    {
        switch (questType)
        {
            case 1:
                Debug.Log("Quest 1");
                KillQuest(NPCName);
                break;
            case 2:
                Debug.Log("Quest 2");
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

        Text updateQuest = GameObject.Find("QuestPanel/Enemy").GetComponent<Text>();
        updateQuest.text = "kill " + killamount + " of " + killtarget + "\n Current kills: " + KillQuestCounter;

        if (KillQuestCounter == killamount)
        {
            questList[killQuestList[EnemyName]] = true;
            killQuestList.Remove(EnemyName);
            updateQuest.text = "Quest complete";

        }
    }

    private void KillQuest(string NPCName)
    {
        killamount = 2;
        KillQuestCounter = 0;
        killtarget = "Enemy";

        GameObject parentQuest = GameObject.Find("QuestPanel");
        Text tempText = Instantiate(QuestTxt, parentQuest.transform);


        killQuestList.Add(killtarget, NPCName);

        tempText.text = "kill " + killamount + " of " + killtarget + "\nCurrent kills: " + KillQuestCounter;
        tempText.name = killtarget;
    }

    public void UpdateGatherQuest(string gather)
    {

        Text updateQuest2 = GameObject.Find("QuestPanel/ManaPotion").GetComponent<Text>();
        updateQuest2.text = "Gather " + gatheramount + " of " + gathertarget + "\nCurrent gathers: " + GatherQuestCounter;

        if (GatherQuestCounter == gatheramount)
        {
            questList[gatherQuestList[gather]] = true;
            gatherQuestList.Remove(gather);
            updateQuest2.text = "Quest complete";

        }
    }

    private void GatherQuest(string NPCName)
    {
        gatheramount = 2;
        GatherQuestCounter = 0;
        gathertarget = "ManaPotion";

        GameObject parentQuest = GameObject.Find("QuestPanel");
        Text tempText = Instantiate(QuestTxt, parentQuest.transform);


        gatherQuestList.Add(gathertarget, NPCName);

        tempText.text = "Gather " + gatheramount + " of " + gathertarget + "\nCurrent gathers: " + GatherQuestCounter;
        tempText.name = gathertarget;

    }

    //adding npc quest to list
    public void AddQuestToList(string NPCName)
    {
        questList.Add(NPCName, false);
    }

    public void DialogScript(string ConversationName)
    {

    }
}
