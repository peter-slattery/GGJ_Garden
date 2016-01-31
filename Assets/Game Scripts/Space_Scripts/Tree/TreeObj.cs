using UnityEngine;
using System.Collections;

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

	public abstract void setState (CanAddr cAddr, byte tileType, float growthLevel, TileTypeController tTController);
}
