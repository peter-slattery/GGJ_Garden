using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InventoryItem : DragHandler {   
	public int Index { get; set; }
    public ItemType m_ItemType; 
}
