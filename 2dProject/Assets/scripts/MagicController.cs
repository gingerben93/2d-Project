using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicController : MonoBehaviour {

    public GameObject MagicImagePrefab;
    private static GameObject ClickedMagic;

    private Canvas canvas;

    // Use this for initialization
    void Start () {

        canvas = GetComponent<Canvas>();
    }
	
	// Update is called once per frame
	void Update () {

        if (ClickedMagic != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            position.Set(position.x, position.y);
            ClickedMagic.transform.position = canvas.transform.TransformPoint(position);
        }
    }
    public void ClickMagic(GameObject clicked)
    {
        if (ClickedMagic == null)
        {
            Debug.Log("if");
            ClickedMagic = (GameObject)Instantiate(MagicImagePrefab);
            ClickedMagic.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
            ClickedMagic.name = clicked.name;
            ClickedMagic.transform.SetParent(GameObject.Find("MagicMenuCanvas").transform, true);
        }
        else if(clicked.name.Contains("Hotbar"))
        {
            Debug.Log("else if");
            clicked.GetComponent<Image>().sprite = ClickedMagic.GetComponent<Image>().sprite;
            Destroy(ClickedMagic);
        }
        else
        {
            Debug.Log("else");
            Destroy(ClickedMagic);
        }


            //from = clicked.GetComponent<Slot>();
            //from.GetComponent<Image>().color = Color.gray;

        //hoverObject = (GameObject)Instantiate(iconPrefab);
        //hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
        //hoverObject.name = "Hover";

        //RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
        //RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

        //hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
        //hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

        //hoverObject.transform.SetParent(GameObject.Find("InvMenuCanvas").transform, true);
        //hoverObject.transform.localScale = from.gameObject.transform.localScale;

    }
}
