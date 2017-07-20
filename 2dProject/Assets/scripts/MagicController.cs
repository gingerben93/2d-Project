using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicController : MonoBehaviour {

    public GameObject MagicImagePrefab;
    public Sprite DefaultImage;
    private GameObject FromMagic;
    private GameObject ToMagic;

    private Canvas canvas;

    // Use this for initialization
    void Start () {

        canvas = GetComponent<Canvas>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (FromMagic != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            position.Set(position.x, position.y);
            FromMagic.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    public void ClickMagic(GameObject clicked)
    {
        //if nothing is floating with the cursor and you clicked on empty slot
        if (FromMagic == null && clicked.GetComponent<Image>().sprite.name.Contains("Inv Slot"))
        {
            Debug.Log("if");
        }
        //for clicking on somthing
        else if (FromMagic == null)
        {
            Debug.Log("else if 1");
            if (clicked.name.Contains("Hotbar"))
            {
                FromMagic = Instantiate(MagicImagePrefab);
                FromMagic.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                FromMagic.name = clicked.name;
                FromMagic.transform.SetParent(GameObject.Find("MagicMenuCanvas").transform, true);
                clicked.GetComponent<Image>().sprite = DefaultImage;

                //for unassigning delegate functions if you move spell from hotbar slot
                assignHotbarDelegate(clicked.name, false);
            }
            else
            {
                FromMagic = (GameObject)Instantiate(MagicImagePrefab);
                FromMagic.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                FromMagic.name = clicked.name;
                FromMagic.transform.SetParent(GameObject.Find("MagicMenuCanvas").transform, true);
            }
        }

        else if (ToMagic == null)
        {
            Debug.Log("else if 2");
            if (clicked.name.Contains("Hotbar"))
            {
                if(clicked.GetComponent<Image>().sprite.name.Contains("Inv Slot"))
                {
                    //for assigning delegate functions
                    assignHotbarDelegate(clicked.name, true);
                    clicked.GetComponent<Image>().sprite = FromMagic.GetComponent<Image>().sprite;
                    Destroy(FromMagic);
                }
                //for swaping place of items you clicked on
                else
                {
                    ToMagic = Instantiate(MagicImagePrefab);
                    ToMagic.transform.SetParent(GameObject.Find("MagicMenuCanvas").transform, true);
                    ToMagic.GetComponent<Image>().sprite = FromMagic.GetComponent<Image>().sprite;
                    FromMagic.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                    clicked.GetComponent<Image>().sprite = ToMagic.GetComponent<Image>().sprite;
                    
                    //for assigning delegate functions
                    assignHotbarDelegate(clicked.name, true);
                    Destroy(ToMagic);
                }
            }
            else
            {
                //destorying ig you click somewhere random
                Destroy(FromMagic);
            }
        }
        else
        {
            Debug.Log("else");
            Destroy(FromMagic);
        }
    }

    //for assigning hotkey methods in gamecontroller
    void assignHotbarDelegate(string hotbarName, bool On)
    {
        if (hotbarName.Contains("HotbarButtonOne"))
        {
            if (On)
            {
                GameController.GameControllerSingle.HotBarSlot1 = test;
            }
            else
            {
                GameController.GameControllerSingle.HotBarSlot1 = null;
            }
        }
        else if (hotbarName.Contains("HotbarButtonTwo"))
        {
            if (On)
            {
                GameController.GameControllerSingle.HotBarSlot2 = test2;
            }
            else
            {
                GameController.GameControllerSingle.HotBarSlot2 = null;
            }
        }
        else if (hotbarName.Contains("HotbarButtonThree"))
        {
            if (On)
            {
                GameController.GameControllerSingle.HotBarSlot3 = test3;
            }
            else
            {
                GameController.GameControllerSingle.HotBarSlot3 = null;
            }
        }
    }

    void test()
    {
        Debug.Log("test 1");
    }

    void test2()
    {
        Debug.Log("test 2");
    }

    void test3()
    {
        Debug.Log("test 3");
    }
}
