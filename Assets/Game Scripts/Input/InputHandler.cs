using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	public enum PressState { NO_PRESS, FIRST_FRAME_DOWN, HELD_DOWN, FIRST_FRAME_NO_PRESS, };

	private PressState m_pressState = PressState.NO_PRESS;
	private Vector3 m_lastPressWS;

	private Plane m_XZPlane;

	void Start () {
		m_XZPlane = new Plane (Vector3.up, Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			m_pressState = PressState.FIRST_FRAME_DOWN;
			m_lastPressWS = GetPlanarCoordForScreenCoord (Input.mousePosition);

			GameObject test = GameObject.CreatePrimitive (PrimitiveType.Cube);
			test.transform.position = m_lastPressWS;

		} else if (Input.GetMouseButton (0)) {
			m_pressState = PressState.HELD_DOWN;
			m_lastPressWS = GetPlanarCoordForScreenCoord (Input.mousePosition);
		} else if (Input.GetMouseButtonUp (0)) {
			m_pressState = PressState.FIRST_FRAME_NO_PRESS;
		} else {
			m_pressState = PressState.NO_PRESS;
		}
	}

	Vector3 GetPlanarCoordForScreenCoord(Vector3 screenCoord) {
		float dist;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (m_XZPlane.Raycast(ray, out dist)){
			Vector3 hit = ray.GetPoint(dist);
			return hit;
		}
		return new Vector3 (0, 100, 0);
	}

	public Vector3 GetLastPressWorldCoord() {
		if (m_pressState != PressState.NO_PRESS) {
			return m_lastPressWS;
		}
		return new Vector3 (0, 100, 0);
	}

	public PressState GetPressState() {
		return m_pressState;
	}
}
