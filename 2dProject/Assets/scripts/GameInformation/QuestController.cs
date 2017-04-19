using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour {

    public int quest;
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

    public static QuestController QuestControllerSingle;
    /*
     * NPC chooses a rand quest
     * Kill/Collection
     * Kill/Collection quests - rand between objective names
     * Rand amount on how many to kill/collect (5-10)
     * Amount of kill/collect will affect reward (reward x count)
     * 
     * 
     * 
     * 
     * 
     * */
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
    }

    public void PickQuest(string NPCName)
    {
        switch (quest)
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
}
