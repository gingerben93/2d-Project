using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlitzCrank : MonoBehaviour {

    //reward
    public Transform reward;

    public bool touchingCharacter;

    //Coroutine
    bool Herod = false;
    bool NPCd = false;

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

        if (QuestController.QuestControllerSingle.currentQuest == 4f)
        {
            Debug.Log("quest is 4");
            hasQuest = true;
            Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
            GameObject.Find("Blitz").AddComponent<MainQuest4_0>();
        }
        else if (QuestController.QuestControllerSingle.currentQuest == 5f)
        {
            hasQuest = true;
            Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
            GameObject.Find("Blitz").AddComponent<MainQuest5_0>();
            Debug.Log("quest is 5");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (hasQuest)
        {
            //do nothing let quest do stuff
        }
        else if (Input.GetKeyDown(KeyCode.Q) && touchingCharacter)
        {
            DialogManager.DialogManagerSingle.TalkingCharacter.sprite = newSprite;
            KillQuest kill = QuestController.QuestControllerSingle.transform.GetComponent<KillQuest>();
            //remove sidequest and text object
            if (kill)
            {
                if (kill.KillQuestCounter >= kill.killamount)
                {
                    Destroy(kill.QuestTxt.gameObject);
                    Destroy(kill);

                    StartCoroutine(HeroDialog(Herotext, "Did your quest"));
                    StartCoroutine(NPCDialog(NPCtext, "Thanks, want to do it again?"));
                    canvas.alpha = 1;
                }
                else
                {
                    StartCoroutine(HeroDialog(Herotext, "I killed a few guys"));
                    StartCoroutine(NPCDialog(NPCtext, "Well go kill the rest"));
                    canvas.alpha = 1;
                }
            }
            else
            {
                //TextAsset TextObject = Resources.Load("Dialog/Con1") as TextAsset;
                //string fullConversation = TextObject.text;
                //string[] perline = fullConversation.Split('\n');

                //StartCoroutine(HeroDialog(Herotext, perline[0]));
                //StartCoroutine(NPCDialog(NPCtext, perline[1]));

                StartCoroutine(HeroDialog(Herotext, "I'm bored af my dude. Give me something."));
                StartCoroutine(NPCDialog(NPCtext, "Here is ur quest."));
                canvas.alpha = 1;
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
            touchingCharacter = false;
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
