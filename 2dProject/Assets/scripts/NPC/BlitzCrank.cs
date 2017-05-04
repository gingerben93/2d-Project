using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlitzCrank : MonoBehaviour {

    //reward
    public Transform reward;

    public bool touchingCharacter;
    public bool chat;

    //Coroutine
    bool Herod = false;
    bool NPCd = false;

    //Canvas text and image
    private Text NPCtext;
    private Text Herotext;
    private CanvasGroup canvas;
    public Sprite newSprite;

    // Use this for initialization
    void Start () {
        touchingCharacter = false;
        chat = false;
        NPCtext = GameObject.Find("StarAreaCanvas/Panel/NPC/NPCText/Text").GetComponent<Text>();
        Herotext = GameObject.Find("StarAreaCanvas/Panel/Hero/HeroText/Text").GetComponent<Text>();
        //GameObject.Find("StarAreaCanvas/Panel/NPC").GetComponent<Image>().sprite = newSprite;
        canvas = GameObject.Find("StarAreaCanvas").GetComponent<CanvasGroup>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Q) && touchingCharacter && GameController.GameControllerSingle.Boss1 == true)
        {
            GameObject.Find("StarAreaCanvas/Panel/NPC").GetComponent<Image>().sprite = newSprite;
            StartCoroutine(NPCDialog(NPCtext, "You beat the boss, game over faggot."));
            StartCoroutine(HeroDialog(Herotext, "You aint shit as a boss. Easy mode faggot get gud."));
            canvas.alpha = 1;
            chat = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && touchingCharacter && !QuestController.QuestControllerSingle.questList.ContainsKey("Blitz"))
        {
            GameObject.Find("StarAreaCanvas/Panel/NPC").GetComponent<Image>().sprite = newSprite;
            chat = true;
            StartCoroutine(HeroDialog(Herotext, "I'm bored af my dude. Give me something."));
            StartCoroutine(NPCDialog(NPCtext, "Here is ur quest."));
            canvas.alpha = 1;
            QuestController.QuestControllerSingle.AddQuestToList("Blitz");
            QuestController.QuestControllerSingle.quest = 1;
            QuestController.QuestControllerSingle.PickQuest("Blitz");
        }
        else if (Input.GetKeyDown(KeyCode.Q) && touchingCharacter && QuestController.QuestControllerSingle.questList.ContainsKey("Blitz"))
        {
            if (QuestController.QuestControllerSingle.questList["Blitz"] == false)
            {
                GameObject.Find("StarAreaCanvas/Panel/NPC").GetComponent<Image>().sprite = newSprite;
                StartCoroutine(NPCDialog(NPCtext, "You already have ur quest."));
                StartCoroutine(HeroDialog(Herotext, "Didn't want to talk to you anyways."));
                canvas.alpha = 1;
                chat = true;
            }
            else
            {
                //for removing quest
                GameObject removeQuest = GameObject.Find("Enemy");
                Destroy(removeQuest);

                GameObject.Find("StarAreaCanvas/Panel/NPC").GetComponent<Image>().sprite = newSprite;
                StartCoroutine(NPCDialog(NPCtext, "You completed your quest, now defeat the boss."));
                StartCoroutine(HeroDialog(Herotext, "kool"));
                canvas.alpha = 1;
                chat = true;
                QuestController.QuestControllerSingle.questList.Remove("Blitz");
                Instantiate(reward, transform.position, Quaternion.identity);
                //Transform savedGameData = Instantiate(reward, transform.position, Quaternion.identity);

                QuestController.QuestControllerSingle.questDoorOpen[QuestController.QuestControllerSingle.currentQuest] = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            touchingCharacter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            touchingCharacter = false;
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
