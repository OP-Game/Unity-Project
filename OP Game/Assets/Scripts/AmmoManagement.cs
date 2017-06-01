using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManagement : MonoBehaviour {

    public Text text;
    
	// Update is called once per frame
	void Update () {
        text = GetComponent<Text>();
        text.text = Weapon.magAmmo + " / " + Weapon.ammoRemaining;
        if (Weapon.inHand)
            text.enabled = true;
        else
            text.enabled = false;

    }
}
