using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicController : MonoBehaviour
{

    public GameObject MagicImagePrefab;
    public Sprite DefaultImage;
    private GameObject FromMagic;
    private GameObject ToMagic;

    //GameObject Attack;
    //GameObject HotBarSlot1, HotBarSlot2, HotBarSlot3;

    private Canvas canvas;


    // Use this for initialization
    void Start()
    {
        canvas = GetComponent<Canvas>();

        //Vector2 position;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
        //position.Set(position.x, position.y);
        //FromMagic.transform.position = canvas.transform.TransformPoint(position);

    }

    void Update()
    {
        if (FromMagic)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            position.Set(position.x, position.y);
            FromMagic.transform.position = canvas.transform.TransformPoint(position);
        }

        if (Input.GetMouseButtonUp(0) && FromMagic)
        {
            if (!EventSystem.current.IsPointerOverGameObject(-1))
            {
                Destroy(FromMagic);
                //hoverObject = null;
            }
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
            //Debug.Log("else if 1");
            if (clicked.name.Contains("Hotbar"))
            {
                //Debug.Log("else if 1 if");
                FromMagic = Instantiate(MagicImagePrefab, GameObject.Find("HotbarCanvas").transform);
                FromMagic.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                FromMagic.name = clicked.name;
                //FromMagic.transform.SetParent(GameObject.Find("MagicMenuCanvas").transform, true);
                //FromMagic.GetComponent<Magic>().type = clicked.GetComponent<Magic>().type;
                clicked.GetComponent<Image>().sprite = DefaultImage;

                //for unassigning delegate functions if you move spell from hotbar slot
                //Debug.Log(FromMagic.GetComponent<Image>().sprite.name);

                AssignHotbarDelegate(clicked.name, "none", null, false);
            }
            else
            {
                
                FromMagic = Instantiate(MagicImagePrefab, GameObject.Find("HotbarCanvas").transform);
                //Debug.Log("FromMagic = " + FromMagic.name);
                //Debug.Log("clicked = " + clicked.name);
                FromMagic.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                FromMagic.name = clicked.name;
                //FromMagic.transform.SetParent(GameObject.Find("MagicMenuCanvas").transform, true);
                //Debug.Log("clicked.GetComponent<Magic>().type + " + clicked.GetComponent<Magic>().type);
                //Debug.Log("FromMagic.GetComponent<Magic>().type + " + FromMagic.GetComponent<Magic>().type);
                //FromMagic.GetComponent<Magic>().type = clicked.GetComponent<Magic>().type;
            }
        }

        else if (ToMagic == null)
        {
            //Debug.Log("else if 2");
            if (clicked.name.Contains("Hotbar"))
            {
                if (clicked.GetComponent<Image>().sprite.name.Contains("Inv Slot"))
                {
                    //for assigning delegate functions
                    Debug.Log(FromMagic.GetComponent<Image>().sprite.name);
                    //assignhotbardelegate(clicked.name, FromMagic.GetComponent<Image>().sprite.name, true);

                    clicked.GetComponent<Image>().sprite = FromMagic.GetComponent<Image>().sprite;
                    //clicked.GetComponent<Magic>().type = FromMagic.GetComponent<Magic>().type;

                    //pass spell type
                    AssignHotbarDelegate(clicked.name, FromMagic.GetComponent<Image>().sprite.name, clicked.GetComponent<Magic>().type, true);

                    Destroy(FromMagic);
                }
                //for swaping place of items you clicked on
                else
                {
                    ToMagic = Instantiate(MagicImagePrefab, GameObject.Find("HotbarCanvas").transform);
                    //ToMagic.transform.SetParent(GameObject.Find("MagicMenuCanvas").transform, true);
                    ToMagic.GetComponent<Image>().sprite = FromMagic.GetComponent<Image>().sprite;
                    //ToMagic.GetComponent<Magic>().type = FromMagic.GetComponent<Magic>().type;
                    FromMagic.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                    //FromMagic.GetComponent<Magic>().type = clicked.GetComponent<Magic>().type;
                    clicked.GetComponent<Image>().sprite = ToMagic.GetComponent<Image>().sprite;
                    //clicked.GetComponent<Magic>().type = ToMagic.GetComponent<Magic>().type;

                    //for assigning delegate functions
                    //Debug.Log(ToMagic.GetComponent<Image>().sprite.name);
                    AssignHotbarDelegate(clicked.name, clicked.GetComponent<Image>().sprite.name, clicked.GetComponent<Magic>().type, true);
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
    void AssignHotbarDelegate(string hotbarName, string spellName, Enum type, bool on)
    {
        if (hotbarName.Contains("HotbarButtonOne"))
        {
            if (on)
            {
                PlayerController.PlayerControllerSingle.HotBarSlot1 = GameObject.Find(hotbarName).GetComponent<Magic>().Cast;
                //SelectWeapon(spellName, ref GameController.GameControllerSingle.HotBarSlot1, ref HotBarSlot1);
            }
            else
            {
                //Destroy(HotBarSlot1);
                PlayerController.PlayerControllerSingle.HotBarSlot1 = null;
            }
        }
        else if (hotbarName.Contains("HotbarButtonTwo"))
        {
            if (on)
            {
                PlayerController.PlayerControllerSingle.HotBarSlot2 = GameObject.Find(hotbarName).GetComponent<Magic>().Cast;
                //    SelectWeapon(spellName, ref GameController.GameControllerSingle.HotBarSlot2, ref HotBarSlot2);
            }
            else
            {
                //Destroy(HotBarSlot2);
                PlayerController.PlayerControllerSingle.HotBarSlot2 = null;
            }
        }
        else if (hotbarName.Contains("HotbarButtonThree"))
        {
            if (on)
            {
                PlayerController.PlayerControllerSingle.HotBarSlot3 = GameObject.Find(hotbarName).GetComponent<Magic>().Cast;
                //    SelectWeapon(spellName, ref GameController.GameControllerSingle.HotBarSlot3, ref HotBarSlot3);
            }
            else
            {
                //Destroy(HotBarSlot3);
                PlayerController.PlayerControllerSingle.HotBarSlot3 = null;
            }
        }
    }
}
