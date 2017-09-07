﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainQuest11_0 : MonoBehaviour
{

    public bool touchingCharacter { get; set; }
    private Text NPCtext;
    private Text Herotext;
    private CanvasGroup canvas;

    private bool inRange = false;
    private string DialogPath;

    void Start()
    {
        Debug.Log("TalkOnApproach");
        NPCtext = DialogManager.DialogManagerSingle.NPCtext;
        Herotext = DialogManager.DialogManagerSingle.Herotext;
        canvas = DialogManager.DialogManagerSingle.canvas;

        //set quest text in questlog
        QuestController.QuestControllerSingle.MainQuestText.text = "return to CH1 and discover your magic type." + "Main Quest " + QuestController.QuestControllerSingle.currentQuest;
        //path to dialog
        DialogPath = "MainQuest11_0.0";
    }

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
                StartCoroutine(Dialog());
            }
        }
    }

    public IEnumerator Dialog()
    {
        string Conversation1 = DialogManager.DialogManagerSingle.MainQuestDialogueLoadPath + "MainQuest11_0.0";

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

        QuestController.QuestControllerSingle.currentQuest = 9f;

        if (QuestController.QuestControllerSingle.currentQuest == 9f)
        {
            Debug.Log("quest is 11");
            Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
            //GameObject.Find("Hero").AddComponent<MainQuest12_0>();
        }
        //Destroy(this);
        Destroy(gameObject);
    }
}
