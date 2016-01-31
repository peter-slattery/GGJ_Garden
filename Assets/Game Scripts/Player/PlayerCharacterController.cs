using UnityEngine;
using System.Collections;

public class PlayerCharacterController : MonoBehaviour {

	// Player Statistics
	public float m_speed = 2.0f;

	InputHandler m_input;
	Vector3 m_lastTarget;

	// Use this for initialization
	void Start () {
		m_input = FindObjectOfType (typeof(InputHandler)) as InputHandler;
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput ();
	}

	public void HandleInput () {
		if (m_input == null) {
			Debug.Log ("HandlePressInput: InputHandler Not Found");
			return;
		}

		// Get Mouse Input
		if (m_input.GetInputMode () == InputHandler.InputMode.INTERACT_WORLD) {
			Vector3 target = m_input.GetLastPressWorldCoord ();
			// Only accept targets on the xz plane
			if (target.y < 0.001f && target.y > -0.001f) {
				if (target != m_lastTarget) {
					StopCoroutine ("FindPathToTarget");
					StartCoroutine ("FindPathToTarget", target);
					m_lastTarget = target;
				}
			}
		}
	}

	IEnumerator FindPathToTarget(Vector3 target){
		float distance_epsilon = 1;

		Vector3 targetCorrected = target;
		targetCorrected.y = transform.position.y;

		while (Vector3.Distance (transform.position, targetCorrected) > distance_epsilon) {
			Vector3 toTarget = (targetCorrected - transform.position).normalized;
			transform.Translate (toTarget * m_speed * Time.deltaTime);
			yield return null;
		}
    }

    void OnMouseDown()
    {
        Inventory._instance.Open();
    }
}
