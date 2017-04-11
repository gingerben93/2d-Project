using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlitzCrank : MonoBehaviour {

    //reward
    public Transform reward;

    public bool character;
    public bool chat;

    //Coroutine
    bool Herod = false;
    bool NPCd = false;

    //Canvas text and image
    private Text NPCtext;
    private Text Herotext;
    private CanvasGroup canvas;

    // Use this for initialization
    void Start () {
        character = false;
        chat = false;
        NPCtext = GameObject.Find("StarAreaCanvas/Panel/NPC/NPCText/Text").GetComponent<Text>();
        Herotext = GameObject.Find("StarAreaCanvas/Panel/Hero/HeroText/Text").GetComponent<Text>();
        canvas = GameObject.Find("StarAreaCanvas").GetComponent<CanvasGroup>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Q) && character && GameController.GameControllerSingle.Boss1 == true)
        {
            StartCoroutine(NPCDialog(NPCtext, "You beat the boss, game over faggot.\n"));
            StartCoroutine(HeroDialog(Herotext, "You aint shit as a boss. Easy mode faggot get gud.\n"));
            canvas.alpha = 1;
            chat = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && character && !QuestController.QuestControllerSingle.questList.ContainsKey("Blitz"))
        {
            chat = true;
            StartCoroutine(HeroDialog(Herotext, "I'm bored af my dude. Give me something.\n"));
            StartCoroutine(NPCDialog(NPCtext, "Here is ur quest.\n"));
            canvas.alpha = 1;
            QuestController.QuestControllerSingle.AddQuestToList("Blitz");
            QuestController.QuestControllerSingle.quest = 1;
            QuestController.QuestControllerSingle.PickQuest("Blitz");
        }
        else if (Input.GetKeyDown(KeyCode.Q) && character && QuestController.QuestControllerSingle.questList.ContainsKey("Blitz"))
        {
            if (QuestController.QuestControllerSingle.questList["Blitz"] == false)
            {
                StartCoroutine(NPCDialog(NPCtext, "You already have ur quest.\n"));
                StartCoroutine(HeroDialog(Herotext, "Didn't want to talk to you anyways.\n"));
                canvas.alpha = 1;
                chat = true;
            }
            else
            {
                StartCoroutine(NPCDialog(NPCtext, "You completed your quest, now defeat the boss.\n"));
                StartCoroutine(HeroDialog(Herotext, "kool\n"));
                canvas.alpha = 1;
                chat = true;
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
            canvas.alpha = 0;


            //Text reset and stopping Coroutine
            NPCtext.text = "";
            Herotext.text = "";
            StopAllCoroutines();
            NPCd = false;
            Herod = false;
        }
    }

    IEnumerator HeroDialog(Text textComp, string message)
    {
        
        while (NPCd)
        {
            yield return new WaitForSeconds(0.1f);
        }
        textComp.text = "";

        Herod = true;
        foreach (char letter in message.ToCharArray())
        {
            textComp.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        Herod = false;
    }

    IEnumerator NPCDialog(Text textComp, string message)
    {
       
        while (Herod)
        {
            yield return new WaitForSeconds(0.1f);
        }
        textComp.text = "";

        NPCd = true;
        foreach (char letter in message.ToCharArray())
        {
            textComp.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        NPCd = false;
    }

    //void OnGUI()
    //{
    //    if (chat)
    //    {
    //        GUI.Label(new Rect(Screen.width/2, Screen.height/2 - 50, 1000f, 200f), text);
    //    }
    //}
}
