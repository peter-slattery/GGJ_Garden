using UnityEngine;
using System.Collections;

public class TileVizController : MonoBehaviour {

    public enum TileVizType
    {
        TILE_BLANK,
        TILE_TILLED,
        TILE_WEEDS,
        TILE_VINE,
        TILE_FLOWERS,
        TILE_TREE,
        TILE_ROCK,
    };

    public TileVizType m_tileType = TileVizType.TILE_BLANK;
	private TileSingleton m_tileSingle;

	private TileTypeController m_typeController;

	// Use this for initialization
	void Start () {
		CreateControllerForType ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void EditorUpdateElement (bool vertElem) {
		if (m_typeController != null) {
			m_typeController.RandomizeVerticalElements ();
		}
	}

	void CreateControllerForType(){
		switch (m_tileType) {
		case TileVizType.TILE_FLOWERS:
			break;
		case TileVizType.TILE_ROCK:
			break;
		case TileVizType.TILE_TILLED:
			break;
		case TileVizType.TILE_TREE:
			break;
		case TileVizType.TILE_VINE:
			m_typeController = gameObject.AddComponent<VineTileController> () as VineTileController;
			break;
		case TileVizType.TILE_WEEDS:
			
			break;
		default:	// TILE_BLANK
			break;
		}
	}
}
