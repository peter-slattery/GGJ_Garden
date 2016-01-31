using UnityEngine;
using System.Collections;

public class Empty : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			GridController.CreateGrid (MapArray.mapIntArray, MapArray.mapWidth);
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			
		}
	}
	
	void OnApplicationQuit() {
		
	}
}
