using UnityEngine;
using System.Collections;

public class Empty : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("Effect of Flower on Tree: " + Tile.INTER_TILE_EFFECTS[Tile.FLOWERS_INDEX, Tile.TREE_INDEX]);
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
