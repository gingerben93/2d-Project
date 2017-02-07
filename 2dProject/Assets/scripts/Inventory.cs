using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{

    private RectTransform invRect;
    private float invWidth, invHeight;

    public int slots;
    public int rows;
    public float slotPaddingLeft, slotPaddingTop;
    public float slotSize;

    public GameObject slotPrefab;

    private static Slot from, to;

    private List<GameObject> allSlots;

    public GameObject iconPrefab;
    private static GameObject hoverObject;

    private static int emptySlots;

    public Canvas canvas;

    private float hoverYOffset;

    public EventSystem eventSystem;

    public static int EmptySlots
    {
        get
        {
            return emptySlots;
        }

        set
        {
            emptySlots = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        CreateLayout();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null)
            {
                from.GetComponent<Image>().color = Color.white;
                from.ClearSlot();
                Destroy(GameObject.Find("Hover"));
                to = null;
                from = null;
                hoverObject = null;
            }
        }

        if (hoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            position.Set(position.x, position.y - hoverYOffset);
            hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    private void CreateLayout()
    {
        allSlots = new List<GameObject>();

        hoverYOffset = slotSize * 0.01f;

        emptySlots = slots;

        /*
        slots = 128;
        rows = 12;
        slotSize = 2;
        slotPaddingLeft = 2;
        slotPaddingTop = 2;
        */

        invWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        invHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        invRect = GetComponent<RectTransform>();
        invRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, invWidth);
        invRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, invHeight);

        int columns = slots / rows;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (y == 0 && x == 0)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "Helm";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Helm location
                    slotRect.localPosition = invRect.localPosition + new Vector3(-5 + (slotSize * 6), -10 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                    allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 1)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "Amulet";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Amulet Location
                    slotRect.localPosition = invRect.localPosition + new Vector3(5 + (slotSize * 7), -20 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                    allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 2)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "Weapon";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Weapon Location
                    slotRect.localPosition = invRect.localPosition + new Vector3((slotSize * 4), -30 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * 3);

                    allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 3)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "Chest";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Body Location
                    slotRect.localPosition = invRect.localPosition + new Vector3(5 + (slotSize * 5), -40 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * 2);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * 3);

                    allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 4)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "OffHand";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Shield Location
                    slotRect.localPosition = invRect.localPosition + new Vector3(-5 + (slotSize * 8), -50 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * 2);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * 2);

                    allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 5)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "Gloves";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Gloves Location
                    slotRect.localPosition = invRect.localPosition + new Vector3((slotSize * 4), -95 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * 2);

                    allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 6)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "Waist";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Waist Location
                    slotRect.localPosition = invRect.localPosition + new Vector3(5 + (slotSize * 5), -110 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * 2);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                    allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 7)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "Boots";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Boots Location
                    slotRect.localPosition = invRect.localPosition + new Vector3(5 + (slotSize * 5), -135 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * 2);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                    allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 8)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "Ring1";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Ring1 Location
                    slotRect.localPosition = invRect.localPosition + new Vector3((slotSize * 8), -100 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                    allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 9)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "Ring2";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Ring2 Location
                    slotRect.localPosition = invRect.localPosition + new Vector3((slotSize * 8), -125 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                    allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }

                if (y > 7)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "Slot";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Inventory slots
                    slotRect.localPosition = invRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                    allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
            }
        }
        //Helm Location


    }

    public bool AddItem(Item item)
    {
        if (item.maxSize == 1)
        {
            PlaceEmpty(item);
            return true;
        }
        else
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if (!tmp.IsEmpty)
                {
                    if (tmp.CurrentItem.type == item.type && tmp.IsAvailable)
                    {
                        tmp.AddItem(item);
                        return true;
                    }
                }
            }
            if (emptySlots > 0)
            {
                PlaceEmpty(item);
            }
        }
        return false;
    }

    private bool PlaceEmpty(Item item)
    {
        if (emptySlots > 0)
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();


                if (tmp.IsEmpty)
                {
                    tmp.AddItem(item);
                    emptySlots--;
                    return true;
                }
            }
        }
        return false;
    }

    public void MoveItem(GameObject clicked)
    {
        if (from == null)
        {
            if (!clicked.GetComponent<Slot>().IsEmpty)
            {
                from = clicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.gray;

                hoverObject = (GameObject)Instantiate(iconPrefab);
                hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                hoverObject.name = "Hover";

                RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

                hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
                hoverObject.transform.localScale = from.gameObject.transform.localScale;
            }
        }
        else if(to == null)
        {
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }
        if (to != null && from != null)
        {
            Stack<Item> tmpTo = new Stack<Item>(to.Items);
            to.AddItems(from.Items);

            if (tmpTo.Count == 0)
            {
                from.ClearSlot();
            }
            else
            {
                from.AddItems(tmpTo);
            }

            from.GetComponent<Image>().color = Color.white;
            to = null;
            from = null;
            hoverObject = null;
        }
    }
}
