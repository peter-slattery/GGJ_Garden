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

	public float m_variationRadius = .2f;

	// Use this for initialization
	void Start () {
		RandomizeVerticalElements ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void EditorUpdateElement (bool vertElem) {
		if (vertElem) {
			RandomizeVerticalElements ();
		}
	}

	bool FindVerticalElement () {
		for (int i = 0; i < transform.childCount; i++) {
			GameObject child = transform.GetChild (i).gameObject;
			if (child.name == "vertical_elements") {
				m_verticalElements = child;
			}
		}

		if (m_verticalElements == null) {
			return false;
		}
		return true;
	}

	// NOTE: Override in subclasses
	void RandomizeVerticalElements () {
		if (m_verticalElements == null) {
			if (!FindVerticalElement ()) {
				Debug.Log ("Randomize Vertical Elements: No Vertical Elements");
				return;
			}
		}

		for (int i = 0; i < m_verticalElements.transform.childCount; i++) {
			Vector2 circle = Random.insideUnitCircle;
			Vector3 XYcircle = new Vector3 (circle.x, 0, circle.y);
			m_verticalElements.transform.GetChild (i).Translate (XYcircle * m_variationRadius);
		}
	}
}
