using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour {

    public int quest;
    public Text QuestTxt;

    //General Quest
    public bool QuestTracker = false;

    //Kill Quest
    public bool IsKillQuestActive = false;
    public int KillQuestCounter = 0;
    int amount;
    string target;

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

    public void PickQuest()
    {
        switch (quest)
        {
            case 1:
                Debug.Log("Quest 1");
                KillQuest();
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

    public void UpdateKillQuest()
    {
        QuestTxt.text = "kill " + amount + " of " + target + "\n Current kills: " + KillQuestCounter;
        if (KillQuestCounter == amount)
        {
            IsKillQuestActive = false;
            QuestTxt.text = "Quest complete";
            QuestTracker = true;
        }
    }

    private void KillQuest()
    {
        IsKillQuestActive = true;
        amount = 2;
        KillQuestCounter = 0;
        target = "Enemy";

        QuestTxt.text = "kill " + amount + " of " + target + "\n Current kills: " + KillQuestCounter;
    }
}
