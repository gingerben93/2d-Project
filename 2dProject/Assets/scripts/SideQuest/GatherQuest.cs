using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatherQuest : MonoBehaviour {

    //quest prefab
    public Text QuestTxt;

    //quest giver
    public string questGiver { get; set; }
    public int GatherQuestCounter { get; set; }
    public int gatheramount { get; set; }
    public string gathertarget { get; set; }

    public void UpdateGatherQuest(string gather)
    {
        Text updateQuest2 = GameObject.Find("QuestPanel/ManaPotions").GetComponent<Text>();
        updateQuest2.text = "Gather " + gatheramount + " of " + gather + "\nCurrent Gathered: " + GatherQuestCounter;

        if (GatherQuestCounter >= gatheramount)
        {
            updateQuest2.text = questGiver + " Gather Quest Complete";

            //for if thye main quest needs side quest complete to continue
            if (GameController.GameControllerSingle.sideQuestBool == true)
            {
                GameController.GameControllerSingle.sideQuestCounter += 1;
            }
        }
    }

    public void GatherQuestStarter()
    {
        //QuestTxt = tempText;

        QuestTxt.text = "Gather " + gatheramount + " of " + gathertarget + "\nCurrent Gathered: " + GatherQuestCounter;
        QuestTxt.name = "ManaPotions";
    }
}
