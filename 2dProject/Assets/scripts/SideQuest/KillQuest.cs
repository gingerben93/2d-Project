using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillQuest : MonoBehaviour {

    //quest prefab
    public Text QuestTxt;

    //quest giver
    public string questGiver { get; set; }

    //Kill Quest
    public int KillQuestCounter { get; set; }
    public int killamount { get; set; }
    public string killtarget { get; set; }

    public void UpdateKillQuest(string target)
    {
        Text updateQuest2 = GameObject.Find("QuestPanel/KillQuest").GetComponent<Text>();
        updateQuest2.text = "Kill " + killamount + " of " + target + "\nCurrent Kills: " + KillQuestCounter;

        if (KillQuestCounter >= killamount)
        {
            updateQuest2.text = questGiver + " Kill Quest Complete";

            //for if thye main quest needs side quest complete to continue
            if (GameController.GameControllerSingle.sideQuestBool == true)
            {
                GameController.GameControllerSingle.sideQuestCounter += 1;
            }
        }
    }

    public void KillQuestStarter()
    {
        QuestTxt.text = "kill " + killamount + " of " + killtarget + "\nCurrent kills: " + KillQuestCounter;
        QuestTxt.name = "KillQuest";
    }
}
