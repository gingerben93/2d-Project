using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuest6_0 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BossDoor1")
        {
            GameController.GameControllerSingle.questTravel = true;

            QuestController.QuestControllerSingle.currentQuest = 7f;

            if (QuestController.QuestControllerSingle.currentQuest == 7f)
            {
                //change main quest text
                QuestController.QuestControllerSingle.MainQuestText.text = "Complete Main Quest " + QuestController.QuestControllerSingle.currentQuest;

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
