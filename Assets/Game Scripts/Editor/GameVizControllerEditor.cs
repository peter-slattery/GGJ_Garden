using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (TileVizController))]
public class GameVizControllerEditor : Editor {

	public override void OnInspectorGUI () {
		DrawDefaultInspector ();

		if (GUILayout.Button ("Update Elements")) {
			TileVizController[] controllers = FindObjectsOfType (typeof(TileVizController)) as TileVizController[];
			foreach (TileVizController c in controllers) {
				c.EditorUpdateElement (true);
			}
		}
	}
}
