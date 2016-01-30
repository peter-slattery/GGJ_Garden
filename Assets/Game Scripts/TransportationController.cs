using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TransportationController : MonoBehaviour {

	public string m_levelToGoTo;

	private InputHandler m_input;

	// Use this for initialization
	void Start () {
		m_input = FindObjectOfType (typeof(InputHandler)) as InputHandler;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown () {
		if (m_input == null) {
			Debug.Log ("OnMouseDown: No InputHandler Found");
			return;
		}

		if (m_input.GetInputMode () == InputHandler.InputMode.INTERACT_WORLD) {
			GoToLevel (m_levelToGoTo);
		}
	}

	private void GoToLevel(string levelName){
		SceneManager.LoadScene (levelName);
	}
}
