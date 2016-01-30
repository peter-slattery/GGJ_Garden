using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> tools = new List<InventoryItem>();
    public List<InventoryItem> collectable = new List<InventoryItem>();
    public GameObject slotPrefab;
    public Transform slotContainer;
    public Transform itemContainer;
    public int slotCount = 15;

    public void Start ()
    {
        // Instintate items
        CreateSlots();
    }

    public void CreateSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            var slot = Instantiate(slotPrefab) as GameObject;
            slot.transform.parent = slotContainer;
        }
    }

    public void AddItem (InventoryItem prefab)
    {
        // Get and store the prefab type

        //GameObject instance = Instantiate(Resources.Load<GameObject>("MrMob"));
        //GameObject testPrefab = (GameObject)Resources.Load("/Prefabs/yourPrefab");

        // Is the picked up item a tool or a collectable?
        // Add item to appropriate array
    }

    public void UseTool()
    {
        // 
    }

    public void DropItem()
    {
        // Check if there is an item already on the ground
        // Create a new item of the object type on the ground (instintate prefab)

        // Is item a collectable and count > 0?
            // Substract from count
        // Else remove item from list
    }
}