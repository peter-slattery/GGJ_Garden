using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(RotateToCamera))]
public class RotateToCameraEditor : Editor {

	public override void OnInspectorGUI(){
		RotateToCamera.m_rotateToFace = EditorGUILayout.Toggle ("Rotate All On Update", RotateToCamera.m_rotateToFace);
	}
}
