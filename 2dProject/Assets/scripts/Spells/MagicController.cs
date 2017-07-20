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

        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
        position.Set(position.x, position.y);
        FromMagic.transform.position = canvas.transform.TransformPoint(position);

    }


    //public void clickmagic(GameObject clicked)
    //{
    //    //if nothing is floating with the cursor and you clicked on empty slot
    //    if (frommagic == null && clicked.getcomponent<image>().sprite.name.contains("inv slot"))
    //    {
    //        debug.log("if");
    //    }
    //    //for clicking on somthing
    //    else if (frommagic == null)
    //    {
    //        debug.log("else if 1");
    //        if (clicked.name.contains("hotbar"))
    //        {
    //            frommagic = instantiate(magicimageprefab);
    //            frommagic.getcomponent<image>().sprite = clicked.getcomponent<image>().sprite;
    //            frommagic.name = clicked.name;
    //            frommagic.transform.setparent(gameobject.find("magicmenucanvas").transform, true);
    //            clicked.getcomponent<image>().sprite = defaultimage;

    //            //for unassigning delegate functions if you move spell from hotbar slot
    //            assignhotbardelegate(clicked.name, false);
    //        }
    //        else
    //        {
    //            frommagic = (gameobject)instantiate(magicimageprefab);
    //            frommagic.getcomponent<image>().sprite = clicked.getcomponent<image>().sprite;
    //            frommagic.name = clicked.name;
    //            frommagic.transform.setparent(gameobject.find("magicmenucanvas").transform, true);
    //        }
    //    }

    //    else if (tomagic == null)
    //    {
    //        debug.log("else if 2");
    //        if (clicked.name.contains("hotbar"))
    //        {
    //            if (clicked.getcomponent<image>().sprite.name.contains("inv slot"))
    //            {
    //                //for assigning delegate functions
    //                assignhotbardelegate(clicked.name, true);
    //                clicked.getcomponent<image>().sprite = frommagic.getcomponent<image>().sprite;
    //                destroy(frommagic);
    //            }
    //            //for swaping place of items you clicked on
    //            else
    //            {
    //                tomagic = instantiate(magicimageprefab);
    //                tomagic.transform.setparent(gameobject.find("magicmenucanvas").transform, true);
    //                tomagic.getcomponent<image>().sprite = frommagic.getcomponent<image>().sprite;
    //                frommagic.getcomponent<image>().sprite = clicked.getcomponent<image>().sprite;
    //                clicked.getcomponent<image>().sprite = tomagic.getcomponent<image>().sprite;

    //                //for assigning delegate functions
    //                assignhotbardelegate(clicked.name, true);
    //                destroy(tomagic);
    //            }
    //        }
    //        else
    //        {
    //            //destorying ig you click somewhere random
    //            destroy(frommagic);
    //        }
    //    }
    //    else
    //    {
    //        debug.log("else");
    //        destroy(frommagic);
    //    }
    //}

    ////for assigning hotkey methods in gamecontroller
    //void assignhotbardelegate(string hotbarname, bool on)
    //{
    //    if (hotbarname.contains("hotbarbuttonone"))
    //    {
    //        if (on)
    //        {
    //            gamecontroller.gamecontrollersingle.hotbarslot1 = test;
    //        }
    //        else
    //        {
    //            gamecontroller.gamecontrollersingle.hotbarslot1 = null;
    //        }
    //    }
    //    else if (hotbarname.contains("hotbarbuttontwo"))
    //    {
    //        if (on)
    //        {
    //            gamecontroller.gamecontrollersingle.hotbarslot2 = test2;
    //        }
    //        else
    //        {
    //            gamecontroller.gamecontrollersingle.hotbarslot2 = null;
    //        }
    //    }
    //    else if (hotbarname.contains("hotbarbuttonthree"))
    //    {
    //        if (on)
    //        {
    //            gamecontroller.gamecontrollersingle.hotbarslot3 = test3;
    //        }
    //        else
    //        {
    //            gamecontroller.gamecontrollersingle.hotbarslot3 = null;
    //        }
    //    }
    //}

    //void test()
    //{
    //    debug.log("test 1");
    //}

    //void test2()
    //{
    //    debug.log("test 2");
    //}

    //void test3()
    //{
    //    debug.log("test 3");
    //}
}
