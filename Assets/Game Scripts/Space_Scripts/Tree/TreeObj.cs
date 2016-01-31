using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public abstract class TreeObj {

	protected TreeObj prnt;
	protected CanAddr addr;
	protected int aggLev;

	public CanAddr Address
	{
		get { return this.addr; }
	}

	public TreeObj Parent
	{
		get { return this.prnt; }
	}

	public int AggregateLevel
	{
		get { return this.aggLev; }
	}

	public abstract Tile getTile (CanAddr cAddr);

	public abstract void updateState ();
	public abstract void updateProperties ();

	public abstract void setState (CanAddr cAddr, TileTypeController.TileType tileType, float growthLevel);
	public abstract void RegisterController (CanAddr cAddr, TileTypeController tTCont);

	public abstract void QueryForTileType (List<Tile> workingList, TileTypeController.TileType tileType);
}
