using UnityEngine;
using System.Threading;
using System.Collections;

[System.Serializable]
public class Tile : TreeObj {

	/* ******************
	 * 	STATIC MEMBERS
	 * ******************/

	// NOTE: MIN and MAX determine range for valid growth values
	public static readonly float GROWTH_MIN 			= 0.0f;
	public static readonly float GROWTH_MAX 			= 10.0f;

	// NOTE: Growth levels are stored by the growthState variable and range from [0, NUM_GROWTH_LEVELS-1]
	public static readonly int NUM_GROWTH_LEVELS		= 4;

	public static readonly float TIME_UNIT_STANDARD		= 10.0f;

	public static readonly ushort BLANK_INDEX	= 0;
	public static readonly ushort TILLED_INDEX	= 1;
	public static readonly ushort WEEDS_INDEX	= 2;
	public static readonly ushort VINE_INDEX	= 3;
	public static readonly ushort FLOWERS_INDEX	= 4;
	public static readonly ushort TREE_INDEX	= 5;
	public static readonly ushort ROCK_INDEX	= 6;

	public const byte BLANK_MASK 	= 0x01;
	public const byte TILL_MASK 	= 0x02;
	public const byte WEEDS_MASK	= 0x04;
	public const byte VINE_MASK 	= 0x08;
	public const byte FLOWERS_MASK	= 0x10;
	public const byte TREE_MASK 	= 0x20;
	public const byte ROCK_MASK		= 0x40;

	// NOTE: This array defines the growth rates of the different tile types
	//  Order is as follows:
	//  BLANK, TILLED, WEEDS, VINE, FLOWERS, TREE, ROCK
	public static readonly float[] TILE_GROWTH_RATES = {
		0.4f, 0.0f, 0.6f, 0.7f, 0.4f, 0.3f, 0.0f
	};

	// NOTE: This matrix defines the effects that plants have on their neighbors.
	//  These entries are added to the 
	//  The structure of this matrix is as follows:
	//   [effector, effectee]
	//  The ordering is as follows:
	//   Blank, Tilled, Weeds, Vine, Flower, Tree, rock
	public static readonly float[,] INTER_TILE_EFFECTS	= {
		{  0.2f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f },
		{  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f },
		{  0.0f,  0.0f,  0.2f,  0.1f, -0.1f, -0.1f,  0.0f },
		{ -0.2f,  0.0f,  0.1f,  0.2f, -0.2f, -0.4f,  0.0f },
		{ -0.1f,  0.0f, -0.2f, -0.2f,  0.4f,  0.4f,  0.0f },
		{ -0.1f,  0.0f, -0.1f, -0.1f, -0.1f, -0.1f,  0.0f },
		{  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f }
	};

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

	private static ushort typeIndexForState (byte state) {
		switch (state) {
		case BLANK_MASK:
			return 0;
		case TILL_MASK:
			return 1;
		case WEEDS_MASK:
			return 2;
		case VINE_MASK:
			return 3;
		case FLOWERS_MASK:
			return 4;
		case TREE_MASK:
			return 5;
		case ROCK_MASK:
			return 6;
		default: 
			Debug.LogError ("Invalid tile type state");
			return 0;
		}
	}

	/* ******************
	 * 	TREEOBJ MEMBERS
	 * ******************/

	public override Tile getTile (CanAddr cAddr) {
		return this;
	}

	// TODO: This function will need to have things added for the different plant behaviors (spread, seed, etc.)
	public override void updateState () {
		int prevGrowthState = this.growthState;

		this.growthState = (int)((Tile.GROWTH_MAX - Tile.GROWTH_MIN) / this.growth);

		if (prevGrowthState != this.growthState) {
			// NOTE: This line requires that the ordering of the TileTypeController.TileVizType matches the Tile type indices at the top of this file
			TileTypeController.TileVizType curState = (TileTypeController.TileVizType) this.typeIndex;
			int [] dir = [0];
			this.tileCont.UpdateTileState(curState, this.growth, dir);
		}
	}

	public override void updateProperties () {
		this.updateGrowth ();
		this.AffectNeighbors ();
	}

	public override void setState (CanAddr cAddr, byte tileType, float growthLevel, TileTypeController tTController) {
		this.tileType = tileType;
		this.typeIndex = Tile.typeIndexForState (this.tileType);
		this.growthState = growthState;
		this.tileCont = tTController;
	}

	/* ******************
	 * 	TILE SPECIFIC
	 * ******************/

	Tile[] outRef	= new Tile[NUM_NEIGHBORS];

	public byte		tileType	= 0x0;
	public ushort	typeIndex	= 0;
	public float	growth		= 0x0;
	public int		growthState	= 0;

	public TileTypeController tileCont = null;

	public Tile (TreeObj prnt, int nextTuple) {
		this.prnt = prnt;
		this.addr = new CanAddr(prnt.Address);
		this.aggLev = prnt.AggregateLevel - 1;
		this.addr.setTuple((byte) nextTuple, this.aggLev);
	}

	public bool isBlank () {
		return (this.tileType & Tile.BLANK_MASK) != 0;
	}

	public bool isTilled () {
		return (this.tileType & Tile.TILL_MASK) != 0;
	}

	public bool hasWeeds () {
		return (this.tileType & Tile.WEEDS_MASK) != 0;
	}

	public bool hasVine () {
		return (this.tileType & Tile.VINE_MASK) != 0;
	}

	public bool hasFlowers () {
		return (this.tileType & Tile.FLOWERS_MASK) != 0;
	}

	public bool hasTree () {
		return (this.tileType & Tile.TREE_MASK) != 0;
	}

	public bool isRock () {
		return (this.tileType & Tile.ROCK_MASK) != 0;
	}

	private void updateGrowth () {
		float timeStep = (float) GridController.getCurInstance ().updateDiff.TotalSeconds / Tile.TIME_UNIT_STANDARD;
		this.growth += Tile.TILE_GROWTH_RATES [this.typeIndex] * timeStep;
	}

	private void AffectNeighbors () {
		float timeStep = (float) GridController.getCurInstance ().updateDiff.TotalSeconds / Tile.TIME_UNIT_STANDARD;
		for (int i = 0; i < Tile.NUM_NEIGHBORS; i++) {
			this.outRef [i].growth += Tile.INTER_TILE_EFFECTS [this.typeIndex, this.outRef [i].typeIndex] * timeStep;
		}
	}
}
