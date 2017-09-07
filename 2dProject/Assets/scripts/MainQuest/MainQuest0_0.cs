using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainQuest0_0 : MonoBehaviour {

    private Text NPCtext;
    private Text Herotext;
    private CanvasGroup canvas;

    //Coroutine
    bool inRange = false;

    //for name
    public bool promptName = false;

    void Start()
    {
        Debug.Log("TalkOnApproach");
        NPCtext = DialogManager.DialogManagerSingle.NPCtext;
        Herotext = DialogManager.DialogManagerSingle.Herotext;
        canvas = DialogManager.DialogManagerSingle.canvas;

        //main quest log text
        QuestController.QuestControllerSingle.MainQuestText.text = "Talk to doctor." + "Main Quest " + QuestController.QuestControllerSingle.currentQuest;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inRange)
        {
            if (Vector3.Distance(GameController.GameControllerSingle.transform.position, transform.position) <= 5f)
            {
                DialogManager.DialogManagerSingle.TalkingCharacter.sprite = transform.GetComponent<SpriteRenderer>().sprite;
                inRange = true;
                Debug.Log("TalkOnApproach is in range");

                canvas.alpha = 1;
                //start quest sequence
                StartCoroutine(Dialog());
            }
        }
    }

    //for getting name
    void OnGUI()
    {
        if (promptName)
        {
            GameController.GameControllerSingle.playerName = GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2, 200, 20), GameController.GameControllerSingle.playerName, 25);
        }
        if (Event.current.isKey && Event.current.keyCode == KeyCode.Return && promptName == true)
        {
            Debug.Log("enter is pressed.");
            promptName = false;
        }
    }

    IEnumerator Dialog()
    {

        //list all conversation that will be had
        string Conversation1 = DialogManager.DialogManagerSingle.MainQuestDialogueLoadPath + "MainQuest0_0.0";
        string Conversation2 = DialogManager.DialogManagerSingle.MainQuestDialogueLoadPath + "MainQuest0_0.1";

        //old freeze player ; differet than old way its stops all movement period; new way just stop game controls and sets velocity to 0
        //GameController.GameControllerSingle.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        //freeze player
        GameController.GameControllerSingle.freezePlayer = true;
        GameController.GameControllerSingle.rb2d.velocity = Vector2.zero;

        //start conversavtion
        StartCoroutine(DialogManager.DialogManagerSingle.Dialog(Conversation1));      

        //waits for conversation to finish
        while((DialogManager.DialogManagerSingle.dialogOn == true))
        {
            yield return new WaitForSeconds(0.1f);
        }

        //pauses and gets name
        promptName = true;
        while (promptName == true)
        { 
            yield return new WaitForSeconds(0.1f);
        }

        //start conversavtion 2
        StartCoroutine(DialogManager.DialogManagerSingle.Dialog(Conversation2));

        //waits for conversation to finish
        while ((DialogManager.DialogManagerSingle.dialogOn == true))
        {
            yield return new WaitForSeconds(0.1f);
        }

        //wait 1 sec before continuing
        yield return new WaitForSeconds(1f);
        canvas.alpha = 0;

        //old unfreeze player ; differet than old way its stops all movement period; new way just stop game controls and sets velocity to 0
        //GameController.GameControllerSingle.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        //GameController.GameControllerSingle.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        //GameController.GameControllerSingle.transform.transform.rotation = Quaternion.identity;

        //freeze player
        GameController.GameControllerSingle.freezePlayer = false;

        //Text reset
        NPCtext.text = "";
        Herotext.text = "";

        //set quest number
        QuestController.QuestControllerSingle.currentQuest = 1f;

        //add next quest component where it needs to go
        if (QuestController.QuestControllerSingle.currentQuest == 1f)
        {
            Debug.Log("quest is 1");
            Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");

            GameObject.Find("Character1").AddComponent<MainQuest1_0>();
        }

        
        //QuestController.QuestControllerSingle.isQuestCurrent = false;
        Destroy(this);
    }
}
