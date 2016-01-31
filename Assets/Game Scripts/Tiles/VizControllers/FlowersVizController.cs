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
	void Start () {
		base.Start ();

		InitializeFlowerBuds ();
	}

	void InitializeFlowerBuds (){
		flowerBuds = new GameObject[m_verticalElements.transform.childCount];
		flowers = new GameObject[m_verticalElements.transform.childCount];

		int nextBud = 0;
		int nextFlower = 0;

		for (int i = 0; i < m_verticalElements.transform.childCount; i++) {
			GameObject child = m_verticalElements.transform.GetChild (i).gameObject;
			if (child.name.Substring (0, 3) == "bud") {
				flowerBuds [nextBud] = child;
				nextBud++;
			} else if (child.name.Substring (0, 4) == "bush") {
				flowerBush = child;
			} else if (child.name.Substring (0, 4) == "flower") {
				flowers [nextFlower] = child;
				nextFlower++;
			}

			child.SetActive (false);

			numBuds++;
		}
	}

	public override void UpdateViz(float growth) {
		ShowFlowersForGrowthLevel (growth, m_flowersToGrowthRatio);
	}

	void ShowFlowersForGrowthLevel (float growth, int flowerRatio){
		if (growth > 6.6) {
			for (int c = 0; c < m_verticalElements.transform.childCount; c++) {
				GameObject child = m_verticalElements.transform.GetChild (c).gameObject;
				if (child.activeSelf) {
					Destroy (child);
				}
			}
			Instantiate (flowerBush, m_verticalElements.transform.position, Quaternion.identity);
		} else {
			int numFlowers = (int)(growth * flowerRatio);
			numFlowers -= m_verticalElements.transform.childCount - numBuds;

			if (numFlowers >= 0) {
				for (int i = 0; i < numFlowers; i++) {
				 
					GameObject toInst;

					float chanceFlower = Random.Range (0.0f, 10.0f);

					if (chanceFlower + growth > 8.0f) {
						toInst = flowers [Random.Range (0, lastFlower)];
					} else {
						toInst = flowerBuds [Random.Range (0, lastBud)];
					}

					Vector2 offset = Random.insideUnitCircle;

					GameObject flower = Instantiate (toInst,
						                   m_verticalElements.transform.position + (new Vector3 (offset.x, 0, offset.y) * (1.73f / 2)),
						                   Quaternion.identity) as GameObject;
					flower.transform.SetParent (m_verticalElements.transform);
					flower.SetActive (true);

				}
			} else {
				for (int i = 0; i > numFlowers; i--) {
					GameObject child = m_verticalElements.transform.GetChild (Random.Range (0, m_verticalElements.transform.childCount - 1)).gameObject;
					if (child.activeInHierarchy) {
						Destroy (child);
					}
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
