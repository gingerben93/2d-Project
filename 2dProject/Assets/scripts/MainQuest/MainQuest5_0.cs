using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainQuest5_0 : MonoBehaviour {

    private Text NPCtext;
    private Text Herotext;
    private CanvasGroup canvas;
    private Sprite BlitzSprite;

    private bool touchingCharacter = false;

    // Use this for initialization
    void Start ()
    {
        Debug.Log("TalkOnApproach");
        NPCtext = DialogManager.DialogManagerSingle.NPCtext;
        Herotext = DialogManager.DialogManagerSingle.Herotext;
        canvas = DialogManager.DialogManagerSingle.canvas;
    }

    void Update()
    {
        if (touchingCharacter && Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("talk to character1 for quest5");
            Debug.Log(GameController.GameControllerSingle.sideQuestCounter);
            Debug.Log(GameController.GameControllerSingle.sideQuestBool);
            if (GameController.GameControllerSingle.sideQuestCounter >= 1 && GameController.GameControllerSingle.sideQuestBool == true)
            {
                GameController.GameControllerSingle.sideQuestBool = false;
                Debug.Log("Complete, go to main quest 5");
                BlitzCrank.BlitzCrankSingle.hasQuest = false;
                QuestController.QuestControllerSingle.currentQuest = 6f;

                //freeze player
                GameController.GameControllerSingle.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

                canvas.alpha = 1;
                StartCoroutine(Dialog("Con5"));
            }
            else
            {
                Debug.Log("not complete side quest yet, talked to side quest character to complete");
            }
        }
    }

    public IEnumerator Dialog(string Conversation)
    {
        DialogManager.DialogManagerSingle.TalkingCharacter.sprite = BlitzSprite;
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

        //Text reset
        NPCtext.text = "";
        Herotext.text = "";

        QuestController.QuestControllerSingle.currentQuest = 6f;

        if (QuestController.QuestControllerSingle.currentQuest == 6f)
        {
            GameController.GameControllerSingle.sideQuestCounter = 0;
            GameController.GameControllerSingle.sideQuestBool = true;

            Debug.Log("quest is 6");
            Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
            GameObject.Find("Hero").AddComponent<MainQuest6_0>();
        }

        Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Blitz")
        {
            BlitzSprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
            touchingCharacter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Blitz")
        {
            touchingCharacter = false;
        }
    }

}
