﻿using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour {

    public static ButtonManager _instance;

	void Awake ()
    {
        _instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Inventory._instance.Toggle();
        }
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
