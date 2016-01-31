using UnityEngine;
using System.Collections;

public class VineTileController : TileVizController {

	private GameObject m_tendrilParent;
	private GameObject m_tendrilObject;

	private int[] growthDirections = { 0, 0, 0, 0, 0, 0 };

	// Use this for initialization
	public override void Start () {
		base.Start ();

		InitializeVineViz ();
	}

	void Awake () {

	}

	// Update is called once per frame
	void Update () {
	
	}

	public void InitializeVineViz () {
		// Find Tendril Parent
		if (m_tendrilObject == null) {
			for (int i = 0; i < transform.childCount; i++) {
				GameObject child = transform.GetChild (i).gameObject;
				if (child.name == "tendril_elements") {
					m_tendrilParent = child;
					break;
				}
			}
		}

		if (m_tendrilObject == null) {
			m_tendrilObject = m_tendrilParent.transform.GetChild (0).gameObject;
			m_tendrilObject.SetActive (false);
		}
	}

	public void SetGrowthDirections(int[] directions){
		if (directions.Length > 6) {
			Debug.Log ("SetGrowthDirections: Too Many Directions Provided. Using Only The First Six.");
		}

		for (int i = 0; i < directions.Length && i < 6; i++) {
			if (directions [i] == 0) {
				continue;
			}
			growthDirections [i] = directions [i];
			CreateTendrilToPosition (growthDirections [i]);
		}
	}

	void CreateTendrilToPosition(int position){
		float rot = 0;

		switch (position) {
		case 1:
			rot = -90;
			break;
		case 2:
			rot = 30;
			break;
		case 3:
			rot = -30;
			break;
		case 4: 
			rot = 150;
			break;
		case 5:
			rot = -150;
			break;
		case 6:
			rot = 90;
			break;
		}

		if (m_tendrilObject == null || m_tendrilParent == null) {
			InitializeVineViz ();	
		}

		if (m_tendrilObject == null) {
			Debug.Log ("Object missing");
		} 
		if (m_tendrilParent == null) {
			Debug.Log ("Parent Missing");
		}

		GameObject newTendril = Instantiate (m_tendrilObject, m_tendrilParent.transform.position, m_tendrilObject.transform.rotation) as GameObject;
		newTendril.transform.Rotate (new Vector3 (0, rot, 0), Space.World);
		newTendril.transform.parent = m_tendrilParent.transform;
		newTendril.SetActive(true);
	}
}
