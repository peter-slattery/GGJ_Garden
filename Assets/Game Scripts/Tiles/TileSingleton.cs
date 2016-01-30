using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileSingleton : MonoBehaviour {

    public List<GameObject> m_prefabs;
	public bool m_shouldOrientVerticals = true;

	// Use this for initialization
	void Start () {
	
	}
	
	public GameObject GetPrefabOfType(TileVizController.TileVizType type)
    {
        if (m_prefabs == null || m_prefabs.Count == 0)
        {
            Debug.Log("GetPrefabOfType: No Prefabs Set");
            return null;
        }

        foreach(GameObject obj in m_prefabs)
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
