using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour {

    public int quest;
    public Text QuestTxt;

    //Kill Quest
    public int KillQuestCounter = 0;
    int amount;
    string target;

    public Dictionary<string, bool> questList = new Dictionary<string, bool>();

    //kill quest
    public Dictionary<string, string> killQuestList = new Dictionary<string, string>();

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

        Text updateQuest = GameObject.Find("Enemy").GetComponent<Text>();
        updateQuest.text = "kill " + amount + " of " + target + "\n Current kills: " + KillQuestCounter;

        if (KillQuestCounter == amount)
        {
            questList[killQuestList[EnemyName]] = true;
            killQuestList.Remove(EnemyName);
            updateQuest.text = "Quest complete";

        }
    }

    private void KillQuest(string NPCName)
    {
        amount = 2;
        KillQuestCounter = 0;
        target = "Enemy";

        GameObject parentQuest = GameObject.Find("QuestPanel");
        Text tempText = Instantiate(QuestTxt, parentQuest.transform);


        killQuestList.Add(target, NPCName);

        tempText.text = "kill " + amount + " of " + target + "\nCurrent kills: " + KillQuestCounter;
        tempText.name = target;
    }

    //adding npc quest to list
    public void AddQuestToList(string NPCName)
    {
        questList.Add(NPCName, false);
    }
}
