using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    //Used when moving around items to equipment slots.
    public Item info, info2;

    //int alph; //For moving items when canvas is hidden

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

    public static Inventory InventorySingle;

    void Awake()
    {
        if (InventorySingle == null)
        {
            InventorySingle = this;
        }
        else if (InventorySingle != this)
        {
            Destroy(gameObject);
        }
    }


    //Tooltip for items
    public GameObject tooltipObject;
    private static GameObject tooltip;


    public Text sizeTextObject;
    private static Text sizeText;
    public Text visualTextObject;
    private static Text visualText;


    // Use this for initialization
    void Start()
    {
        tooltip = tooltipObject;
        sizeText = sizeTextObject;
        visualText = visualTextObject;

        CreateLayout();
    }

    // Update is called once per frame
    void Update()
    {
        //alph = (int)GetComponent<CanvasGroup>().alpha;
        if (Input.GetMouseButtonUp(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject(-1) && from != null)
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
                    newSlot.name = "HELM";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Helm location
                    slotRect.localPosition = invRect.localPosition + new Vector3(-5 + (slotSize * 6), -10 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                    //allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 1)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "AMULET";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Amulet Location
                    slotRect.localPosition = invRect.localPosition + new Vector3(5 + (slotSize * 7), -20 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                    //allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 2)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "WEAPON";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Weapon Location
                    slotRect.localPosition = invRect.localPosition + new Vector3((slotSize * 4), -30 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * 3);

                    //allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 3)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "CHEST";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Body Location
                    slotRect.localPosition = invRect.localPosition + new Vector3(5 + (slotSize * 5), -40 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * 2);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * 3);

                    //allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 4)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "SHIELD";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Shield Location
                    slotRect.localPosition = invRect.localPosition + new Vector3(-5 + (slotSize * 8), -50 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * 2);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * 2);

                    //allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 5)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "GLOVES";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Gloves Location
                    slotRect.localPosition = invRect.localPosition + new Vector3((slotSize * 4), -95 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * 2);

                    //allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 6)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "BELT";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Waist Location
                    slotRect.localPosition = invRect.localPosition + new Vector3(5 + (slotSize * 5), -110 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * 2);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                    //allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 7)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "BOOTS";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Boots Location
                    slotRect.localPosition = invRect.localPosition + new Vector3(5 + (slotSize * 5), -135 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * 2);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                    //allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 8)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "RING";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Ring1 Location
                    slotRect.localPosition = invRect.localPosition + new Vector3((slotSize * 8), -100 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                    //allSlots.Add(newSlot);
                    newSlot.transform.SetParent(this.transform);
                }
                if (y == 0 && x == 9)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "RING";
                    newSlot.transform.SetParent(this.transform.parent);
                    //Ring2 Location
                    slotRect.localPosition = invRect.localPosition + new Vector3((slotSize * 8), -125 - (slotSize * y));
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                    //allSlots.Add(newSlot);
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

                hoverObject.transform.SetParent(GameObject.Find("InvMenuCanvas").transform, true);
                hoverObject.transform.localScale = from.gameObject.transform.localScale;
            }
        }
        else if (to == null)
        {
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }
        if (to != null && from != null)
        {
            Stack<Item> tmpTo = new Stack<Item>(to.Items);
            
            if (tmpTo.Count == 0)
            {
                info = from.Items.Peek();
                //Debug.Log(info.type.ToString());
                //Debug.Log(to.gameObject.name);

                //Swapping items to an empty slot
                if (info.type.ToString() == to.gameObject.name.ToString() || to.gameObject.name == "Slot") {
                    if(info.type.ToString() == "WEAPON" && to.gameObject.name.ToString() == "WEAPON")
                    {
                        //Set weapon damage to projectile damage;
                        GameController.GameControllerSingle.damage = info.damage;
                        Debug.Log(info.damage);

                    }
                    Debug.Log(info.damage);
                    to.AddItems(from.Items);
                    from.ClearSlot();
                }
            }
            else
            {
                info = from.Items.Peek();
                info2 = to.Items.Peek();
                //Debug.Log(info.type.ToString());
                //Debug.Log(info2.type.ToString());
                //Debug.Log(from.gameObject.name);
                //Debug.Log(to.gameObject.name);

                //Swap items between each others slots. Check if the swap between two items will allow both items to switch into proper slots.
                if (info.type.ToString() == to.gameObject.name || to.gameObject.name == "Slot" && info2.type.ToString() == from.gameObject.name || from.gameObject.name == "Slot" && to.gameObject.name == "Slot")
                {
                    to.AddItems(from.Items);
                    from.AddItems(tmpTo);
                }
            }

            from.GetComponent<Image>().color = Color.white;
            info = null;
            info2 = null;
            to = null;
            from = null;
            hoverObject = null;
        }
    }

    public void ShowToolTip(GameObject slot)
    {
        Slot tmpSlot = slot.GetComponent<Slot>();

        //If item in a slot and hovering over item with mouse
        if (!tmpSlot.IsEmpty && hoverObject == null)
        {

            visualText.text = tmpSlot.CurrentItem.GetToolTip();
            sizeText.text = visualText.text;

            tooltip.SetActive(true);

            float xPos = slot.transform.position.x + slotPaddingLeft;
            float yPos = slot.transform.position.y - slot.GetComponent<RectTransform>().sizeDelta.y - slotPaddingTop;

            tooltip.transform.position = new Vector2(xPos, yPos);
        }
    }

    public void HideToolTip()
    {
        tooltip.SetActive(false);
    }
}
