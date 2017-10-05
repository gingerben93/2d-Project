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
        spawnLocationHolder = PlayerController.PlayerControllerSingle.respawnLocation;
        PlayerController.PlayerControllerSingle.respawnLocation = new Vector3(0, 1, 0);

        NPCtext = DialogManager.DialogManagerSingle.NPCtext;
        Herotext = DialogManager.DialogManagerSingle.Herotext;
        canvas = DialogManager.DialogManagerSingle.canvas;

        bossDead = false;

        //set quest text in questlog
        QuestController.QuestControllerSingle.MainQuestText.text = "Kill Boss. " + "Main Quest " + QuestController.QuestControllerSingle.currentMainQuest;
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
        PlayerController.PlayerControllerSingle.respawnLocation = spawnLocationHolder;

        //freeze player
        PlayerController.PlayerControllerSingle.LockPosition();

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
        PlayerController.PlayerControllerSingle.UnLockPosition();

        //Text reset
        NPCtext.text = "";
        Herotext.text = "";

        QuestController.QuestControllerSingle.currentMainQuest = 9f;

        if (QuestController.QuestControllerSingle.currentMainQuest == 9f)
        {
            Debug.Log("quest is 9");
            Debug.Log(QuestController.QuestControllerSingle.currentMainQuest + " = QuestController.QuestControllerSingle.currentQuest");
            //GameObject.Find("Hero").AddComponent<MainQuest9_0>();
        }

        CutSceneLoader.CutSceneLoaderSingle.loadScene("CutScene3");
        Destroy(this);
    }
}
