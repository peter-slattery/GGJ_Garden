using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpaceGrid {

	public System.DateTime lastUpdate = System.DateTime.UtcNow;
	public System.DateTime curUpdate = System.DateTime.UtcNow;

	public System.TimeSpan updateDiff;

	public Node rootNode = null;
	
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
		lastUpdate = curUpdate;
		curUpdate = System.DateTime.UtcNow;
		updateDiff = curUpdate - lastUpdate;
		rootNode.updateProperties();
	}
	
	public void setState (CanAddr cAddr, byte state) {
		if (rootNode == null) { rootNode = new Node(); }
		rootNode.setState(cAddr, state);
	}
}
