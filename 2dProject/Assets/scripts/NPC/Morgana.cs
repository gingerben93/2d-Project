using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Morgana : MonoBehaviour
{

    //reward
    public Transform reward;

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
        NPCtext = DialogManager.DialogManagerSingle.NPCtext;
        Herotext = DialogManager.DialogManagerSingle.Herotext;
        canvas = DialogManager.DialogManagerSingle.canvas;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && character)
        {
            DialogManager.DialogManagerSingle.TalkingCharacter.sprite = newSprite;
            GatherQuest gather = QuestController.QuestControllerSingle.transform.GetComponent<GatherQuest>();
            if (gather)
            {
                if (gather.GatherQuestCounter >= gather.gatheramount)
                {
                    Destroy(gather.QuestTxt.gameObject);
                    Destroy(gather);

                    canvas.alpha = 1;

                    //start conversavtion
                    StartCoroutine(DialogManager.DialogManagerSingle.Dialog(DialogManager.DialogManagerSingle.NPCDialogueLoadPath + "Morgana/QuestComplete"));
                }
                else
                {
                    canvas.alpha = 1;

                    //start conversavtion
                    StartCoroutine(DialogManager.DialogManagerSingle.Dialog(DialogManager.DialogManagerSingle.NPCDialogueLoadPath + "Morgana/QuestIncomplete"));
                }
            }
            else
            {
                canvas.alpha = 1;

                //start conversavtion
                StartCoroutine(DialogManager.DialogManagerSingle.Dialog(DialogManager.DialogManagerSingle.NPCDialogueLoadPath + "Morgana/AcceptQuest"));
                
                //will be random quest later
                QuestController.QuestControllerSingle.PickQuest("Morgana", 2);
            }
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
        }
    }
}
