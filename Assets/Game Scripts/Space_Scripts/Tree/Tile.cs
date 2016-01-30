using UnityEngine;
using System.Threading;
using System.Collections;

[System.Serializable]
public class Tile : TreeObj {

	/* ******************
	 * 	STATIC MEMBERS
	 * ******************/

	public static readonly byte BLANK_MASK 				= 0x01;
	public static readonly byte TILL_MASK 				= 0x02;
	public static readonly byte WEEDS_MASK	 			= 0x04;
	public static readonly byte VINE_MASK				= 0x08;
	public static readonly byte FLOWERS_MASK			= 0x10;
	public static readonly byte TREE_MASK				= 0x20;
	public static readonly byte ROCK_MASK				= 0x40;

	public static readonly float BLANK_GROWTH_RATE		= 0.2f;
	public static readonly float TILL_GROWTH_RATE		= 0.0f;
	public static readonly float WEEDS_GROWTH_RATE		= 0.4f;
	public static readonly float VINE_GROWTH_RATE		= 0.5f;
	public static readonly float FLOWERS_GROWTH_RATE	= 0.2f;
	public static readonly float TREE_GROTH_RATE		= 0.1f;
	public static readonly float ROCK_GROWTH_RATE		= 0.0f;

	public static readonly float GROWTH_MIN 			= 0.0f;
	public static readonly float GROWTH_MAX 			= 10.0f;
	public static readonly float GROWTH_GEN_PARAM 		= 1.0f;

	public static readonly int NUM_GROWTH_LEVELS		= 4;

	public static int NUM_NEIGHBORS = 12;
	
	private static LatAddr getNeighborLatOffset (int nIndex) {
		switch (nIndex) {
		case 0:
			return new LatAddr(1,0,0);
		case 1:
			return new LatAddr(0,1,0);
		case 2:
			return new LatAddr(1,1,0);
		case 3:
			return new LatAddr(0,0,1);
		case 4:
			return new LatAddr(1,0,1);
		case 5:
			return new LatAddr(0,1,1);
		case 6:
			return new LatAddr(2,1,0);
		case 7:
			return new LatAddr(1,2,0);
		case 8:
			return new LatAddr(0,2,1);
		case 9:
			return new LatAddr(0,1,2);
		case 10:
			return new LatAddr(1,0,2);
		case 11:
			return new LatAddr(2,0,1);
		default:
			Debug.LogError ("Invalid Neighbor Index");
			return null;
		}
	}

	/* ******************
	 * 	TREEOBJ MEMBERS
	 * ******************/

	public override Tile getTile (CanAddr cAddr) {
		return this;
	}
		
	// TODO: The contents of these two methods are temporary, and should be replaced when you have a better understanding of the fire simulation
	public override void updateState () {
		byte prevStatus = this.state;

		if (prevStatus != this.state) {
			this.prnt.notifyStateChange(this.addr.getTuple(this.aggLev), this.state);
		}
	}

	public override void updateProperties () {
		this.updateGrowth ();
		Debug.Log ("TS: " + GridController.getCurInstance ().updateDiff.TotalSeconds);
		for (int i = 0; i < Tile.NUM_NEIGHBORS; i++) {
			this.AffectNeighbor (i);
		}
	}

	public override void inheritState (byte state) {
		this.state = state;
	}

	public override void setState (CanAddr cAddr, byte state) {
		this.state = state;
		this.prnt.notifyStateChange(this.addr.getTuple(this.aggLev), this.state);
	}

	// Both of these methods should be NO-OPS.
	// Tiles won't have children who report state change to them.
	public override void notifyStateChange (int childIndex, byte childStatus) {}
	public override void removalRequest (int childIndex) {}
	
	public void notifyReferenceAddition (int index) {
		this.inRef = (ushort) (this.inRef | (0x01 << index));
	}

	public void notifyReferenceRemoval (int index) {
		this.inRef = (ushort) (this.inRef & ~(0x01 << index));

		if (this.inRef == 0 && !this.isBlank()) {
			this.prnt.removalRequest(this.addr.getTuple(this.aggLev));
		}
	}

	/* ******************
	 * 	TILE SPECIFIC
	 * ******************/

	Tile[] outRef	= new Tile[NUM_NEIGHBORS];
	ushort inRef = 0x0000;

	public byte state = 0x0;
	public byte growth = 0x0;

	public Tile (TreeObj prnt, int nextTuple) {
		this.prnt = prnt;
		this.addr = new CanAddr(prnt.Address);
		this.aggLev = prnt.AggregateLevel - 1;
		this.addr.setTuple((byte) nextTuple, this.aggLev);
	}

	public bool isBlank () {
		return (this.state & Tile.BLANK_MASK) != 0;
	}

	public bool isTilled () {
		return (this.state & Tile.TILL_MASK) != 0;
	}

	public bool hasWeeds () {
		return (this.state & Tile.WEEDS_MASK) != 0;
	}

	public bool hasVine () {
		return (this.state & Tile.VINE_MASK) != 0;
	}

	public bool hasFlowers () {
		return (this.state & Tile.FLOWERS_MASK) != 0;
	}

	public bool hasTree () {
		return (this.state & Tile.TREE_MASK) != 0;
	}

	public bool isRock () {
		return (this.state & Tile.ROCK_MASK) != 0;
	}

	private void updateGrowth () {

	}

	private void AffectNeighbor (int nIndex) {

	}
}
