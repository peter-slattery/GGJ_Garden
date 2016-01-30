using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileSingleton : MonoBehaviour {

    public List<GameObject> m_prefabs;

	// Use this for initialization
	void Start () {
	
	}
	
	public void InstantiatePrefabOfType()
    {
        foreach(GameObject obj in m_prefabs)
        {

        }
    }
}
