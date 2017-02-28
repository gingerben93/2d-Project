using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlitzCrank : MonoBehaviour {


    public bool character;
    public bool chat;
    // Use this for initialization
    void Start () {
        character = false;
        chat = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q) && character)
        {
            Debug.Log("Hey");
            chat = true;
            QuestController.QuestControllerSingle.quest = 1;
            QuestController.QuestControllerSingle.PickQuest();
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
