using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory _instance;
    public List<Item> itemsList = new List<Item>();
    public GameObject slotPrefab;
    public Transform slotContainer;
    public Transform itemContainer;
    public int bagSize = 15;
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
        DisplayItems();
    }

    public void Toggle()
    {
        // Prevent Player from walking when doing inventory clicks
        InputHandler inputHandler = FindObjectOfType(typeof (InputHandler)) as InputHandler;
        inputHandler.toggleInventory();

        // Toggle visibility
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
        for (int i = 0; i < bagSize; i++)
        {
            GameObject slot = Instantiate(slotPrefab);
            slot.transform.parent = slotContainer;
        }
    }

    void DisplayItems ()
    {
        // TODO: maybe find a better way to do this? idk :(
        // Delete items currently in the container
        foreach (Transform child in itemContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        // Add items again
        Debug.Log("Item count: " + itemsList.Count);
        for (int i = 0; i < itemsList.Count; i++)
        {
            GameObject item = new GameObject();
            item.AddComponent<Image>();
            item.AddComponent<InventoryItem>();
            item.GetComponent<InventoryItem>().Index = i;
            item.AddComponent<CanvasGroup>();
            item.GetComponent<Image>().sprite = itemsList[i].ItemIcon;
            item.transform.SetParent(itemContainer);
        }
    }

    public bool AddItem (GameObject item)
    {
        if (itemsList.Count < bagSize)
        {
            Item inventoryItem = item.GetComponent<Item>();
            itemsList.Add(inventoryItem);
            return true;
        }
        else
        {
            return false;
        }        
    }

    public void UseTool()
    {
        // 
    }

    public void DropItem(GameObject item)
    {
        int index = item.GetComponent<InventoryItem>().Index;
        Item currItem = itemsList[index];

        // TODO: Check if there is an item already on the ground?

        // Create a new item of the object type where the player is
        GameObject dropppedItem = Instantiate(Resources.Load(currItem.PrefabName)) as GameObject;
        dropppedItem.transform.position = Object.FindObjectOfType<PlayerCharacterController>().transform.position;
        
        // Remove item from list
        itemsList.Remove(currItem);
        Destroy(item);
    }
}