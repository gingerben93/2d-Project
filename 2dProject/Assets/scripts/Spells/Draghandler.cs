using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draghandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public static GameObject spellBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        spellBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }



    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }



    public void OnEndDrag(PointerEventData eventData)
    {
        spellBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Debug.Log(startParent);
        Debug.Log(transform.parent);
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
    }


}
