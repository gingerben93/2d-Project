using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character1 : MonoBehaviour
{
    public bool character;
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
    void Start()
    {
        character = false;
        chat = false;
        //assign text boxes for dialog
        NPCtext = GameObject.Find("StarAreaCanvas/Panel/NPC/NPCText/Text").GetComponent<Text>();
        Herotext = GameObject.Find("StarAreaCanvas/Panel/Hero/HeroText/Text").GetComponent<Text>();
        canvas = GameObject.Find("StarAreaCanvas").GetComponent<CanvasGroup>();

        if (QuestController.QuestControllerSingle.currentQuest == 1f)
        {
            //QuestController.QuestControllerSingle.isQuestCurrent = true;
            Debug.Log("quest is 1");
            Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
            //QuestController.QuestControllerSingle.NextMainQuest(QuestController.QuestControllerSingle.currentQuest);
            GameObject.Find("Character1").AddComponent<MainQuest1_0>();
        }
        else if (QuestController.QuestControllerSingle.currentQuest == 3f)
        {
            //QuestController.QuestControllerSingle.isQuestCurrent = true;
            Debug.Log("quest is 3");
            Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
            //QuestController.QuestControllerSingle.NextMainQuest(QuestController.QuestControllerSingle.currentQuest);
            GameObject.Find("Character1").AddComponent<MainQuest3_0>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q) && character)
        {
            GameObject.Find("StarAreaCanvas/Panel/NPC").GetComponent<Image>().sprite = newSprite;
            chat = true;
            StartCoroutine(HeroDialog(Herotext, "Who are you?"));
            StartCoroutine(NPCDialog(NPCtext, "I'm the Magic Mike Man"));
            canvas.alpha = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            character = true;
        }
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
