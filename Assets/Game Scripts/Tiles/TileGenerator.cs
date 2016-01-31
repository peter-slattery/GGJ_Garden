using UnityEngine;
using System.Collections;

public class TileGenerator :  MonoBehaviour{
	public static float tile_diameter = 1.73f;
	public static float tile_radius {
		get {
			return tile_diameter / 2.0f;
		}
	}

	public int patch_width = 5;
	public int patch_height = 5;

	public TileTypeController.TileType patch_type;

	public void GeneratePatch() {

	}
}
