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

	public static int getGrowthStateForLevel (float growthLevel) {
		return (int) (growthLevel / ((GROWTH_MAX - GROWTH_MIN) / NUM_GROWTH_LEVELS));
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
		this.growthState = Tile.getGrowthStateForLevel (this.growthLevel);

		if (prevGrowthState != this.growthState) {
			if (this.tileCont != null) {
				int [] dir = { 0 };
				this.tileCont.UpdateTileState(this.typeIndex, this.growthLevel, dir);
			}
		}
	}

	public override void updateProperties () {
		this.updateGrowth ();
		this.AffectNeighbors ();
	}

	public override void setState (CanAddr cAddr, TileTypeController.TileType tileType, float growthLevel) {
		this.typeIndex = tileType;
		this.growthLevel = growthLevel;

		this.growthState = (int)((Tile.GROWTH_MAX - Tile.GROWTH_MIN) / this.growthLevel);
		this.fillOutRef ();

		if (this.tileCont != null) {
			int [] dir = { 0 };
			this.tileCont.UpdateTileState(this.typeIndex, this.growthLevel, dir);
		}
	}

	public override void RegisterController (CanAddr cAddr, TileTypeController tTCont) {
		this.tileCont = tTCont;
	}

	/* ******************
	 * 	TILE SPECIFIC
	 * ******************/

	Tile[] outRef	= new Tile[NUM_NEIGHBORS];

	public TileTypeController.TileType	typeIndex	= TileTypeController.TileType.TILE_ROCK;
	public float						growthLevel	= 0x0;
	public int							growthState	= 0;

	public TileTypeController tileCont = null;

	public Tile (TreeObj prnt, int nextTuple) {
		this.prnt = prnt;
		this.addr = new CanAddr(prnt.Address);
		this.aggLev = prnt.AggregateLevel - 1;
		this.addr.setTuple((byte) nextTuple, this.aggLev);

		this.typeIndex = TileTypeController.TileType.TILE_ROCK;
		this.growthState = 0;
		this.fillOutRef ();
	}

	public bool isBlank () {
		return (this.typeIndex == TileTypeController.TileType.TILE_BLANK);
	}

	public bool isTilled () {
		return (this.typeIndex == TileTypeController.TileType.TILE_TILLED);
	}

	public bool hasWeeds () {
		return (this.typeIndex == TileTypeController.TileType.TILE_WEEDS);
	}

	public bool hasVine () {
		return (this.typeIndex == TileTypeController.TileType.TILE_VINE);
	}

	public bool hasFlowers () {
		return (this.typeIndex == TileTypeController.TileType.TILE_FLOWERS);
	}

	public bool hasTree () {
		return (this.typeIndex == TileTypeController.TileType.TILE_TREE);
	}

	public bool isRock () {
		return (this.typeIndex == TileTypeController.TileType.TILE_ROCK);
	}

	private void updateGrowth () {
		float timeStep = (float) GridController.getCurInstance ().updateDiff.TotalSeconds / Config.TIME_UNIT_STANDARD;
		this.growthLevel = Mathf.Clamp (this.growthLevel + (Tile.TILE_GROWTH_RATES [(int) this.typeIndex] * timeStep), Tile.GROWTH_MIN, Tile.GROWTH_MAX);
	}

	private void AffectNeighbors () {
		float timeStep = (float) GridController.getCurInstance ().updateDiff.TotalSeconds / Config.TIME_UNIT_STANDARD;
		for (int i = 0; i < Tile.NUM_NEIGHBORS; i++) {
			if (this.outRef [i] != null) {
				this.outRef [i].growthLevel = Mathf.Clamp(this.outRef[i].growthLevel + (Tile.INTER_TILE_EFFECTS [(int) this.typeIndex, (int) this.outRef [i].typeIndex] * timeStep), Tile.GROWTH_MIN, Tile.GROWTH_MAX);
			}
		}
	}

	private void fillOutRef () {
		// TODO: If I am Rock, Tilled, Or Outside (?) do not make outRefs
		if (this.typeIndex != TileTypeController.TileType.TILE_ROCK && this.typeIndex != TileTypeController.TileType.TILE_TILLED) {
			for (int i = 0; i < Tile.NUM_NEIGHBORS; i++) {
				LatAddr lAdd = CanAddr.convertCanAddrToLatAddr (this.addr);
				lAdd.addLatAddr (Tile.getNeighborLatOffset (i));
				this.outRef [i] = GridController.getCurInstance ().getTile (CanAddr.convertLatAddrToCanAddr (lAdd));
			}
		}
	}
}
