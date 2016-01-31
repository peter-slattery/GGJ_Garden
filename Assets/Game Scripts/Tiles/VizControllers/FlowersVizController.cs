using UnityEngine;
using System.Collections;

public class FlowersVizController : TileVizController {

	GameObject[] flowerBuds;
	GameObject[] flowers;
	GameObject flowerBush;

	int lastBud = -1;
	int lastFlower = -1;

	int numBuds = 0;

	public int m_flowersToGrowthRatio = 5;

	// Use this for initialization
	public override void Start () {
		base.Start ();

		InitializeViz ();
	}

	public override void InitializeViz (){
		if (!m_initialized) {
			base.InitializeViz ();

			flowerBuds = new GameObject[m_verticalElements.transform.childCount];
			flowers = new GameObject[m_verticalElements.transform.childCount];

			int nextBud = 0;
			int nextFlower = 0;

			for (int i = 0; i < m_verticalElements.transform.childCount; i++) {
				GameObject child = m_verticalElements.transform.GetChild (i).gameObject;
				if (child.name.Substring (0, 1) == "0") {
					flowerBuds [nextBud] = child;
					nextBud++;
				} else if (child.name.Substring (0, 1) == "2") {
					flowerBush = child;
				} else if (child.name.Substring (0, 1) == "1") {
					flowers [nextFlower] = child;
					nextFlower++;
				}

				child.SetActive (false);

				numBuds++;
			}

			lastBud = nextBud - 1;
			lastFlower = nextFlower - 1;
		}
	}

	public override void UpdateViz(float growth) {
		ShowFlowersForGrowthLevel (growth, m_flowersToGrowthRatio);
	}

	void ShowFlowersForGrowthLevel (float growth, int flowerRatio){
		if (growth > 7.0f) {
			GameObject newBush = Instantiate (flowerBush, m_verticalElements.transform.position, flowerBush.transform.rotation) as GameObject;
			newBush.transform.SetParent (m_verticalElements.transform);
			newBush.SetActive (true);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
