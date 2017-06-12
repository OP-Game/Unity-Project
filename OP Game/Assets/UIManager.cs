using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour {
    private bool isShowing = false;
    private GameObject menu;
    

    public void QuitApplication()
    {
        if (isShowing == true) {
            print("QUIT THE FUCKING GAME");
            Application.Quit();
        }
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
