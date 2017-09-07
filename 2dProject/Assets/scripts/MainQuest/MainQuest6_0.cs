using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuest6_0 : MonoBehaviour {

	void Start ()
    {
        //set quest text in questlog
        QuestController.QuestControllerSingle.MainQuestText.text = "Find factory In Cave. " + "Main Quest " + QuestController.QuestControllerSingle.currentQuest;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BossDoor1")
        {
            GameController.GameControllerSingle.questTravel = true;

            QuestController.QuestControllerSingle.currentQuest = 7f;

            if (QuestController.QuestControllerSingle.currentQuest == 7f)
            {
                Debug.Log("quest is 6");
                Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
                GameObject.Find("Hero").AddComponent<MainQuest7_0>();
            }

            Destroy(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BossDoor1")
        {
            GameController.GameControllerSingle.questTravel = false;
        }
    }

}
