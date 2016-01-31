using UnityEngine;
using System.Collections;

public class Empty : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			GridController.getCurInstance ().getTile (new CanAddr());
		}
	}
	
	void OnApplicationQuit() {
		
	}
}
