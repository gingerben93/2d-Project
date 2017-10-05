using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainQuest4_0 : MonoBehaviour {

    public bool touchingCharacter { get; set; }
    private Text NPCtext;
    private Text Herotext;
    private CanvasGroup canvas;

    private bool inRange = false;

    void Start()
    {
        Debug.Log("TalkOnApproach");
        NPCtext = DialogManager.DialogManagerSingle.NPCtext;
        Herotext = DialogManager.DialogManagerSingle.Herotext;
        canvas = DialogManager.DialogManagerSingle.canvas;

        //set quest text in questlog
        QuestController.QuestControllerSingle.MainQuestText.text = "Talk to blitz. " + "Main Quest " + QuestController.QuestControllerSingle.currentMainQuest;
    }

    void Update()
    {
        if (!inRange)
        {
            if (Vector3.Distance(PlayerController.PlayerControllerSingle.transform.position, transform.position) <= 5f)
            {
                DialogManager.DialogManagerSingle.TalkingCharacter.sprite = transform.GetComponent<SpriteRenderer>().sprite;
                inRange = true;
                Debug.Log("TalkOnApproach is in range");

                canvas.alpha = 1;
                StartCoroutine(Dialog());
            }
        }
    }

    public IEnumerator Dialog()
    {
        string Conversation1 = DialogManager.DialogManagerSingle.MainQuestDialogueLoadPath + "MainQuest4_0.0";

        //freeze player
        PlayerController.PlayerControllerSingle.LockPosition();

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
        PlayerController.PlayerControllerSingle.UnLockPosition();

        //Text reset
        NPCtext.text = "";
        Herotext.text = "";

        QuestController.QuestControllerSingle.currentMainQuest = 5f;

        if (QuestController.QuestControllerSingle.currentMainQuest == 5f)
        {
            //moved to quest 5
            //GameController.GameControllerSingle.sideQuestCounter = 0;
            //GameController.GameControllerSingle.sideQuestBool = true;

            Debug.Log("quest is 5");
            Debug.Log(QuestController.QuestControllerSingle.currentMainQuest + " = QuestController.QuestControllerSingle.currentQuest");
            GameObject.Find("Hero").AddComponent<MainQuest5_0>();
        }
        BlitzCrank.BlitzCrankSingle.hasQuest = false;
        Destroy(this);
    }
}
