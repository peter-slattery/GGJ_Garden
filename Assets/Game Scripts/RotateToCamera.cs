using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RotateToCamera : MonoBehaviour {

	public static bool m_rotateToFace = true;

	// Use this for initialization
	void Start () {
		FaceCamera ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Application.isPlaying && m_rotateToFace) {
			FaceCamera ();
		}
	}

	void FaceCamera() {
		if (RotateToCamera.m_rotateToFace) {
			Vector3 camLook = -Camera.main.transform.forward;
			camLook.y = 0;
			camLook.Normalize ();
			Vector3 lookAtPos = transform.position + camLook;
			transform.LookAt (lookAtPos);
		}
	}
}
