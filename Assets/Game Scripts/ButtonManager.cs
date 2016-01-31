using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour {

    public Transform _instance;

	void Awake ()
    {
        _instance = this.transform;
	}
	
	//public void OpenInventory ()
 //   {
 //       Inventory._instance.Open();
 //   }
    public void CloseInventory()
    {
        Inventory._instance.Close();
    }
}
