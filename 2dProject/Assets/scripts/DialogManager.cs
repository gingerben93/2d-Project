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

    }
}
