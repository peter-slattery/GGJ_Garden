using UnityEngine;
using System.Collections;

public class Empty : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log ("Doing the thing");
			GridController.getCurInstance ().setState (new CanAddr (), TileTypeController.TileType.TILE_ROCK, 0f);
		}
	}
	
	void OnApplicationQuit() {
		
	}
}
