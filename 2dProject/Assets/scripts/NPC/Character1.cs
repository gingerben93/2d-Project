using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character1 : MonoBehaviour
{
    public bool character;

    //Canvas text and image
    private Text NPCtext;
    private Text Herotext;
    private CanvasGroup canvas;
    public Sprite newSprite;

    // Use this for initialization
    void Start()
    {
        character = false;
        //assign text boxes for dialog
        NPCtext = DialogManager.DialogManagerSingle.NPCtext;
        Herotext = DialogManager.DialogManagerSingle.Herotext;
        canvas = DialogManager.DialogManagerSingle.canvas;

        if (QuestController.QuestControllerSingle.currentMainQuest == 1f)
        {
            Debug.Log("quest is 1");
            Debug.Log(QuestController.QuestControllerSingle.currentMainQuest + " = QuestController.QuestControllerSingle.currentQuest");
            gameObject.AddComponent<MainQuest1_0>();
        }
        else if (QuestController.QuestControllerSingle.currentMainQuest == 3f)
        {
            Debug.Log("quest is 3");
            Debug.Log(QuestController.QuestControllerSingle.currentMainQuest + " = QuestController.QuestControllerSingle.currentQuest");
            gameObject.AddComponent<MainQuest3_0>();
        }
        else if (QuestController.QuestControllerSingle.currentMainQuest == 11f)
        {
            Debug.Log("quest is 11");
            Debug.Log(QuestController.QuestControllerSingle.currentMainQuest + " = QuestController.QuestControllerSingle.currentQuest");
            gameObject.AddComponent<MainQuest11_0>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && character && DialogManager.DialogManagerSingle.dialogOn == false)
        {
            //set sprite
            DialogManager.DialogManagerSingle.TalkingCharacter.sprite = newSprite;

            canvas.alpha = 1;

            //start conversation
            StartCoroutine(DialogManager.DialogManagerSingle.Dialog(DialogManager.DialogManagerSingle.NPCDialogueLoadPath + "Character1/AcceptQuest"));

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
            // set dialog manager variables to false and off, must to to retalk to anyone
            DialogManager.DialogManagerSingle.dialogOn = false;
            canvas.alpha = 0;

            //set locals to false
            character = false;

            //Text reset and stopping Coroutine
            NPCtext.text = "";
            Herotext.text = "";
            StopAllCoroutines();
        }
    }
}
