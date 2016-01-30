using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
    

	void Start ()
    {
	
	}
	
    void Update ()
    { 
	
	}
    
    void OnMouseDown()
    {
        // this object was clicked - do something
        Destroy(this.gameObject);
    }
}
