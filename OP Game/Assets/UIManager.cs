using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour {
    private bool isShowing = false;
    private GameObject menu;
    

    void QuitApplication()
    {
        if (isShowing == true) Application.Quit();
    }

    private void Start()
    {
        menu = GameObject.Find("/UI/menu");
    }

    private void Update()
    {
        menu.SetActive(isShowing);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isShowing = !isShowing;
        }

    }

}
