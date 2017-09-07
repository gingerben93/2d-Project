using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainQuest2_0 : MonoBehaviour {

    public bool touchingCharacter { get; set; }

    void Start()
    {
        GameController.GameControllerSingle.sideQuestCounter = 0;
        GameController.GameControllerSingle.sideQuestBool = true;

        //set quest text in questlog
        QuestController.QuestControllerSingle.MainQuestText.text = "Do Side Quest. " + "Main Quest " + QuestController.QuestControllerSingle.currentQuest;
    }

    // Update is called once per frame
    void Update ()
    {


        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("talk to character1 for quest3");
            Debug.Log(GameController.GameControllerSingle.sideQuestCounter);
            Debug.Log(GameController.GameControllerSingle.sideQuestBool);
            if (touchingCharacter && GameController.GameControllerSingle.sideQuestCounter >= 1 && GameController.GameControllerSingle.sideQuestBool == true)
            {
                GameController.GameControllerSingle.sideQuestBool = false;

                //set quest text in questlog
                QuestController.QuestControllerSingle.MainQuestText.text = "Talk to CH1. " + "Main Quest " + QuestController.QuestControllerSingle.currentQuest;

                Debug.Log("Complete, go to main quest 3");
                QuestController.QuestControllerSingle.currentQuest = 3f;
                GameController.GameControllerSingle.transform.position = transform.position + new Vector3(-6f, 0, 0);
                CutSceneLoader.CutSceneLoaderSingle.loadScene("CutScene2");
                GameObject.Find("Character1").AddComponent<MainQuest3_0>();
                Destroy(this);
            }
            else if (GameController.GameControllerSingle.sideQuestCounter >= 1 && GameController.GameControllerSingle.sideQuestBool == true)
            {
                //set quest text in questlog
                QuestController.QuestControllerSingle.MainQuestText.text = "Talk to CH1. " + "Main Quest " + QuestController.QuestControllerSingle.currentQuest;

            }
            else
            {
                Debug.Log("not complete side quest yet, talked to side quest character to complete");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Character1")
        {
            touchingCharacter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Character1")
        {
            touchingCharacter = false;
        }
    }
}
