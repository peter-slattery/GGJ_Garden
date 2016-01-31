using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour {

	public static SpaceGrid curInstance = null;

	public static YieldInstruction SimulationUpdateYield = new WaitForSeconds(Config.TIME_UNIT_STANDARD);

	public static SpaceGrid getCurInstance () {
		if (curInstance == null) {
			curInstance = new SpaceGrid ();
		}
		return curInstance;
	}

	public static void setCurInstance (SpaceGrid sG) {
		curInstance = sG;
	}

	public bool isRunning = true;

	// Use this for initialization
	void Start () {
		StartCoroutine (runUpdate());
	}

	IEnumerator runUpdate () {
		while (isRunning) {
			GridController.getCurInstance().updateState();
			GridController.getCurInstance().updateProperties();
			yield return SimulationUpdateYield;
		}
	}

	public static Vector2 GridToWorld (CanAddr addr) {
		Vector2 result = LatAddr.convertLatAddrToVector (CanAddr.convertCanAddrToLatAddr(addr));
		return (result * Config.WorldToGridScale);
	}

	public static CanAddr WorldToGrid (Vector2 vec) {
		vec /= Config.WorldToGridScale;
		CanAddr result = CanAddr.convertLatAddrToCanAddr (LatAddr.convertVectorToLatAddr(vec));
		return result;
	}

	public static void CreateGrid (int[] tiles, int arrayWidth) {
		for (int i = 0; i < tiles.Length; i++) {
			CanAddr tmp = CanAddr.convertLatAddrToCanAddr(new LatAddr ((i % arrayWidth), ((int) i / arrayWidth), 0));
			GridController.getCurInstance ().setState (tmp, (TileTypeController.TileType)tiles [i], Random.Range (0f, (Tile.GROWTH_MAX / 3)));
			Vector2 worldVec = GridController.GridToWorld (tmp);
		}
		//GridController.CreateVizForCurGrid ();
	}

	public static void CreateVizForCurGrid () {
		getCurInstance ().rootNode.CreateVizForTile (GameObject.FindObjectOfType<TileSingleton>() as TileSingleton);
	}
}
