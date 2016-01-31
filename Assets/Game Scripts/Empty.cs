using UnityEngine;
using System.Collections;

public class Empty : MonoBehaviour {

	// Use this for initialization
	void Start () {
		CanAddr tmp = new CanAddr ();
		Debug.Log ("(0,0,0): " + GridController.GridToWorld (tmp));

		tmp.setTuple (1, 0);
		Debug.Log ("(1,0,0): " + GridController.GridToWorld (tmp));
		tmp.setTuple (2, 0);
		Debug.Log ("(0,1,0): " + GridController.GridToWorld (tmp));
		tmp.setTuple (4, 0);
		Debug.Log ("(0,0,1): " + GridController.GridToWorld (tmp));
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log ("Doing the thing");
			CanAddr tmp = new CanAddr ();
			GridController.getCurInstance ().setState (tmp, TileTypeController.TileType.TILE_VINE, 0f);
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			GridController.getCurInstance ().setState (new CanAddr (), TileTypeController.TileType.TILE_TILLED, 0f);
		}
	}
	
	void OnApplicationQuit() {
		
	}
}
