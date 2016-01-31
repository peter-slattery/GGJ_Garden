using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Vine {

	public const int MAX_CHILDREN = 6;

	public Vine[] children 		= new Vine[MAX_CHILDREN];
	public int numChildren 		= 0;
	public Tile gridLocation 	= null;

	public int[] dirs = new int[6];

	public Vine (Tile tile, int numNewChildren) {
		// TODO: 
	}

	private void addChild () {
		// TODO: 
	}
}
