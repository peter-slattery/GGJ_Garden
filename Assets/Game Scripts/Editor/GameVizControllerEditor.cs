using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (TileTypeController))]
public class GameVizControllerEditor : Editor {

	public override void OnInspectorGUI () {
		DrawDefaultInspector ();

		if (GUILayout.Button ("Update Element")) {
			(target as TileTypeController).EditorUpdateElement (true);
		}

		if (GUILayout.Button ("Update All Elements")) {
			TileTypeController[] controllers = FindObjectsOfType (typeof(TileTypeController)) as TileTypeController[];
			foreach (TileTypeController c in controllers) {
				c.EditorUpdateElement (true);
			}
		}
	}
}
