using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(TileGenerator))]
public class TileGeneratorEditor : Editor {

	public override void OnInspectorGUI () {
		DrawDefaultInspector ();

		if (GUILayout.Button ("Generate Patch")) {
			(target as TileGenerator).GeneratePatch ();
		}
	}
}
