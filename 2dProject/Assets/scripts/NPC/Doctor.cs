﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Doctor : MonoBehaviour
{
    public bool character;

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
        //assign text boxes for dialog
        NPCtext = DialogManager.DialogManagerSingle.NPCtext;
        Herotext = DialogManager.DialogManagerSingle.Herotext;
        canvas = DialogManager.DialogManagerSingle.canvas;

        if (QuestController.QuestControllerSingle.currentQuest == 0f)
        {
            //QuestController.QuestControllerSingle.isQuestCurrent = true;
            Debug.Log("quest is 0");
            Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
            //QuestController.QuestControllerSingle.NextMainQuest(QuestController.QuestControllerSingle.currentQuest);
            GameObject.Find("Doctor").AddComponent<MainQuest0_0>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q) && character)
        {
            DialogManager.DialogManagerSingle.TalkingCharacter.sprite = newSprite;
            StartCoroutine(HeroDialog(Herotext, "Who are you?"));
            StartCoroutine(NPCDialog(NPCtext, "I'm the doctor"));
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
}