using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        // Prevent Player from walking when doing inventory clicks
        InputHandler inputHandler = FindObjectOfType(typeof(InputHandler)) as InputHandler;
        inputHandler.CloseInventory();

        isOpen = false;
        this.gameObject.SetActive(false);
    }

    public void Open()
    {
        // Prevent Player from walking when doing inventory clicks
        InputHandler inputHandler = FindObjectOfType(typeof(InputHandler)) as InputHandler;
        inputHandler.OpenInventory();

        isOpen = true;
        this.gameObject.SetActive(true);
        DisplayItems();
    }

    public void Toggle()
    {
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
			slot.transform.SetParent(slotContainer);
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
        for (int i = 0; i < itemsList.Count; i++)
        {
            int index = i; // Store it in a variable because for whatever reason it doesn't get the right i in UseItem and Index without it :/ 

            if (!itemsList[i].isRemovedFromInventory)  // HACK: see the TODO in Inventory.cs > RemoveItem()
            {
                GameObject item = new GameObject();
                item.AddComponent<Image>();
                item.AddComponent<InventoryItem>();
                item.GetComponent<InventoryItem>().Index = i;
                item.name = "item_" + index;
                item.AddComponent<CanvasGroup>();
                item.AddComponent<Button>();
                item.GetComponent<Button>().onClick.AddListener(() => { UseItem(index); });
                item.GetComponent<Image>().sprite = itemsList[i].ItemIcon;
                item.transform.SetParent(itemContainer);
            }
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

    public void UseItem(int index)
    {
        Item currItem = itemsList[index];
        ItemType itemType = currItem.m_ItemType;
        GameObject currGameObject = GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject;

        Debug.Log(itemType);

        switch (itemType)
        {
            case ItemType.Seed:
                Debug.Log("Plant a Seed");
                RemoveItem(index, currGameObject);
                break;
            case ItemType.Tool:
                Debug.Log("Do a tool thing");
                break;
            case ItemType.Normal:
                break;
            default:
                break;
        }
    }

    public void DropItem(GameObject item)
    {
        int index = item.GetComponent<InventoryItem>().Index;
        Item currItem = itemsList[index];

        // TODO: Check if there is an item already on the ground?

        // Create a new item of the object type where the player is
        GameObject dropppedItem = Instantiate(Resources.Load(currItem.PrefabName)) as GameObject;
        dropppedItem.transform.position = Object.FindObjectOfType<PlayerCharacterController>().transform.position;

        RemoveItem(index, item);
    }

    void RemoveItem(int index, GameObject physicalRepresentation)
    {
        // TODO: Optimize this....The lists change index when you delete things, and we need the index to be correct to delete items. For now, I'm setting a bool to null and keeping the removed item in the list, but there's got to be a better way.
        // "Remove" item from list
        Item currItem = itemsList[index];
        currItem.isRemovedFromInventory = true;
        // Destroy sprite
        Destroy(physicalRepresentation);
    }

}