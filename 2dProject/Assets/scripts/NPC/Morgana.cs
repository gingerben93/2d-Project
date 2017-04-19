using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Morgana : MonoBehaviour
{

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
    public Sprite newSprite;

    // Use this for initialization
    void Start()
    {
        character = false;
        chat = false;
        NPCtext = GameObject.Find("StarAreaCanvas/Panel/NPC/NPCText/Text").GetComponent<Text>();
        Herotext = GameObject.Find("StarAreaCanvas/Panel/Hero/HeroText/Text").GetComponent<Text>();
        //GameObject.Find("StarAreaCanvas/Panel/NPC").GetComponent<Image>().sprite = newSprite;
        canvas = GameObject.Find("StarAreaCanvas").GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q) && character && !QuestController.QuestControllerSingle.questList.ContainsKey("Morgana"))
        {
            GameObject.Find("StarAreaCanvas/Panel/NPC").GetComponent<Image>().sprite = newSprite;
            chat = true;
            StartCoroutine(HeroDialog(Herotext, "Wow Queen. Ur sooo beautiful."));
            StartCoroutine(NPCDialog(NPCtext, "Fucking faggot."));
            canvas.alpha = 1;
            QuestController.QuestControllerSingle.AddQuestToList("Morgana");
            QuestController.QuestControllerSingle.quest = 2;
            QuestController.QuestControllerSingle.PickQuest("Morgana");
        }
        else if (Input.GetKeyDown(KeyCode.Q) && character && QuestController.QuestControllerSingle.questList.ContainsKey("Morgana"))
        {
            if (QuestController.QuestControllerSingle.questList["Morgana"] == false)
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
                GameObject removeQuest = GameObject.Find("ManaPotion");
                Destroy(removeQuest);

                GameObject.Find("StarAreaCanvas/Panel/NPC").GetComponent<Image>().sprite = newSprite;
                StartCoroutine(NPCDialog(NPCtext, "You completed your quest, now defeat the boss."));
                StartCoroutine(HeroDialog(Herotext, "kool"));
                canvas.alpha = 1;
                chat = true;
                QuestController.QuestControllerSingle.questList.Remove("Morgana");
                //Transform savedGameData = Instantiate(reward, transform.position, Quaternion.identity);
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
