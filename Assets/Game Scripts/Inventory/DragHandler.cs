using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public static GameObject itemBeingDragged;
    public bool isDropped;
    Vector3 startPosition;
    Transform startParent;
    
    public void OnBeginDrag (PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        this.transform.SetParent(startParent);

        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
    }

    void OnMouseDown()
    {
        Debug.Log("MOUSE DOWN");
    }
}
