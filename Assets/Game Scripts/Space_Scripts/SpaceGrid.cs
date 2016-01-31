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
	
	public void setState (CanAddr cAddr, TileTypeController.TileType tileType, float growthLevel) {
		if (rootNode == null) { rootNode = new Node(); }
		rootNode.setState(cAddr, tileType, growthLevel);
	}

	public void RegisterTile (Vector2 worldPos, TileTypeController.TileType tileType, float growthLevel, TileTypeController tileController) {
		CanAddr cAddr = GridController.WorldToGrid (worldPos);
		this.setState (cAddr, tileType, growthLevel);
		this.rootNode.RegisterController (cAddr, tileController);
	}


}
