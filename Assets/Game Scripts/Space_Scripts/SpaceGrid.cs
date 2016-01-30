using UnityEngine;
using System.Collections;

public class SpaceGrid : MonoBehaviour {

	public static SpaceGrid singleton = null;
	
	public static YieldInstruction SimulationUpdateYield = new WaitForSeconds(0.4f);

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
			updateState();
			updateProperties();
			yield return SimulationUpdateYield;
		}
		isPaused = true;
	}

	private static Node rootNode = null;
	public static int propertyIndex = 0;
	
	public Tile getTile (CanAddr cAddr) {
		if (rootNode == null) { rootNode = new Node(); }
		return rootNode.getTile(cAddr);
	}
	
	public void updateState () {
		if (rootNode == null) { rootNode = new Node(); }
		rootNode.updateState();
	}
	
	public void updateProperties () {
		if (rootNode == null) { rootNode = new Node(); }
		propertyIndex = (propertyIndex + 1) % 2;
		rootNode.updateProperties();
	}
	
	public void setState (CanAddr cAddr, byte state) {
		if (rootNode == null) { rootNode = new Node(); }
		rootNode.setState(cAddr, state);
	}
}
