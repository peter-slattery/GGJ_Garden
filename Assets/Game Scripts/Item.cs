using UnityEngine;
using System.Collections;

public enum ItemType
{
    Tool
    , Fruit
    , Seed
}

public class Item : MonoBehaviour {
    
    public string PrefabName;
    public string Description;
    public Sprite ItemIcon;
    
    void Start ()
    {
	
	}
	
    void Update ()
    { 
	
	}
    
    void OnMouseDown()
    {
        if (Inventory._instance.AddItem(this.gameObject))
        {
            Destroy(this.gameObject);
        }
        else
        {
            // Too many items in bag
            // TODO: play sound?
        }
    }
}
