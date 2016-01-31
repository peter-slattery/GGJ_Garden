using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Node : TreeObj {

	/* ******************
	 * 	STATIC MEMBERS
	 * ******************/

	public static int NUM_CHILDREN = 7;

	/* ******************
	 * 	TREEOBJ MEMBERS
	 * ******************/
	
	public override Tile getTile (CanAddr cAddr) {
		int childIndex = (int) cAddr.getTuple(this.aggLev - 1);

		if (children[childIndex] == null) {
			if (this.aggLev == 1) {
				children[childIndex] = new Tile (this, childIndex);
			}
			else { children[childIndex] = new Node(this, childIndex); }
		}
		return children[childIndex].getTile(cAddr);
	}
		
	public override void updateState () {
		for (int i = 0; i < NUM_CHILDREN; i++) {
			if (children[i] != null) {
				children[i].updateState();
			}
		}
	}

	public override void updateProperties () {
		for (int i = 0; i < NUM_CHILDREN; i++) {
			if (children[i] != null) { 
				children[i].updateProperties();
			}
		}
	}

	public override void setState (CanAddr cAddr, TileTypeController.TileType tileType, float growthLevel) {
		int childIndex = (int) cAddr.getTuple(this.aggLev - 1);

		if (children[childIndex] == null) {
			if (this.aggLev == 1) {
				this.children[childIndex] = new Tile (this, childIndex);
				this.children[childIndex].setState(cAddr, tileType, growthLevel);
			}
			else {
				this.children[childIndex] = new Node (this, childIndex);
				this.children[childIndex].setState(cAddr, tileType, growthLevel);
			}
		}
		else {
			children[childIndex].setState(cAddr, tileType, growthLevel);
		}
	}

	public override void RegisterController (CanAddr cAddr, TileTypeController tTCont) {
		int childIndex = (int) cAddr.getTuple(this.aggLev - 1);

		if (children[childIndex] == null) {
			if (this.aggLev == 1) {
				this.children[childIndex] = new Tile (this, childIndex);
				this.children[childIndex].RegisterController(cAddr, tTCont);
			}
			else {
				this.children[childIndex] = new Node (this, childIndex);
				this.children[childIndex].RegisterController(cAddr, tTCont);
			}
		}
		else {
			children[childIndex].RegisterController(cAddr, tTCont);
		}
	}

	public override void QueryForTileType (List<Tile> workingList, TileTypeController.TileType tileType) {
		for (int i = 0; i < Node.NUM_CHILDREN; i++) {
			if (this.children [i] != null) {
				this.children [i].QueryForTileType (workingList, tileType);
			}
		}
	}

	public override void CreateVizForTile (TileSingleton tS) {
		for (int i = 0; i < Node.NUM_CHILDREN; i++) {
			if (this.children [i] != null) {
				this.children [i].CreateVizForTile (tS);
			}
		}
	}

	/* ******************
	 * 	NODE SPECIFIC
	 * ******************/

	private TreeObj[] children = new TreeObj[NUM_CHILDREN];
	
	public Node () {
		this.prnt = null;
		this.addr = new CanAddr();
		this.aggLev = Config.TREE_DEPTH;
	}
	
	public Node (TreeObj prnt, int nextTuple) {
		this.prnt = prnt;
		this.addr = new CanAddr(prnt.Address);
		this.aggLev = prnt.AggregateLevel - 1;
		this.addr.setTuple((byte) nextTuple, aggLev);
	}

	private bool livingChildrenCheck () {
		bool livingChildren = false;
		for (int i = 0; i < NUM_CHILDREN; i++) 		{ livingChildren = livingChildren | (this.children[i] != null); }
		return livingChildren;
	}
}
