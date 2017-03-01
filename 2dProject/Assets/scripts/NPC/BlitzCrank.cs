using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlitzCrank : MonoBehaviour {

    //reward
    public Transform reward;

    public bool character;
    public bool chat;

    // Use this for initialization
    void Start () {
        character = false;
        chat = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q) && character && !QuestController.QuestControllerSingle.questList.ContainsKey("Blitz"))
        {
            Debug.Log("Hey");
            chat = true;
            QuestController.QuestControllerSingle.AddQuestToList("Blitz");
            QuestController.QuestControllerSingle.quest = 1;
            QuestController.QuestControllerSingle.PickQuest("Blitz");
        }
        else if (Input.GetKeyDown(KeyCode.Q) && character && QuestController.QuestControllerSingle.questList.ContainsKey("Blitz"))
        {
            if (QuestController.QuestControllerSingle.questList["Blitz"] == false)
            {
                Debug.Log("Already got quest!");
            }
            else
            {
                Debug.Log("QUEST COMPLETE");
                QuestController.QuestControllerSingle.questList.Remove("Blitz");
                Transform savedGameData = Instantiate(reward, transform.position, Quaternion.identity);
                GameData.GameDataSingle.isBossRoomOpen["6"] = true;
                //savedGameData.name = savedGameData.name + GameData.GameDataSingle.isBossRoomOpen;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            character = true;
        }
        //character = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            character = false;
            chat = false;
        }
    }

    void OnGUI()
    {
        if (chat)
        {
            GUI.Label(new Rect(Screen.width/2, Screen.height/2 - 50, 100f, 20f), "Testing");
        }
    }
}
