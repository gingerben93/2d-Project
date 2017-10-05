using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillQuest : MonoBehaviour {

    //quest text prefab object
    public Text QuestTxt;

    //quest giver name
    public string questGiver { get; set; }

    //Kill Quest variables
    public int killQuestCounter { get; set; }
    public int killAmount { get; set; }
    public string killTarget { get; set; }

    public void UpdateKillQuest()
    {
        //Text QuestTxt = GameObject.Find("QuestPanel/KillQuest").GetComponent<Text>();
        QuestTxt.text = "Kill " + killAmount + " of " + killTarget + "\nCurrent Kills: " + killQuestCounter;

        if (killQuestCounter >= killAmount)
        {
            QuestTxt.text = "Kill Quest Complete. Return to " + questGiver;

            //for if thye main quest needs side quest complete to continue
            if (GameController.GameControllerSingle.sideQuestBool == true)
            {
                GameController.GameControllerSingle.sideQuestCounter += 1;
            }
        }
    }

    public void KillQuestStarter()
    {
        QuestTxt.text = "kill " + killAmount + " of " + killTarget + "\nCurrent kills: " + killQuestCounter;
        //QuestTxt.name = questGiver + " Gather " + gatherTarget;
    }
}
