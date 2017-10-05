using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlitzCrank : MonoBehaviour {

    //reward
    public Transform reward;

    public bool touchingCharacter;

    //Coroutine
    //bool Herod = false;
    //bool NPCd = false;

    //Canvas text and image
    private Text NPCtext;
    private Text Herotext;
    private CanvasGroup canvas;
    public Sprite newSprite;

    //has a quest
    public bool hasQuest { get; set; }

    public static BlitzCrank BlitzCrankSingle;

    void Awake()
    {
        if (BlitzCrankSingle == null)
        {
            BlitzCrankSingle = this;
        }
        else if (BlitzCrankSingle != this)
        {
            Destroy(gameObject);
        }
    }

        // Use this for initialization
    void Start ()
    {
        //put quest to false
        hasQuest = false;

        touchingCharacter = false;
        NPCtext = DialogManager.DialogManagerSingle.NPCtext;
        Herotext = DialogManager.DialogManagerSingle.Herotext;
        canvas = DialogManager.DialogManagerSingle.canvas;

        if (QuestController.QuestControllerSingle.currentMainQuest == 4f)
        {
            Debug.Log("quest is 4");
            hasQuest = true;
            Debug.Log(QuestController.QuestControllerSingle.currentMainQuest + " = QuestController.QuestControllerSingle.currentQuest");
            gameObject.AddComponent<MainQuest4_0>();
        }
        else if (QuestController.QuestControllerSingle.currentMainQuest == 9f)
        {
            Debug.Log("quest is 9");
            hasQuest = true;
            Debug.Log(QuestController.QuestControllerSingle.currentMainQuest + " = QuestController.QuestControllerSingle.currentQuest");
            gameObject.AddComponent<MainQuest9_0>();
        }
        else if (QuestController.QuestControllerSingle.currentMainQuest == 10f)
        {
            Debug.Log("quest is 10");
            hasQuest = true;
            Debug.Log(QuestController.QuestControllerSingle.currentMainQuest + " = QuestController.QuestControllerSingle.currentQuest");
            gameObject.AddComponent<MainQuest10_0>();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (hasQuest)
        {
            //do nothing let quest do stuff
        }
        else if (Input.GetKeyDown(KeyCode.Q) && touchingCharacter && DialogManager.DialogManagerSingle.dialogOn == false)
        {
            DialogManager.DialogManagerSingle.TalkingCharacter.sprite = newSprite;
            KillQuest[] listKillQuests = QuestController.QuestControllerSingle.GetComponentsInChildren<KillQuest>();
            //remove sidequest and text object
            if (listKillQuests.Length > 0)
            {
                foreach (KillQuest quest in listKillQuests)
                {
                    if (quest.questGiver == gameObject.name)
                    {
                        if (quest.killQuestCounter >= quest.killAmount)
                        {
                            Destroy(quest.QuestTxt.gameObject);
                            Destroy(quest.gameObject);

                            canvas.alpha = 1;
                            StartCoroutine(DialogManager.DialogManagerSingle.Dialog(DialogManager.DialogManagerSingle.NPCDialogueLoadPath + "Blitz/QuestComplete"));
                        }
                        else
                        {
                            canvas.alpha = 1;
                            StartCoroutine(DialogManager.DialogManagerSingle.Dialog(DialogManager.DialogManagerSingle.NPCDialogueLoadPath + "Blitz/QuestIncomplete"));
                        }
                    }
                    else
                    {
                        canvas.alpha = 1;

                        //start conversavtion
                        StartCoroutine(DialogManager.DialogManagerSingle.Dialog(DialogManager.DialogManagerSingle.NPCDialogueLoadPath + "Blitz/AcceptQuest"));

                        QuestController.QuestControllerSingle.PickQuest("Blitz", 1);
                        QuestController.QuestControllerSingle.PickQuest("Blitz", 1);
                        QuestController.QuestControllerSingle.PickQuest("Blitz", 1);
                    }
                }
            }
            else
            {
                canvas.alpha = 1;

                //start conversavtion
                StartCoroutine(DialogManager.DialogManagerSingle.Dialog(DialogManager.DialogManagerSingle.NPCDialogueLoadPath + "Blitz/AcceptQuest"));

                QuestController.QuestControllerSingle.PickQuest("Blitz", 1);
                QuestController.QuestControllerSingle.PickQuest("Blitz", 1);
                QuestController.QuestControllerSingle.PickQuest("Blitz", 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            touchingCharacter = true;
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
            touchingCharacter = false;

            //Text reset and stopping Coroutine
            NPCtext.text = "";
            Herotext.text = "";
            StopAllCoroutines();
            //NPCd = false;
            //Herod = false;
        }
    }

    //IEnumerator HeroDialog(Text textComp, string message)
    //{
    //    while (NPCd)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    textComp.text = "";

    //    Herod = true;
    //    foreach (char letter in message.ToCharArray())
    //    {
    //        textComp.text += letter;
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    Herod = false;
    //}

    //IEnumerator NPCDialog(Text textComp, string message)
    //{
    //    while (Herod)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    textComp.text = "";

    //    NPCd = true;
    //    foreach (char letter in message.ToCharArray())
    //    {
    //        textComp.text += letter;
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    NPCd = false;
    //}
}
