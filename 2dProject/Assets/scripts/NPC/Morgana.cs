using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Morgana : MonoBehaviour
{

    //reward
    public Transform reward;

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

                    StartCoroutine(HeroDialog(Herotext, "I got those potions."));
                    StartCoroutine(NPCDialog(NPCtext, "Thanks now get lost loser."));
                    canvas.alpha = 1;

                }
                else
                {
                    StartCoroutine(HeroDialog(Herotext, "Hi"));
                    StartCoroutine(NPCDialog(NPCtext, "Complete my quest already"));
                    canvas.alpha = 1;
                }
            }
            else
            {
                StartCoroutine(HeroDialog(Herotext, "Wow Queen. Ur sooo beautiful.Wow Queen. "));
                StartCoroutine(NPCDialog(NPCtext, "I know, would you get me some potions?"));
                canvas.alpha = 1;
                //QuestController.QuestControllerSingle.AddQuestToList("Morgana");
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
            NPCd = false;
            Herod = false;
        }
    }

    //nested IEnumerator
    //IEnumerator testNest()
    //{
    //    StartCoroutine(HeroDialog(Herotext, "I got those potions."));
    //    yield return new WaitForSeconds(5.0f);
    //    StartCoroutine(NPCDialog(NPCtext, "Thanks now get lost loser."));
    //}

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
