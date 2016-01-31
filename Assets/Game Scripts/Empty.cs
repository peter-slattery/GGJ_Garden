using UnityEngine;
using System.Collections;

public class Empty : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log ("Doing the thing");
			CanAddr tmp = new CanAddr ();
			GridController.getCurInstance ().setState (tmp, TileTypeController.TileType.TILE_VINE, 0f);
			tmp.setTuple (1, 0);
			GridController.getCurInstance ().setState (tmp, TileTypeController.TileType.TILE_BLANK, 0f);
			tmp.setTuple (2, 0);
			GridController.getCurInstance ().setState (tmp, TileTypeController.TileType.TILE_BLANK, 0f);
			tmp.setTuple (3, 0);
			GridController.getCurInstance ().setState (tmp, TileTypeController.TileType.TILE_BLANK, 0f);
			tmp.setTuple (4, 0);
			GridController.getCurInstance ().setState (tmp, TileTypeController.TileType.TILE_BLANK, 0f);
			tmp.setTuple (5, 0);
			GridController.getCurInstance ().setState (tmp, TileTypeController.TileType.TILE_BLANK, 0f);
			tmp.setTuple (6, 0);
			GridController.getCurInstance ().setState (tmp, TileTypeController.TileType.TILE_BLANK, 0f);
		}
	}
	
	void OnApplicationQuit() {
		
	}
}
