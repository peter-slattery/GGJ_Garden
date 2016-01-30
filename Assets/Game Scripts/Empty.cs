using UnityEngine;
using System.Collections;

public class Empty : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			GridController.getCurInstance ().updateProperties ();
			GridController.getCurInstance ().updateState ();
			Debug.Log ("TS: " + GridController.getCurInstance ().updateDiff.TotalSeconds);
		}
	}
	
	void OnApplicationQuit() {
		
	}
}
