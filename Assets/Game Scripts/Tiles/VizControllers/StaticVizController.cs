using UnityEngine;
using System.Collections;

public class StaticVizController : TileVizController {

	GameObject[] verticals;

	// Use this for initialization
	public override void Start () {
		base.Start ();


	}

	public override void InitializeViz() {
		if (!m_initialized) {
			base.InitializeViz ();

			verticals = new GameObject[m_verticalElements.transform.childCount];
			for (int i=0; i<m_verticalElements.transform.childCount; i++){
				verticals [i] = m_verticalElements.transform.GetChild (i).gameObject;
			}

			int numRocks = Random.Range (0, verticals.Length - 1);
			for (int i = 0; i < numRocks; i++) {
				Vector2 circle = Random.insideUnitCircle;
				Vector3 XYcircle = new Vector3 (circle.x, 0, circle.y);
				GameObject newRock = Instantiate (verticals [Random.Range (0, verticals.Length - 1)], m_verticalElements.transform.position + XYcircle, Quaternion.identity) as GameObject;
				newRock.transform.parent = m_verticalElements.transform;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void UpdateViz(float growth) {
		return;
	}
}
