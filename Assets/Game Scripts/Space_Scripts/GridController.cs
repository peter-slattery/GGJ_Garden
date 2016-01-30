using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour {

	public static SpaceGrid curInstance = null;

	public static YieldInstruction SimulationUpdateYield = new WaitForSeconds(0.4f);

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
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator runUpdate () {
		isPaused = false;
		while (isRunning) {
			curInstance.updateState();
			curInstance.updateProperties();
			yield return SimulationUpdateYield;
		}
		isPaused = true;
	}
}
