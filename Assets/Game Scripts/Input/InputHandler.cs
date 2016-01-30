using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	public enum InputMode { WAITING, INTERACT_WORLD, PAN_CAMERA, };
	public enum PressState { NO_PRESS, FIRST_FRAME_DOWN, HELD_DOWN, FIRST_FRAME_NO_PRESS, };

	public float m_minPanSpeed = .005f;

	private InputMode m_mode = InputMode.INTERACT_WORLD;
	private PressState m_pressState = PressState.NO_PRESS;

	private Vector3 m_lastPressVS;
	private Vector3 m_lastPressWS;

	private Vector3 m_lastDeltaPressVS;
	private Vector3 m_lastDeltaPressWS;

	private Plane m_XZPlane;

	void Start () {
		m_XZPlane = new Plane (Vector3.up, Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			m_pressState = PressState.FIRST_FRAME_DOWN;

			m_lastPressVS = Input.mousePosition;
			m_lastPressVS.x /= Screen.width;
			m_lastPressVS.y /= Screen.height;

			m_lastPressWS = GetPlanarCoordForScreenCoord (m_lastPressVS);

			m_lastDeltaPressVS = Vector3.zero;
			m_lastDeltaPressWS = Vector3.zero;

		} else if (Input.GetMouseButton (0)) {
			m_pressState = PressState.HELD_DOWN;

			// Store the change in press location
			Vector3 vsPress = Input.mousePosition;
			vsPress.x /= Screen.width;
			vsPress.y /= Screen.height;

			Vector3 wsPress = GetPlanarCoordForScreenCoord (vsPress);

			m_lastDeltaPressVS = vsPress - m_lastPressVS;
			m_lastDeltaPressWS = wsPress - m_lastPressWS;

			m_lastPressVS = vsPress;
			m_lastPressWS = wsPress;

			if (m_lastDeltaPressVS.magnitude > m_minPanSpeed) {
				SetInputMode (InputMode.PAN_CAMERA);
			}

		} else if (Input.GetMouseButtonUp (0)) {
			if (m_mode == InputMode.WAITING) {
				SetInputMode (InputMode.INTERACT_WORLD);
			}
			m_pressState = PressState.FIRST_FRAME_NO_PRESS;
		} else {
			SetInputMode (InputMode.WAITING);
			m_pressState = PressState.NO_PRESS;
		}
	}

	Vector3 GetPlanarCoordForScreenCoord(Vector3 screenCoord) {
		float dist;
		Ray ray = Camera.main.ViewportPointToRay (screenCoord);
		if (m_XZPlane.Raycast(ray, out dist)){
			Vector3 hit = ray.GetPoint(dist);
			return hit;
		}
		return new Vector3 (0, 100, 0);
	}

	public Vector3 GetLastPressScreenCoord(bool overrideState = false) {
		if (m_pressState != PressState.NO_PRESS  || overrideState) {
			return m_lastPressVS;
		}
		return new Vector3 (0, 0, 100);
	}

	public Vector3 GetLastPressWorldCoord(bool overrideState = false) {
		if (m_pressState != PressState.NO_PRESS || overrideState) {
			return m_lastPressWS;
		}
		return new Vector3 (0, 100, 0);
	}

	public Vector3 GetDeltaPressSS(bool overrideState = false) {
		if (m_pressState != PressState.NO_PRESS || overrideState) {
			return m_lastDeltaPressVS;
		}
		return new Vector3 (0, 0, 100);
	}

	public Vector3 GetDeltaPressWS(bool overrideState = false) {
		if (m_pressState != PressState.NO_PRESS || overrideState) {
			return m_lastDeltaPressWS;
		}
		return new Vector3 (0, 100, 0);
	}

	public InputMode GetInputMode() {
		return m_mode;
	}

	public PressState GetPressState() {
		return m_pressState;
	}

	public void SetInputMode(InputMode newMode) {
		Debug.Log ("InputState: " + newMode);
		m_mode = newMode;
	}

	IEnumerator DelaySetInput(InputMode newMode) {
		yield return new WaitForSeconds (.2f);
		SetInputMode (newMode);
	}

	public void NextInputMode() {
		switch (m_mode) {
		case InputMode.INTERACT_WORLD:
			SetInputMode(InputMode.PAN_CAMERA);
			break;
		default:
			StartCoroutine (DelaySetInput (InputMode.INTERACT_WORLD));
			break;
		}
	}
}
