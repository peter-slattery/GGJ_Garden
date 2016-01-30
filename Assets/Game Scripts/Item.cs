using UnityEngine;
using System.Collections;

public enum ItemType
{
    Tool
    , Fruit
    , Seed
}

public class Item : MonoBehaviour {

    public string Name;
    public string Description;
    public Sprite ItemIcon;
    public GameObject ItemModel;
    //public int Count;
    //public ItemType Type;
    
    void Start ()
    {
	
	}
	
    void Update ()
    { 
	
	}
    
    void OnMouseDown()
    {
        Inventory._instance.AddItem(this.gameObject);
        Destroy(this.gameObject);
    }
}
