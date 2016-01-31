using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrassVizController : TileVizController {

	GameObject[] grassTufts;
	int numSeeds = 0;

	public int m_grassToGrowthRatio = 5;	

	// Use this for initialization
	void Start () {
		base.Start ();

		InitializeGrassTufts ();
	}

	void InitializeGrassTufts () {
		grassTufts = new GameObject[m_verticalElements.transform.childCount];

		for (int i = 0; i < m_verticalElements.transform.childCount; i++) {
			grassTufts [i] = m_verticalElements.transform.GetChild (i).gameObject;
			grassTufts [i].SetActive (false);
			numSeeds++;
		}
	}

	public override void UpdateViz(float growth) {
		ShowGrassForGrowthLevel (growth, m_grassToGrowthRatio);
	}

	void ShowGrassForGrowthLevel(float growth, int grassRatio){
		int numGrass = (int)(growth * grassRatio);
		numGrass -= m_verticalElements.transform.childCount - numSeeds;

		if (numGrass >= 0) {
			for (int i = 0; i < numGrass; i++) {
				Vector2 offset = Random.insideUnitCircle;

				GameObject tuft = Instantiate (grassTufts [Random.Range (0, grassTufts.Length - 1)], 
					                  m_verticalElements.transform.position + (new Vector3 (offset.x, 0, offset.y) * (1.73f / 2)),
					                  Quaternion.identity) as GameObject;
				tuft.transform.SetParent (m_verticalElements.transform);
				tuft.SetActive (true);
			}
		} else {
			for (int i = 0; i > numGrass; i--) {
				GameObject child = m_verticalElements.transform.GetChild (Random.Range (0, m_verticalElements.transform.childCount - 1)).gameObject;
				if (child.activeInHierarchy) {
					Destroy (child);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
