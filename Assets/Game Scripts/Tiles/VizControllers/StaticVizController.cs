using UnityEngine;
using System.Collections;

public class StaticVizController : TileVizController {

	// Use this for initialization
	public override void Start () {
		base.Start ();


	}

	public override void InitializeViz() {
		if (!m_initialized) {
			base.InitializeViz ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void UpdateViz(float growth) {
		return;
	}
}
