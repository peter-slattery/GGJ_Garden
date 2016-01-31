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

	public bool isRunning = false;
	public bool isPaused = true;

	// Use this for initialization
	void Start () {
		CanAddr tmp = new CanAddr ();
		GridController.getCurInstance ().setState (tmp, Tile.BLANK_MASK, 0.0f, null);
		tmp.setTuple (0x1, 0);
		GridController.getCurInstance ().setState (tmp, Tile.BLANK_MASK, 0.0f, null);
		StartCoroutine (runUpdate());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator runUpdate () {
		isPaused = false;
		while (isRunning) {
			GridController.getCurInstance().updateState();
			GridController.getCurInstance().updateProperties();
			yield return SimulationUpdateYield;
		}
		isPaused = true;
	}
}
