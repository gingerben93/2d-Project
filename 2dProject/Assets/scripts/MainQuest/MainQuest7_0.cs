using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainQuest7_0 : MonoBehaviour {

    public bool touchingCharacter { get; set; }
    private Text NPCtext;
    private Text Herotext;
    private CanvasGroup canvas;

    private bool waitforload = true;
    private Sprite BossSprite;

    // Use this for initialization
    void Start ()
    {
        Debug.Log("BossFight");
        NPCtext = DialogManager.DialogManagerSingle.NPCtext;
        Herotext = DialogManager.DialogManagerSingle.Herotext;
        canvas = DialogManager.DialogManagerSingle.canvas;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(GameObject.Find("Boss") && waitforload)
        {
            BossSprite = GameObject.Find("Boss").GetComponent<SpriteRenderer>().sprite;
            DialogManager.DialogManagerSingle.TalkingCharacter.sprite = BossSprite;
            waitforload = false;
            Debug.Log("set boss sprite");
            StartCoroutine(Dialog("MainQuest7_0"));
        }
	}

    public IEnumerator Dialog(string Conversation)
    {
        canvas.alpha = 1;
        Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");

        TextAsset TextObject = Resources.Load("Dialog/" + Conversation) as TextAsset;
        string fullConversation = TextObject.text;
        string[] perline = fullConversation.Split('\n');
        for (int x = 0; x < perline.Length; x += 2)
        {

            Herotext.text = "";
            foreach (char letter in perline[x].ToCharArray())
            {
                Herotext.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            NPCtext.text = "";
            foreach (char letter in perline[x + 1].ToCharArray())
            {
                NPCtext.text += letter;
                yield return new WaitForSeconds(0.05f);
            }
        }
        yield return new WaitForSeconds(1f);
        canvas.alpha = 0;

        //let player move again
        GameController.GameControllerSingle.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GameController.GameControllerSingle.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GameController.GameControllerSingle.transform.transform.rotation = Quaternion.identity;

        //Text reset
        NPCtext.text = "";
        Herotext.text = "";

        QuestController.QuestControllerSingle.currentQuest = 8f;

        if (QuestController.QuestControllerSingle.currentQuest == 8f)
        {
            GameController.GameControllerSingle.sideQuestCounter = 0;
            GameController.GameControllerSingle.sideQuestBool = true;

            Debug.Log("quest is 8");
            Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
            //GameObject.Find("Hero").AddComponent<MainQuest8_0>();
        }
        Destroy(this);
    }
}
