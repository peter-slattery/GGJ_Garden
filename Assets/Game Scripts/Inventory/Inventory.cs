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
    public AudioClip openSound;
    public AudioClip dropItemSound;
    public AudioClip useItemSound;
    public AudioClip failSound;
    public AudioClip pickupSound;

    private AudioSource audioSource;

    void Awake()
    {
        _instance = this;
        // HACK: Using the PLayer's AudioSource to play sounds because Items cannot play sounds on pickup (GameObject is destroyed/unaccessible and Inventory is disabled)
        audioSource = Object.FindObjectOfType<PlayerCharacterController>().GetComponent<AudioSource>();
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
        audioSource.PlayOneShot(openSound, 1.0f);
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
            audioSource.PlayOneShot(pickupSound, 1.0f);
            Item inventoryItem = item.GetComponent<Item>();
            itemsList.Add(inventoryItem);
            return true;
        }
        else
        {
            audioSource.PlayOneShot(failSound, 1.0f);
            return false;
        }        
    }
    
    public void UseItem(int index)
    {
        Item currItem = itemsList[index];
        ItemType itemType = currItem.m_ItemType;
        GameObject currGameObject = GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject;

        Vector3 playerPos = Object.FindObjectOfType<PlayerCharacterController>().transform.position;
        Tile tileStandingOn = GridController.getCurInstance().getTile(GridController.WorldToGrid(playerPos));

        Debug.Log(itemType);

        switch (itemType)
        {
            case ItemType.Seed:
                Debug.Log("Plant a Seed"+ tileStandingOn.tileType);
                if (tileStandingOn.isTilled())
                {
                    Debug.Log("Setting tile to: " + currItem.m_TileType);
                    tileStandingOn.setState(null, currItem.m_TileType, 0f);
                    audioSource.PlayOneShot(useItemSound, 1.0f);
                    RemoveItem(index, currGameObject);
                }
                else
                {
                    audioSource.PlayOneShot(failSound, 1.0f);
                    // TODO: play nope noise?
                }
                break;
            case ItemType.Tool:
                Debug.Log("Do a tool thing");
                tileStandingOn.setState(null, currItem.m_TileType, 0f);
                audioSource.PlayOneShot(useItemSound, 1.0f);
                Debug.Log("Setting tile to: " + currItem.m_TileType);
                break;
            case ItemType.Normal:
                break;
            default:
                break;
        }
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

    public void DropItem(GameObject item)
    {
        audioSource.PlayOneShot(dropItemSound, 1.0f);
        int index = item.GetComponent<InventoryItem>().Index;
        Item currItem = itemsList[index];

        // TODO: Check if there is an item already on the ground?

        // Create a new item of the object type where the player is
        GameObject dropppedItem = Instantiate(Resources.Load(currItem.PrefabName)) as GameObject;
        dropppedItem.transform.position = Object.FindObjectOfType<PlayerCharacterController>().transform.position;

        RemoveItem(index, item);
    }

}