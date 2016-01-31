using UnityEngine;
using System.Collections;

public class TileTypeController : MonoBehaviour {

    public enum TileType
    {
        TILE_BLANK,
        TILE_TILLED,
        TILE_WEEDS,
        TILE_VINE,
        TILE_FLOWERS,
        TILE_TREE,
        TILE_ROCK,
		TILE_EMPTY, // For Testing And Generation Purposes Only. Will Never show up in game
		TILE_RANDOM,
    };

	private TileSingleton m_tileSingle;

    public TileType m_tileType = TileType.TILE_BLANK;
	public float m_tileGrowth = 0.0f;

	private TileVizController m_vizController;

	// Use this for initialization
	void Start () {
		CreateVisualizationForType ();

		// RegisterTile( 2D position, Tile (byte) type, float Growth Level, TileVizController this)
		Vector2 pos = new Vector2(transform.position.x, transform.position.z);

		GridController.getCurInstance ().RegisterTile (pos, m_tileType, m_tileGrowth, this);
	}
	
	// Update is called once per frame
	void Update () {
		int[] test = { 0 };
		UpdateTileState (m_tileType, m_tileGrowth, test);
	}

	public void UpdateTileState(TileType newType, float newGrowth, int[] direction ){
		if (newType != m_tileType) {
			m_tileType = newType;
			CreateVisualizationForType ();
		}

		if (m_vizController == null) {
			m_vizController = transform.GetChild (0).GetComponent<TileVizController> ();
		}

		if (true || newGrowth != m_tileGrowth) {
			// Update the Visualization To Reflect New Growth
			m_tileGrowth = newGrowth;
			m_vizController.UpdateViz (newGrowth);
		}

		if (m_tileType == TileType.TILE_VINE && direction[0] != 0) {
			VineTileController vineTC = m_vizController as VineTileController;
			vineTC.SetGrowthDirections (direction);
		}
	}

	public void EditorUpdateElement (bool vertElem) {
		if (m_vizController == null) {
			CreateVisualizationForType ();
		}
		if (m_vizController != null) {
			m_vizController.RandomizeVerticalElements ();
		}
	}

	void CreateVisualizationForType(){

		EraseDefaultViz ();

		if (m_tileSingle == null) {
			m_tileSingle = FindObjectOfType (typeof(TileSingleton)) as TileSingleton;
		}

		if (m_tileType == TileType.TILE_RANDOM) {
			m_tileType = (TileType)Random.Range (0, 6);
			CreateVisualizationForType ();
			return;
		}

		GameObject prefab = Instantiate (m_tileSingle.GetPrefabOfType (m_tileType), transform.position, Quaternion.identity) as GameObject;
		prefab.transform.parent = gameObject.transform;

		switch (m_tileType) {
		case TileType.TILE_BLANK:
			break;
		case TileType.TILE_FLOWERS:
			break;
		case TileType.TILE_ROCK:
			break;
		case TileType.TILE_TILLED:
			break;
		case TileType.TILE_TREE:
			break;
		case TileType.TILE_VINE:
			m_vizController = prefab.AddComponent<VineTileController> () as VineTileController;
			break;
		case TileType.TILE_WEEDS:
			
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
