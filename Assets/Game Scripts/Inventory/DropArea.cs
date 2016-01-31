using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DropArea : MonoBehaviour, IDropHandler {
    
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        Debug.Log("dropped");
        Inventory._instance.DropItem(eventData.pointerDrag);
    }
}
