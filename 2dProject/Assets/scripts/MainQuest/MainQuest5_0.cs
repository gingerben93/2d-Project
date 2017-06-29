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

                canvas.alpha = 1;
                StartCoroutine(Dialog());
            }
            else
            {
                Debug.Log("not complete side quest yet, talked to side quest character to complete");
            }
        }
    }

    public IEnumerator Dialog()
    {
        //set blitz sprite
        DialogManager.DialogManagerSingle.TalkingCharacter.sprite = BlitzSprite;

        string Conversation1 = DialogManager.DialogManagerSingle.MainQuestDialogueLoadPath + "MainQuest5_0.0";

        //freeze player
        GameController.GameControllerSingle.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        StartCoroutine(DialogManager.DialogManagerSingle.Dialog(Conversation1));

        //waits for conversation to finish
        while ((DialogManager.DialogManagerSingle.dialogOn == true))
        {
            yield return new WaitForSeconds(0.1f);
        }

        //wait 1 sec before continuing
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
            //change main quest text
            QuestController.QuestControllerSingle.MainQuestText.text = "Complete Main Quest " + QuestController.QuestControllerSingle.currentQuest;

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
