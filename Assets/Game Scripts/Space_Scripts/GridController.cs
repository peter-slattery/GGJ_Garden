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
		CanAddr tmp = new CanAddr ();
		GridController.getCurInstance ().setState (tmp, TileTypeController.TileType.TILE_BLANK, 0.0f);
		tmp.setTuple (0x1, 0);
		GridController.getCurInstance ().setState (tmp, TileTypeController.TileType.TILE_BLANK, 0.0f);
		StartCoroutine (runUpdate());
	}
	
	// Update is called once per frame
	void Update () {
		
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
		return (result / Config.WorldToGridScale);
	}

	public static CanAddr WorldToGrid (Vector2 vec) {
		vec *= Config.WorldToGridScale;
		CanAddr result = CanAddr.convertLatAddrToCanAddr (LatAddr.convertVectorToLatAddr(vec));
		return result;
	}
}
