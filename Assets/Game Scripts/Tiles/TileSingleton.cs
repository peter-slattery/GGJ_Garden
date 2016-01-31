using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileSingleton : MonoBehaviour {

	public GameObject m_tileParent;

    public List<GameObject> m_tilePrefabs;
	public bool m_shouldOrientVerticals = true;

	// Use this for initialization
	void Start () {
	
	}
	
	public GameObject GetPrefabOfType(TileTypeController.TileType type)
    {
        if (m_tilePrefabs == null || m_tilePrefabs.Count == 0)
        {
            Debug.Log("GetPrefabOfType: No Prefabs Set");
            return null;
        }

        foreach(GameObject obj in m_tilePrefabs)
        {
			TilePrefabID id = obj.GetComponent<TilePrefabID>();

            if (id == null)
            {
                Debug.Log("GetPrefabOfType: Prefab has no TilePrefabID");
                return null;
            }

			if (id.m_type == type)
            {
                return obj;
            }
        }
        Debug.Log("GetPrefabOfType: No prefab with requested type found");
        return null;
    }
}
