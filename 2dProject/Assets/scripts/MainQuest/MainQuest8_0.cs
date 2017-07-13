using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainQuest8_0 : MonoBehaviour {

    public bool touchingCharacter { get; set; }
    private Text NPCtext;
    private Text Herotext;
    private CanvasGroup canvas;

    //boss sprite gets set after boss is dead in bossScript
    public Sprite BossSprite;

    //wair for boss to die
    public bool bossDead { get; set; }

    //spawn location outside of room
    private Vector3 spawnLocationHolder;

    // Use this for initialization
    void Start ()
    {
        //set player spawn to in room and hold player spawn location for outside area
        spawnLocationHolder = GameController.GameControllerSingle.respawnLocation;
        GameController.GameControllerSingle.respawnLocation = new Vector3(0, 1, 0);

        NPCtext = DialogManager.DialogManagerSingle.NPCtext;
        Herotext = DialogManager.DialogManagerSingle.Herotext;
        canvas = DialogManager.DialogManagerSingle.canvas;

        bossDead = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(bossDead)
        {
            bossDead = false;
            Debug.Log("boss sprite sprite");
            canvas.alpha = 1;
            StartCoroutine(Dialog());
        }
	}

    public IEnumerator Dialog()
    {
        string Conversation1 = DialogManager.DialogManagerSingle.MainQuestDialogueLoadPath + "MainQuest8_0.0";

        //reset spawn location
        GameController.GameControllerSingle.respawnLocation = spawnLocationHolder;

        //freeze player
        GameController.GameControllerSingle.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        //start conversavtion 1
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
            //change main quest text
            QuestController.QuestControllerSingle.MainQuestText.text = "Complete Main Quest " + QuestController.QuestControllerSingle.currentQuest;

            GameController.GameControllerSingle.sideQuestCounter = 0;
            GameController.GameControllerSingle.sideQuestBool = true;

            Debug.Log("quest is 9");
            Debug.Log(QuestController.QuestControllerSingle.currentQuest + " = QuestController.QuestControllerSingle.currentQuest");
            //GameObject.Find("Hero").AddComponent<MainQuest9_0>();
        }

        CutSceneLoader.CutSceneLoaderSingle.loadScene("CutScene3");

        Destroy(this);
    }
}
