using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSystem : MonoBehaviour {

    // Update is called once per frame
    private void Start()
    {
        
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();		
	}
}
