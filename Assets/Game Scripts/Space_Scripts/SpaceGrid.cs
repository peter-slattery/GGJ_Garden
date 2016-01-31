﻿using UnityEngine;
using System.Collections.Generic;

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
		int numUpdates = (int) (updateDiff.TotalSeconds/Config.TIME_UNIT_STANDARD);
		for (int i = 0; i < numUpdates; i++) {
			rootNode.updateProperties ();
		}
	}
	
	public void setState (CanAddr cAddr, TileTypeController.TileType tileType, float growthLevel) {
		if (rootNode == null) { rootNode = new Node(); }
		rootNode.setState(cAddr, tileType, growthLevel);
	}

	public void RegisterTile (Vector3 worldPos, TileTypeController.TileType tileType, float growthLevel, TileTypeController tileController) {
		CanAddr cAddr = GridController.WorldToGrid (worldPos);
		this.setState (cAddr, tileType, growthLevel);
		this.rootNode.RegisterController (cAddr, tileController);
	}

	public List<Tile> QueryForTileType (TileTypeController.TileType tileType) {
		List<Tile> result = new List<Tile> ();
		if (rootNode != null) {
			rootNode.QueryForTileType (result, tileType);
		}
		return result;
	}
}
