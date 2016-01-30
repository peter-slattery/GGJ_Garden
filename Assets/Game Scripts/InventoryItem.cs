using UnityEngine;
using System.Collections;


public class InventoryItem : MonoBehaviour
{
    public string Name;
    public string Description;
    public Texture ItemIcon;
    public GameObject ItemModel; 
    public int Count;
    public ItemType Type;

    public enum ItemType
    {
        Tool
        , Fruit
        , Seed
    }

    public InventoryItem(string name, string description, int count, ItemType type)
    {
        Name = name;
        Description = description;
        Count = count;
        Type = type;
    }

    public InventoryItem()
    {

    }
}

