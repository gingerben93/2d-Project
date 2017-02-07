using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{

    private RectTransform invRect;
    private float invWidth, invHeight;

    public int slots;
    public int rows;
    public float slotPaddingLeft, slotPaddingTop;
    public float slotSize;

    public GameObject slotPrefab;

    private List<GameObject> allSlots;

    private static int emptySlots;

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

    }

    private void CreateLayout()
    {
        allSlots = new List<GameObject>();

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
}
