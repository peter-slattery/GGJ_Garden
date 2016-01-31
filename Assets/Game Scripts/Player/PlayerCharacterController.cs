using UnityEngine;
using System.Collections;

public class PlayerCharacterController : MonoBehaviour {

	// Player Statistics
	public float m_speed = 2.0f;

	InputHandler m_input;
	Vector3 m_lastTarget;

	Animator m_anim;

	// Use this for initialization
	void Start () {
		m_input = FindObjectOfType (typeof(InputHandler)) as InputHandler;

		m_anim = transform.GetChild(0).GetComponent<Animator> ();
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
		float distance_epsilon = .3f;

		Vector3 targetCorrected = target;
		targetCorrected.y = transform.position.y;

		Vector3 toTarget = (targetCorrected - transform.position).normalized;

		int dir = 0;

		float angle = Quaternion.FromToRotation (Vector3.right, toTarget).eulerAngles.y;

		if (angle <= 85 || angle > 320) {
			dir = 1;
		} else if (angle > 80 && angle <= 165) {
			dir = 3;
		} else if (angle > 165 && angle <= 271) {
			dir = 4;
		} else {
			dir = 2;
		}

		m_anim.SetInteger ("State", dir);
		m_anim.SetBool ("Moving", true);

		while (Vector3.Distance (transform.position, targetCorrected) > distance_epsilon) {
			
			Vector3 nextFrame = toTarget * m_speed * Time.deltaTime;
			Vector2 onPlane = new Vector2 (nextFrame.x, nextFrame.z);

			Tile nextTile = GridController.getCurInstance ().getTile (GridController.WorldToGrid (onPlane));

			Debug.Log (nextTile.tileType);

			if (nextTile.tileType == TileTypeController.TileType.TILE_BLANK ||
			    nextTile.tileType == TileTypeController.TileType.TILE_FLOWERS ||
			    nextTile.tileType == TileTypeController.TileType.TILE_TILLED) {

				transform.Translate (toTarget * m_speed * Time.deltaTime);

				yield return null;
			} else {
				break;
			}
		}

		m_anim.SetBool ("Moving", false);
    }

    void OnMouseDown()
    {
        Inventory._instance.Open();
    }
}
