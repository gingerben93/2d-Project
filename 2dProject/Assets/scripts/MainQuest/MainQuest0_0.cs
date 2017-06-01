using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainQuest0_0 : MonoBehaviour {

    private Text NPCtext;
    private Text Herotext;
    private CanvasGroup canvas;

    //skip dialogue
    bool leftClick = false;
    bool rightClick = false;

    //Coroutine
    bool inRange = false;


    void Start()
    {
        Debug.Log("TalkOnApproach");
        NPCtext = DialogManager.DialogManagerSingle.NPCtext;
        Herotext = DialogManager.DialogManagerSingle.Herotext;
        canvas = DialogManager.DialogManagerSingle.canvas;

    }

    // Update is called once per frame
    void Update ()
    {
        if (!inRange)
        {
            if (Vector3.Distance(GameController.GameControllerSingle.transform.position, transform.position) <= 5f)
            {
                DialogManager.DialogManagerSingle.TalkingCharacter.sprite = transform.GetComponent<SpriteRenderer>().sprite;
                inRange = true;
                Debug.Log("TalkOnApproach is in range");

                //freeze player
                GameController.GameControllerSingle.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

                canvas.alpha = 1;
                StartCoroutine(Dialog("Con1"));
            }
        }

        //left mouse
        else if (Input.GetMouseButtonDown(0))
        {
            leftClick = true;
        }

        //right mouse
        else if (Input.GetMouseButtonDown(1))
        {
            if(rightClick)
            {
                rightClick = false;
            }
            else
            {
                rightClick = true;
            }
        }
    }

    public bool promptName = false;
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

    IEnumerator Dialog(string Conversation)
    {
        leftClick = false;
        rightClick = false;

        Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");

        TextAsset TextObject = Resources.Load("Dialog/" + Conversation) as TextAsset;
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
                if(leftClick == true)
                {
                    break;
                }
                yield return new WaitForSeconds(0.1f);
            }

            //stop to promt player for name
            if (x == 2)
            {
                promptName = true;
                while (promptName == true)
                { 
                    yield return new WaitForSeconds(0.1f);
                }
            }

            NPCtext.text = "";
            foreach (char letter in perline[x+1].ToCharArray())
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

        yield return new WaitForSeconds(1f);
        canvas.alpha = 0;

        //let player move again
        GameController.GameControllerSingle.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GameController.GameControllerSingle.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GameController.GameControllerSingle.transform.transform.rotation = Quaternion.identity;

        //set quest not current

        //Text reset
        NPCtext.text = "";
        Herotext.text = "";

        QuestController.QuestControllerSingle.currentQuest = 1f;

        if (QuestController.QuestControllerSingle.currentQuest == 1f)
        {
            //QuestController.QuestControllerSingle.isQuestCurrent = true;
            Debug.Log("quest is 1");
            Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
            //QuestController.QuestControllerSingle.NextMainQuest(QuestController.QuestControllerSingle.currentQuest);
            GameObject.Find("Character1").AddComponent<MainQuest1_0>();
        }
        //update main quest
        
        //QuestController.QuestControllerSingle.isQuestCurrent = false;
        Destroy(this);
    }
}
