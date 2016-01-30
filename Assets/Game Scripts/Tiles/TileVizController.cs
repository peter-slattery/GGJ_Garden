using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
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

	private GameObject m_verticalElements;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


	}
}
