using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellSlot : MonoBehaviour, IDropHandler {

    public GameObject spell
    {
        get
        {
            if(transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!spell)
        {
            Draghandler.spellBeingDragged.transform.SetParent(transform);
        }
    }
}
