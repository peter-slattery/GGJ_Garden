using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Vine {

	public const int MAX_CHILDREN = 6;

	public Vine[] children 		= new Vine[MAX_CHILDREN];

	public int curDirs = 0;
	public int[] dirs = new int[6];
	public Tile owningTile = null;

	public void killChildren () {
		for (int i = 0; i < MAX_CHILDREN; i++) {
			if (this.children [i] != null) {
				this.children [i].killChildren ();
				this.children [i] = null;
			}
		}
		this.owningTile.setState (null, TileTypeController.TileType.TILE_WEEDS, (Tile.GROWTH_MAX / 2f));
	}
}
