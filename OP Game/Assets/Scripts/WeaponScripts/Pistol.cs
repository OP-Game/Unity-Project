using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour {

    /*
    needsReload - does this weapon need to be reloaded
    reloadTime - time in seconds it takes to reload
    shotDelay - time in seconds between consecutive shots
    maxAmmo - total ammo count
    clipSize - number of rounds in a single clip
    fovZoomAmt - amount of FOV zoom to be applied when ADS
    projectilePrefab - string name of the projectile to be instantiated when fired
    power - velocity amount to be added to the projectile when fired
    */

    public bool needsReload = false;
    public float reloadTime = 0;
    public float shotDelay = 1f;
    public int maxAmmo = 100;
    public int clipSize = 100;
    public float fovZoomAmt = 15f;
    public string projectilePrefab = "Tri__Bullet";
    public float shotPower = 200;
	
}
