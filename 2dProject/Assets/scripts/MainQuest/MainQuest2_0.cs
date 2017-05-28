using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainQuest2_0 : MonoBehaviour {


    public bool touchingCharacter { get; set; }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (touchingCharacter && Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("talk to character1 for quest3");
            Debug.Log(GameController.GameControllerSingle.sideQuestCounter);
            Debug.Log(GameController.GameControllerSingle.sideQuestBool);
            if (GameController.GameControllerSingle.sideQuestCounter >= 1 && GameController.GameControllerSingle.sideQuestBool == true)
            {
                GameController.GameControllerSingle.sideQuestBool = false;
                Debug.Log("Complete, go to main quest 3");
                QuestController.QuestControllerSingle.currentQuest = 3f;
                CutSceneLoader.CutSceneLoaderSingle.loadScene("CutScene2");
                GameObject.Find("Character1").AddComponent<MainQuest3_0>();
                Destroy(this);

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
