using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory _instance;
    public List<Item> toolsList = new List<Item>();
    public List<Item> itemsList = new List<Item>();
    public GameObject slotPrefab;
    public Transform slotContainer;
    public Transform itemContainer;
    public int slotCount = 15;
    public bool isOpen = false;

    void Awake()
    {
        _instance = this;
    }

    public void Start ()
    {
        CreateSlots();
        Close();
    }

    public void Close()
    {
        isOpen = false;
        this.gameObject.SetActive(false);
    }

    public void Open()
    {
        isOpen = true;
        this.gameObject.SetActive(true);
        // TODO: Delete current items???
        DisplayItems();
    }

    public void Toggle()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    void CreateSlots ()
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab);
            slot.transform.parent = slotContainer;
        }
    }

    void DisplayItems ()
    {
        Debug.Log("Item count: " + itemsList.Count);
        for (int i = 0; i < itemsList.Count; i++)
        {
            Debug.Log("Show "+ itemsList[i].Name);
            GameObject item = new GameObject();
            item.AddComponent<Image>();
            //item.sprite = itemsList[i].ItemIcon;

            GameObject itemInstance = Instantiate(item);
            itemInstance.transform.parent = itemContainer;
        }
    }

    public void AddItem (GameObject item)
    {
        Item inventoryItem = item.GetComponent<Item>();
        itemsList.Add(inventoryItem);

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