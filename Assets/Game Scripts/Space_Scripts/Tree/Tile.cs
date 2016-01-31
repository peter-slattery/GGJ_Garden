﻿using UnityEngine;
using System.Threading;
using System.Collections.Generic;

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

	public static int NUM_NEIGHBORS = 6;
	
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

	public override void updateState () {
		int prevGrowthState = this.growthState;
		this.growthState = Tile.getGrowthStateForLevel (this.growthLevel);

		if (prevGrowthState != this.growthState) {
			if (Random.value <= (this.growthLevel / GROWTH_MAX)) {
				this.specialAction ();
			}
				
			if (this.tileCont != null) {
				int [] dir = { 0 };
				this.tileCont.UpdateTileState(this.tileType, this.growthLevel, dir);
			}
		}
	}

	public override void updateProperties () {
		this.updateGrowth ();
		this.AffectNeighbors ();
	}

	public override void setState (CanAddr cAddr, TileTypeController.TileType tileType, float growthLevel) {
		if (tileType != TileTypeController.TileType.TILE_VINE && this.tileType == TileTypeController.TileType.TILE_VINE) {
			this.tileType = tileType;
			this.presentVine.killChildren();
		}

		if (this.tileType != TileTypeController.TileType.TILE_VINE && this.tileType == TileTypeController.TileType.TILE_VINE) {
			this.presentVine = new Vine ();
			this.presentVine.owningTile = this;
		}

		this.tileType = tileType;
		this.growthLevel = growthLevel;

		this.growthState = Tile.getGrowthStateForLevel (this.growthLevel);
		this.fillOutRef ();

		if (this.tileCont != null) {
			if (this.tileType == TileTypeController.TileType.TILE_VINE) {
				this.tileCont.UpdateTileState(this.tileType, this.growthLevel, this.presentVine.dirs);
			}
			int[] dir = { 0 };
			this.tileCont.UpdateTileState(this.tileType, this.growthLevel, dir);
		}
	}

	public override void RegisterController (CanAddr cAddr, TileTypeController tTCont) {
		this.tileCont = tTCont;
	}

	public override void QueryForTileType (List<Tile> workingList, TileTypeController.TileType tileType) {
		if (this.tileType == tileType) {
			workingList.Add (this);
		}
	}

	/* ******************
	 * 	TILE SPECIFIC
	 * ******************/

	Tile[] outRef	= new Tile[NUM_NEIGHBORS];

	public TileTypeController.TileType	tileType	= TileTypeController.TileType.TILE_ROCK;
	public float						growthLevel	= 0x0;
	public int							growthState	= 0;

	public TileTypeController tileCont = null;

	// NOTE: This field is for Tiles with Vine Type
	public Vine presentVine = null;

	public Tile (TreeObj prnt, int nextTuple) {
		this.prnt = prnt;
		this.addr = new CanAddr(prnt.Address);
		this.aggLev = prnt.AggregateLevel - 1;
		this.addr.setTuple((byte) nextTuple, this.aggLev);

		this.tileType = TileTypeController.TileType.TILE_ROCK;
		this.growthState = 0;
		this.fillOutRef ();
	}

	public bool isBlank () {
		return (this.tileType == TileTypeController.TileType.TILE_BLANK);
	}

	public bool isTilled () {
		return (this.tileType == TileTypeController.TileType.TILE_TILLED);
	}

	public bool hasWeeds () {
		return (this.tileType == TileTypeController.TileType.TILE_WEEDS);
	}

	public bool hasVine () {
		return (this.tileType == TileTypeController.TileType.TILE_VINE);
	}

	public bool hasFlowers () {
		return (this.tileType == TileTypeController.TileType.TILE_FLOWERS);
	}

	public bool hasTree () {
		return (this.tileType == TileTypeController.TileType.TILE_TREE);
	}

	public bool isRock () {
		return (this.tileType == TileTypeController.TileType.TILE_ROCK);
	}

	private void updateGrowth () {
		float timeStep = (float) GridController.getCurInstance ().updateDiff.TotalSeconds / Config.TIME_UNIT_STANDARD;
		this.growthLevel = Mathf.Clamp (this.growthLevel + (Tile.TILE_GROWTH_RATES [(int) this.tileType] * timeStep), Tile.GROWTH_MIN, Tile.GROWTH_MAX);
	}

	private void AffectNeighbors () {
		float timeStep = (float) GridController.getCurInstance ().updateDiff.TotalSeconds / Config.TIME_UNIT_STANDARD;
		for (int i = 0; i < Tile.NUM_NEIGHBORS; i++) {
			if (this.outRef [i] != null) {
				this.outRef [i].growthLevel = Mathf.Clamp(this.outRef[i].growthLevel + (Tile.INTER_TILE_EFFECTS [(int) this.tileType, (int) this.outRef [i].tileType] * timeStep), Tile.GROWTH_MIN, Tile.GROWTH_MAX);
			}
		}
	}

	private void fillOutRef () {
		// TODO: If I am Rock, Tilled, Or Outside (?) do not make outRefs
		if (this.tileType != TileTypeController.TileType.TILE_ROCK && this.tileType != TileTypeController.TileType.TILE_TILLED) {
			for (int i = 0; i < Tile.NUM_NEIGHBORS; i++) {
				LatAddr lAdd = CanAddr.convertCanAddrToLatAddr (this.addr);
				lAdd.addLatAddr (Tile.getNeighborLatOffset (i));
				this.outRef [i] = GridController.getCurInstance ().getTile (CanAddr.convertLatAddrToCanAddr (lAdd));
			}
		}
	}

	// NOTE: This function does the 'special action' which is determined by tile type
	private void specialAction () {
		switch (this.tileType) {
		// NOTE: If adjacent Tilled Tile, spread
		case TileTypeController.TileType.TILE_BLANK:
			List<Tile> TilledNeighbors = new List<Tile> ();
			for (int i = 0; i < NUM_NEIGHBORS; i++) {
				if (this.outRef [i].tileType == TileTypeController.TileType.TILE_TILLED) {
					TilledNeighbors.Add (this.outRef [i]);
				}
			}
			// NOTE: Tile doesn't use the CanAddr param, so it can be null
			if (TilledNeighbors.Count != 0) {
				TilledNeighbors [Random.Range (0, TilledNeighbors.Count)].setState (null, TileTypeController.TileType.TILE_BLANK, 0f);
			}
			break;
		// NOTE: If adjacent Blank Tile, spread
		case TileTypeController.TileType.TILE_FLOWERS:
			if (Random.value > 0.6f) {
				List<Tile> validNeighbors = new List<Tile> ();
				for (int i = 0; i < NUM_NEIGHBORS; i++) {
					if (this.outRef [i].tileType == TileTypeController.TileType.TILE_TILLED || this.outRef[i].tileType == TileTypeController.TileType.TILE_BLANK) {
						validNeighbors.Add (this.outRef [i]);
					}
				}
				validNeighbors [Random.Range (0, validNeighbors.Count)].setState (null, TileTypeController.TileType.TILE_FLOWERS, 0f);
			}
			break;
		// NOTE: Randomly seed new location
		case TileTypeController.TileType.TILE_TREE:
			if (Random.value > 0.6f) {
				// TODO: Pick random tile (that is active) and make it have a 0 growth tree
			}
			break;
		// NOTE: Chance to turn to Vine root
		case TileTypeController.TileType.TILE_WEEDS:
			if (Random.value > 0.7f) {
				this.setState (null, TileTypeController.TileType.TILE_VINE, (GROWTH_MAX - GROWTH_MIN) / 2);
				// TODO: Add some children of the Vine
			}
			break;
		// NOTE: Create adjacent connected vine
		case TileTypeController.TileType.TILE_VINE:
			if (Random.value > 0.6f) {
				List<Tile> validNeighbors = new List<Tile> ();
				for (int i = 0; i < NUM_NEIGHBORS; i++) {
					if (this.outRef [i].tileType != TileTypeController.TileType.TILE_VINE && 
						this.outRef [i].tileType != TileTypeController.TileType.TILE_ROCK ) {
						validNeighbors.Add (this.outRef [i]);
					}
				}

				bool hasNew = false;
				while (!hasNew) {
					int candidateNeighborIndex = Random.Range (0, validNeighbors.Count);
					if (validNeighbors [candidateNeighborIndex].tileType == TileTypeController.TileType.TILE_FLOWERS ||
					    validNeighbors [candidateNeighborIndex].tileType == TileTypeController.TileType.TILE_TREE) {
						if (validNeighbors [candidateNeighborIndex].growthLevel < (GROWTH_MAX / 2f)) {
							GrowVineInto (validNeighbors [candidateNeighborIndex]);
							hasNew = true;
						}
					} else {
						GrowVineInto (validNeighbors [candidateNeighborIndex]);
						hasNew = true;
					}
				}
			}
			break;
		}
	}

	private void GrowVineInto (Tile tileToGrow) {
		LatAddr dest = CanAddr.convertCanAddrToLatAddr (tileToGrow.addr);
		LatAddr src = CanAddr.convertCanAddrToLatAddr (this.addr);

		src.scaleLatAddr (-1);
		dest.addLatAddr (src);

		CanAddr disp = CanAddr.convertLatAddrToCanAddr (dest);

		tileToGrow.setState (null, TileTypeController.TileType.TILE_VINE, 0f);

		if (this.presentVine != null) {
			this.presentVine.dirs [this.presentVine.curDirs++] = disp.getTuple (0);
			this.presentVine.children [disp.getTuple (0)] = tileToGrow.presentVine;
		}

		tileToGrow.presentVine.dirs [tileToGrow.presentVine.curDirs++] = (int) CanAddr.TUPLE_MASK & ~(disp.getTuple(0));

		this.tileCont.UpdateTileState (this.tileType, this.growthLevel, this.presentVine.dirs);
		tileToGrow.tileCont.UpdateTileState (tileToGrow.tileType, tileToGrow.growthLevel, tileToGrow.presentVine.dirs);
	}
}
