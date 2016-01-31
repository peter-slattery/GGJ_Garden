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
	
	public void setState (CanAddr cAddr, byte tileType, float growthLevel, TileTypeController tileController) {
		if (rootNode == null) { rootNode = new Node(); }
		rootNode.setState(cAddr, tileType, growthLevel, tileController);
	}

	public void RegisterTile (Vector2 worldPos, byte tileType, float growthLevel, TileTypeController tileController) {
		// TODO: convert worldPos to CanAddr and call setState
	}
}
