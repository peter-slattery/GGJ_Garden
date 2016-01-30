using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileSingleton : MonoBehaviour {

    public List<GameObject> m_tilePrefabs;
	public bool m_shouldOrientVerticals = true;

	// Use this for initialization
	void Start () {
	
	}
	
	public GameObject GetPrefabOfType(TileVizController.TileVizType type)
    {
        if (m_tilePrefabs == null || m_tilePrefabs.Count == 0)
        {
            Debug.Log("GetPrefabOfType: No Prefabs Set");
            return null;
        }

        foreach(GameObject obj in m_tilePrefabs)
        {
            TileVizController controller = obj.GetComponent<TileVizController>();

            if (controller == null)
            {
                Debug.Log("GetPrefabOfType: Prefab has no TileVizController");
                return null;
            }

            if (controller.m_tileType == type)
            {
                return obj;
            }
        }
        Debug.Log("GetPrefabOfType: No prefab with requested type found");
        return null;
    }
}
