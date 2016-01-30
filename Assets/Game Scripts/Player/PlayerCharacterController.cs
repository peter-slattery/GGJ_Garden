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
		HandlePressInput ();
	}

	void HandlePressInput () {
		if (m_input == null) {
			Debug.Log ("HandlePressInput: InputHandler Not Found");
			return;
		}

		Vector3 target = m_input.GetLastPressWorldCoord ();
		// Only accept targets on the xz plane
		if (target.y < 0.001f && target.y > -0.001f) {
			if (target != m_lastTarget) {
				StopCoroutine ("FindPathToTarget");
				StartCoroutine ("FindPathToTarget", target);
			}
		}


	}

	IEnumerator FindPathToTarget(Vector3 target){
		float distance_epsilon = 1;

		while (Vector3.Distance (transform.position, target) > distance_epsilon) {
			Vector3 toTarget = (target - transform.position).normalized;
			transform.Translate (toTarget * m_speed * Time.deltaTime);
			yield return null;
		}
	}
}
