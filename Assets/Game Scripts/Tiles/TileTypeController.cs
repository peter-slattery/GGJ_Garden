using UnityEngine;
using System.Collections;

public class TileTypeController : MonoBehaviour {

    public enum TileVizType
    {
        TILE_BLANK,
        TILE_TILLED,
        TILE_WEEDS,
        TILE_VINE,
        TILE_FLOWERS,
        TILE_TREE,
        TILE_ROCK,
		TILE_EMPTY, // For Testing And Generation Purposes Only. Will Never show up in game
    };

	private TileSingleton m_tileSingle;

    public TileVizType m_tileType = TileVizType.TILE_BLANK;
	public float m_tileGrowth = 0.0f;

	private TileVizController m_typeController;

	// Use this for initialization
	void Start () {
		CreateVisualizationForType ();

		// RegisterTile( 2D position, Tile (byte) type, float Growth Level, TileVizController this)
		// GridController.getCurInstance().RegisterTile();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void UpdateTileState(TileVizType newType, float newGrowth, int[] direction ){
		if (newType != m_tileType) {
			// Delete Child Prefab
			// Instantiate Corret Child Prefab
		}

		if (newGrowth != m_tileGrowth) {
			// Update the Visualization To Reflect New Growth
		}

		if (m_tileType == TileVizType.TILE_VINE && direction[0] != 0) {
			VineTileController vineTC = m_typeController as VineTileController;
			vineTC.SetGrowthDirections (direction);
		}
	}

	public void EditorUpdateElement (bool vertElem) {
		if (m_typeController == null) {
			CreateVisualizationForType ();
		}
		if (m_typeController != null) {
			m_typeController.RandomizeVerticalElements ();
		}
	}

	void CreateVisualizationForType(){

		EraseDefaultViz ();

		if (m_tileSingle == null) {
			m_tileSingle = FindObjectOfType (typeof(TileSingleton)) as TileSingleton;
		}
		GameObject prefab = Instantiate (m_tileSingle.GetPrefabOfType (m_tileType), transform.position, Quaternion.identity) as GameObject;
		prefab.transform.parent = gameObject.transform;

		switch (m_tileType) {
		case TileVizType.TILE_BLANK:
			break;
		case TileVizType.TILE_FLOWERS:
			break;
		case TileVizType.TILE_ROCK:
			break;
		case TileVizType.TILE_TILLED:
			break;
		case TileVizType.TILE_TREE:
			break;
		case TileVizType.TILE_VINE:
			m_typeController = prefab.AddComponent<VineTileController> () as VineTileController;
			break;
		case TileVizType.TILE_WEEDS:
			
			break;
		default:	// TILE_BLANK
			break;
		}
	}

	void EraseDefaultViz() {
		for (int i = 0; i < transform.childCount; i++) {
			if (!Application.isPlaying) {
				DestroyImmediate (transform.GetChild (i).gameObject);
			} else {
				Destroy (transform.GetChild (i).gameObject);
			}
		}
	}
}
