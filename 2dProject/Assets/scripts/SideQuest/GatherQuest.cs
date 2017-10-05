using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatherQuest : MonoBehaviour {

    //quest text prefab object
    public Text QuestTxt;

    //quest giver name
    public string questGiver { get; set; }

    //Gather quest variables
    public int gatherQuestCounter { get; set; }
    public int gatherAmount { get; set; }
    public string gatherTarget { get; set; }

    //gather quest item prefab
    public GameObject gatherItemPrefab;

    public void UpdateGatherQuest()
    {
        //Text updateQuest2 = GameObject.Find("QuestPanel/GatherQuest").GetComponent<Text>();
        QuestTxt.text = "Gather " + gatherAmount + " of " + gatherTarget + "\nCurrent Gathered: " + gatherQuestCounter;

        if (gatherQuestCounter >= gatherAmount)
        {
            QuestTxt.text = "Gather Quest Complete. Return to " + questGiver;

            //for if thye main quest needs side quest complete to continue
            if (GameController.GameControllerSingle.sideQuestBool == true)
            {
                GameController.GameControllerSingle.sideQuestCounter += 1;
            }
        }
    }

    public void GatherQuestStarter()
    {
        QuestTxt.text = "Gather " + gatherAmount + " of " + gatherTarget + "\nCurrent Gathered: " + gatherQuestCounter;
        //QuestTxt.name = questGiver + " Gather " + gatherTarget;
    }
}
