using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float m_panSpeed = 5.0f;

	private InputHandler m_input;

	private Vector3 m_forward;
	private Vector3 m_right;

	// Use this for initialization
	void Start () {
		m_input = FindObjectOfType (typeof(InputHandler)) as InputHandler;
		if (m_input == null) {
			Debug.Log ("CameraController: No InputHandler");
		}

		m_right = transform.right;
		m_forward = transform.forward;
		m_forward.y = 0;
		m_forward.Normalize ();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_input.GetInputMode() == InputHandler.InputMode.PAN_CAMERA) {
			Vector3 delta = m_input.GetDeltaPressSS ();
			if (delta.z > -0.01f && delta.z < 0.01f) {
				float horizontalMove = -delta.x * m_panSpeed;
				float verticalMove = -delta.y * m_panSpeed;

				transform.Translate (m_forward * verticalMove * Time.deltaTime, Space.World);
				transform.Translate (m_right * horizontalMove * Time.deltaTime, Space.World);
			}
		}
	}

}
