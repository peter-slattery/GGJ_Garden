using UnityEngine;
using System.Collections;

public class TileTypeController : MonoBehaviour {

    public enum TileType
    {
        TILE_BLANK = 0,
        TILE_TILLED,
        TILE_WEEDS,
        TILE_VINE,
        TILE_FLOWERS,
        TILE_TREE,
        TILE_ROCK,
		TILE_EMPTY, // For Testing And Generation Purposes Only. Will Never show up in game
		TILE_RANDOM,
    };

	private bool m_initialized = false;

	private TileSingleton m_tileSingle;

    public TileType m_tileType = TileType.TILE_BLANK;
	public float m_tileGrowth = 0.0f;

	private GameObject m_tileVizPrefab;
	private TileVizController m_vizController;

	// Use this for initialization
	void Start () {
		InitializeTileController ();
	}
	
	// Update is called once per frame
	void Update () {
		int[] test = { 0 };
		UpdateTileState (m_tileType, m_tileGrowth, test);
	}

	public void InitializeTileController() {
		if (!m_initialized) {
			CreateVisualizationForType ();

			// RegisterTile(position, Tile (byte) type, float Growth Level, TileVizController this)

			GridController.getCurInstance ().RegisterTile (transform.position, m_tileType, m_tileGrowth, this);

			m_initialized = true;
		}
	}

	public void UpdateTileState(TileType newType, float newGrowth, int[] direction ){
		if (!m_initialized) {
			InitializeTileController ();
		}

		if (newType != m_tileType) {
			m_tileType = newType;
			CreateVisualizationForType ();
		}

		if (m_vizController == null) {
			m_vizController = transform.GetChild (0).GetComponent<TileVizController> ();
			if (m_tileType == TileType.TILE_VINE) {
				(m_vizController as VineTileController).InitializeViz ();
			}
		}

		if (newGrowth != m_tileGrowth) {
			// Update the Visualization To Reflect New Growth
			m_tileGrowth = newGrowth;
			m_vizController.UpdateViz (newGrowth);

			float mod = Random.Range (0.0f, 2.0f);
			if (m_tileGrowth + mod >= 8.0f) {
				if (m_tileType == TileType.TILE_FLOWERS) {
					Instantiate (Resources.Load ("Flower"), 
						transform.position,
						Quaternion.identity);
				} else if (m_tileType == TileType.TILE_TREE) {
					Instantiate (Resources.Load("Apple"),
						transform.position,
						Quaternion.identity);
				}
			}
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

		m_tileVizPrefab = Instantiate (m_tileSingle.GetPrefabOfType (m_tileType), transform.position, Quaternion.identity) as GameObject;
		m_tileVizPrefab.transform.parent = gameObject.transform;
		m_vizController = m_tileVizPrefab.GetComponent<TileVizController> ();
		m_vizController.InitializeViz ();
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
