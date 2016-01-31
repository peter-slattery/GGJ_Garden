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

	public static Vector3 GridToWorld (CanAddr addr) {
		Vector2 tmp = LatAddr.convertLatAddrToVector (CanAddr.convertCanAddrToLatAddr(addr));
		Vector3 result = new Vector3 (tmp.y, 0, tmp.x);
		return (result * Config.WorldToGridScale);
	}

	public static CanAddr WorldToGrid (Vector3 vec) {
		vec /= Config.WorldToGridScale;
		Vector2 tmp = new Vector2 (vec.z, vec.x);
		CanAddr result = CanAddr.convertLatAddrToCanAddr (LatAddr.convertVectorToLatAddr(tmp));
		return result;
	}

	public static void CreateGrid (int[] tiles, int arrayWidth) {
		for (int i = 0; i < tiles.Length; i++) {
			LatAddr lAddr = new LatAddr ((i % arrayWidth), ((int)i / arrayWidth), 0);
			CanAddr cAddr = CanAddr.convertLatAddrToCanAddr(lAddr);
			GridController.getCurInstance ().setState (cAddr, (TileTypeController.TileType)tiles [i], Random.Range (0f, (Tile.GROWTH_MAX / 3)));
			Vector2 worldVec = GridController.GridToWorld (cAddr);
		}
		GridController.CreateVizForCurGrid ();
	}

	public static void CreateVizForCurGrid () {
		getCurInstance ().rootNode.CreateVizForTile (GameObject.FindObjectOfType<TileSingleton>() as TileSingleton);
	}
}
