using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainQuest3_0 : MonoBehaviour {

    private Text NPCtext;
    private Text Herotext;
    private CanvasGroup canvas;

    //Coroutine
    bool inRange = false;

    void Start()
    {
        Debug.Log("TalkOnApproach");
        NPCtext = DialogManager.DialogManagerSingle.NPCtext;
        Herotext = DialogManager.DialogManagerSingle.Herotext;
        canvas = DialogManager.DialogManagerSingle.canvas;

        //set quest text in questlog
        QuestController.QuestControllerSingle.MainQuestText.text = "Talk to CH1. " + "Main Quest " + QuestController.QuestControllerSingle.currentMainQuest;
    }

    // Update is called once per frame
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

    IEnumerator Dialog()
    {
        string Conversation1 = DialogManager.DialogManagerSingle.MainQuestDialogueLoadPath + "MainQuest3_0.0";

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

        QuestController.QuestControllerSingle.currentMainQuest = 4f;

        if (QuestController.QuestControllerSingle.currentMainQuest == 4f)
        {
            Debug.Log("quest is 4");
            Debug.Log(QuestController.QuestControllerSingle.currentMainQuest + " = QuestController.QuestControllerSingle.currentQuest");
            GameObject.Find("Blitz").AddComponent<MainQuest4_0>();
            BlitzCrank.BlitzCrankSingle.hasQuest = true;
        }

        Destroy(this);
    }
}
