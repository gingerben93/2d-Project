using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    public Text NPCtext { get; set; }
    public Text Herotext { get; set; }
    public CanvasGroup canvas { get; set; }
    public Image TalkingCharacter { get; set; }

    public static DialogManager DialogManagerSingle;

    //is dialog on
    public bool dialogOn = false;

    //skip dialogue
    public bool leftClick = false;
    public bool rightClick = false;

    public string MainQuestDialogueLoadPath { get; private set; }
    public string NPCDialogueLoadPath { get; private set; }

    void Awake()
    {
        if (DialogManagerSingle == null)
        {
            DontDestroyOnLoad(gameObject);
            DialogManagerSingle = this;
        }
        else if (DialogManagerSingle != this)
        {
            Destroy(gameObject);
        }

        NPCtext = GameObject.Find("DialogCanvas/Panel/NPCStuff/NPCText/Text").GetComponent<Text>();
        Herotext = GameObject.Find("DialogCanvas/Panel/HeroStuff/HeroText/Text").GetComponent<Text>();
        canvas = GameObject.Find("DialogCanvas").GetComponent<CanvasGroup>();
        TalkingCharacter = GameObject.Find("DialogCanvas/Panel/NPCStuff/NPC").GetComponent<Image>();
    }

    // Use this for initialization
    void Start ()
    {
        MainQuestDialogueLoadPath = "Dialog/MainQuest/";
        NPCDialogueLoadPath = "Dialog/NPC/";
    }

    void Update()
    {
        if (dialogOn)
        {
            //left mouse click
            if (Input.GetMouseButtonDown(0))
            {
                leftClick = true;
            }

            //right mouse
            else if (Input.GetMouseButtonDown(1))
            {
                if (rightClick)
                {
                    rightClick = false;
                }
                else
                {
                    rightClick = true;
                }
            }
        }
    }

    public IEnumerator Dialog(string Conversation)
    {
        if (dialogOn == false)
        {
            dialogOn = true;
            leftClick = false;
            rightClick = false;

            Debug.Log(QuestController.QuestControllerSingle.currentMainQuest + " = QuestController.QuestControllerSingle.currentQuest");

            TextAsset TextObject = Resources.Load(Conversation) as TextAsset;
            string fullConversation = TextObject.text;
            string[] perline = fullConversation.Split('\n');

            for (int x = 0; x < perline.Length; x += 2)
            {
                Herotext.text = "";
                foreach (char letter in perline[x].ToCharArray())
                {
                    //skip dialogue
                    if (leftClick == true)
                    {
                        Herotext.text = perline[x];
                        leftClick = false;
                        break;
                    }

                    Herotext.text += letter;
                    yield return new WaitForSeconds(0.05f);
                }

                //for pause while talking if left click to continue
                while (rightClick == true)
                {
                    if (leftClick == true)
                    {
                        break;
                    }
                    yield return new WaitForSeconds(0.1f);
                }

                NPCtext.text = "";
                foreach (char letter in perline[x + 1].ToCharArray())
                {
                    //skip dialogue
                    if (leftClick == true)
                    {
                        NPCtext.text = perline[x + 1];
                        leftClick = false;
                        break;
                    }

                    NPCtext.text += letter;
                    yield return new WaitForSeconds(0.05f);
                }

                //for pause while talking
                while (rightClick == true)
                {
                    if (leftClick == true)
                    {
                        break;
                    }
                    yield return new WaitForSeconds(0.1f);
                }
            }

            //stop checking for mouse inputs
            dialogOn = false;
        }
    }
}
