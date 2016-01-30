using UnityEngine;
using System.Collections;

public class TileTypeController : MonoBehaviour {

	public float m_variationRadius = .2f;

	private GameObject m_verticalElements;

	// Use this for initialization
	public virtual void Start () {
		FindVerticalElement ();
	}
	
	// Update is called once per frame
	void Update () {
	
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
	public void RandomizeVerticalElements () {
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
