using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OPGame
{
    public class Weapon
    {

        /*
        needsReload - does this weapon need to be reloaded
        reloadTime - time in seconds it takes to reload
        shotDelay - time in seconds between consecutive shots
        maxAmmo - total ammo count
        clipSize - number of rounds in a single clip
        fovZoomAmt - amount of FOV zoom to be applied when ADS
        projectilePrefab - string name of the projectile to be instantiated when fired
        power - velocity amount to be added to the projectile when fired
        bulletDamage - damage in Hearts per projectile;
        */

        public bool needsReload;
        public float reloadTime;
        public float shotDelay;
        public int maxAmmo;
        public int clipSize;
        public float fovZoomAmt;
        public string projectilePrefab;
        public float shotPower;
        public int projectileDamage;
        public GameObject shotStart;


        /*
        *  DEFAULT WEAPON CONSTRUCTOR 
        *  Constructor is used when the Weapon being equipped requires reloading and has a set amount of ammo
        */
        public Weapon(bool needsRld, float rldTime, float delay, int ammo, int clip, float zoom, string projectile, float power, int dmg, GameObject start)
        {
            needsReload = needsRld;
            reloadTime = rldTime;
            shotDelay = delay;
            maxAmmo = ammo;
            clipSize = clip;
            fovZoomAmt = zoom;
            projectilePrefab = projectile;
            shotPower = power;
            projectileDamage = dmg;
            shotStart = start;

        }

        public void Fire()
        {

            var bullet = PhotonNetwork.Instantiate(projectilePrefab, shotStart.transform.position, shotStart.transform.rotation, 0);

        }

    }
}



