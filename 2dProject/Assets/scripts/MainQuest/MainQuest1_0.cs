using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainQuest1_0 : MonoBehaviour {

    private Text NPCtext;
    private Text Herotext;
    private CanvasGroup canvas;

    //Coroutine
    bool inRange = false;


    void Start()
    {
        Debug.Log("TalkOnApproach");
        NPCtext = GameObject.Find("StarAreaCanvas/Panel/NPC/NPCText/Text").GetComponent<Text>();
        Herotext = GameObject.Find("StarAreaCanvas/Panel/Hero/HeroText/Text").GetComponent<Text>();
        canvas = GameObject.Find("StarAreaCanvas").GetComponent<CanvasGroup>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!inRange)
        {
            if (Vector3.Distance(GameController.GameControllerSingle.transform.position, transform.position) <= 5f)
            {
                GameObject.Find("StarAreaCanvas/Panel/NPC").GetComponent<Image>().sprite = transform.GetComponent<SpriteRenderer>().sprite;
                inRange = true;
                Debug.Log("TalkOnApproach is in range");

                //freeze player
                GameController.GameControllerSingle.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

                canvas.alpha = 1;
                StartCoroutine(Dialog("Con2"));
            }
        }

        
    }

    IEnumerator Dialog(string Conversation)
    {
        Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");

        TextAsset TextObject = Resources.Load("Dialog/" + Conversation) as TextAsset;
        string fullConversation = TextObject.text;
        string[] perline = fullConversation.Split('\n');
        for (int x = 0; x < perline.Length; x += 2)
        {

            Herotext.text = "";
            foreach (char letter in perline[x].ToCharArray())
            {
                Herotext.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            NPCtext.text = "";
            foreach (char letter in perline[x + 1].ToCharArray())
            {
                NPCtext.text += letter;
                yield return new WaitForSeconds(0.05f);
            }
        }
        yield return new WaitForSeconds(1f);
        canvas.alpha = 0;

        //let player move again
        GameController.GameControllerSingle.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GameController.GameControllerSingle.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GameController.GameControllerSingle.transform.transform.rotation = Quaternion.identity;

        //set quest not current

        //Text reset
        NPCtext.text = "";
        Herotext.text = "";

        QuestController.QuestControllerSingle.currentQuest = 2f;

        if (QuestController.QuestControllerSingle.currentQuest == 2f)
        {
            //QuestController.QuestControllerSingle.isQuestCurrent = true;
            Debug.Log("quest is 2");
            Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
            //QuestController.QuestControllerSingle.NextMainQuest(QuestController.QuestControllerSingle.currentQuest);
            GameObject.Find("Hero").AddComponent<MainQuest2_0>();
        }

        GameController.GameControllerSingle.sideQuestCounter = 0;
        GameController.GameControllerSingle.sideQuestBool = true;

        Destroy(this);
    }
}