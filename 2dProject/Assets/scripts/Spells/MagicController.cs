using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicController : MonoBehaviour {

    public GameObject MagicImagePrefab;
    public Sprite DefaultImage;
    private GameObject FromMagic;
    private GameObject ToMagic;

    GameObject Attack;
    GameObject HotBarSlot1, HotBarSlot2, HotBarSlot3;

    private Canvas canvas;

    // Use this for initialization
    void Start () {

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

    public void clickmagic(GameObject clicked)
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
                FromMagic = Instantiate(MagicImagePrefab);
                FromMagic.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                FromMagic.name = clicked.name;
                FromMagic.transform.SetParent(GameObject.Find("MagicMenuCanvas").transform, true);
                clicked.GetComponent<Image>().sprite = DefaultImage;

                //for unassigning delegate functions if you move spell from hotbar slot
                //Debug.Log(FromMagic.GetComponent<Image>().sprite.name);
                assignhotbardelegate(clicked.name, "none" , false);
            }
            else
            {
                //Debug.Log("else if 1 else");
                FromMagic = (GameObject)Instantiate(MagicImagePrefab);
                FromMagic.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                FromMagic.name = clicked.name;
                FromMagic.transform.SetParent(GameObject.Find("MagicMenuCanvas").transform, true);
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
                    assignhotbardelegate(clicked.name, FromMagic.GetComponent<Image>().sprite.name, true);
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
                    Debug.Log(ToMagic.GetComponent<Image>().sprite.name);
                    assignhotbardelegate(clicked.name, ToMagic.GetComponent<Image>().sprite.name, true);
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
    void assignhotbardelegate(string hotbarName, string spellName, bool on)
    {
        Debug.Log("assignhotbardelegate");
        Debug.Log(hotbarName);
        Debug.Log(spellName);
        if (hotbarName.Contains("HotbarButtonOne"))
        {
            if (on)
            {
                SelectWeapon(spellName, ref GameController.GameControllerSingle.HotBarSlot1, ref HotBarSlot1);
                //GameController.GameControllerSingle.HotBarSlot1 = test;
            }
            else
            {
                Destroy(HotBarSlot1);
                GameController.GameControllerSingle.HotBarSlot1 = null;
            }
        }
        else if (hotbarName.Contains("HotbarButtonTwo"))
        {
            if (on)
            {
                SelectWeapon(spellName, ref GameController.GameControllerSingle.HotBarSlot2, ref HotBarSlot2);
            }
            else
            {
                Destroy(HotBarSlot2);
                GameController.GameControllerSingle.HotBarSlot2 = null;
            }
        }
        else if (hotbarName.Contains("HotbarButtonThree"))
        {
            if (on)
            {
                SelectWeapon(spellName, ref GameController.GameControllerSingle.HotBarSlot3, ref HotBarSlot3);
            }
            else
            {
                Destroy(HotBarSlot3);
                GameController.GameControllerSingle.HotBarSlot3 = null;
            }
        }
    }

    void SelectWeapon(string weaponName, ref GameController.HotBarDelegate delegateTest , ref GameObject Spell)
    {
        Debug.Log("selectweapon");
        Debug.Log(weaponName);
        if (weaponName == "Blowdart")
        {
            //destroy current
            Destroy(Spell);

            //instantiate a object with the weapon attack on it
            Attack = Resources.Load("Prefabs/WeaponAttacks/BlowDartAttack", typeof(GameObject)) as GameObject;
            Spell = Instantiate(Attack, GameObject.Find("SpellAttacks").transform);
            Spell.transform.localPosition = Vector3.zero;
            Blowdart temp = Spell.GetComponent<Blowdart>();
            delegateTest = temp.Attack;
        }
        else if (weaponName == "Sword")
        {
            //destroy current
            Destroy(Spell);

            //instantiate a object with the weapon attack on it
            Attack = Resources.Load("Prefabs/WeaponAttacks/SwordAttack", typeof(GameObject)) as GameObject;
            Spell = Instantiate(Attack, GameObject.Find("SpellAttacks").transform);
            Spell.transform.localPosition = Vector3.zero;
            ShortSword temp = Spell.GetComponent<ShortSword>();
            delegateTest = temp.Attack;
        }
        else
        {
            Destroy(Spell);
            Attack = Resources.Load("Prefabs/WeaponAttacks/GodHands", typeof(GameObject)) as GameObject;
            Spell = Instantiate(Attack, GameObject.Find("SpellAttacks").transform);
            Spell.transform.localPosition = Vector3.zero;
            GodHands temp = Spell.GetComponent<GodHands>();
            temp.targetLocation = GameObject.Find("Hero").transform.position;
            delegateTest = temp.Attack;
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
