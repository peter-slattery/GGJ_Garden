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
		OrientToCamera ();
	}
	
	// Update is called once per frame
	void Update () {
		// Will only be called in editor (Only time Update is called when !isPlaying
		if (!Application.isPlaying) {
			OrientToCamera ();
		}

	}

	void OrientToCamera() {
		if (m_tileSingle == null) {
			m_tileSingle = (TileSingleton)(FindObjectOfType (typeof(TileSingleton)));

			if (m_tileSingle == null) {
				Debug.Log ("OrientToCamera: No Tile Singleton Found");
				return;
			}
		}

		if (!m_tileSingle.m_shouldOrientVerticals) {
			return;
		}

		if (m_verticalElements == null) {
			for (int i = 0; i <transform.childCount; i++) {
				if (transform.GetChild (i).gameObject.name == "vertical_elements") {
					m_verticalElements = transform.GetChild (i).gameObject;
				}
			}
			if (m_verticalElements == null) {
				Debug.Log ("OrientToCamera: No Vertical Element");
				return;
			}
		}

		Vector3 camPos = Camera.main.transform.position;
		camPos.y = 0;
		m_verticalElements.transform.LookAt (camPos);
	}
}
