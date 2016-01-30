using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Q))
        {
            Inventory._instance.Toggle();
        }
	}
}
