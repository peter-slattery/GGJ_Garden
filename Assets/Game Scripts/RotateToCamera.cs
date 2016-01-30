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
		if (!Application.isPlaying) {
			FaceCamera ();
		}
	}

	void FaceCamera() {
		if (RotateToCamera.m_rotateToFace) {
			Vector3 camPos = Camera.main.transform.position;
			camPos.y = transform.position.y;
			transform.LookAt (camPos);
		}
	}
}
