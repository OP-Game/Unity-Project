using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Weapon : MonoBehaviour
{
    //These variables are directly set inside of Unity when applied to an asset
    [SerializeField] public int magSize;                // magazine capacity
    [SerializeField] public int ammoCount;              // inventory ammo minus what is in the magazine
    [SerializeField] public float reloadTime;           // time in seconds it takes to reload   
    [SerializeField] public float shotDelay;            // delay in seconds between consecutive shots
    [SerializeField] public float fovZoomAmt;           // zoom amount when ADS
    [SerializeField] public string projectilePrefab;    // string name of the projectile prefab to be fired
    [SerializeField] public int shotPower;              // force to be applied to the projectile when fired
    [SerializeField] public int projectileDamage;       // damage (in hearts) to deal per projectile
    [SerializeField] public float recoilX, recoilY;     // amount to recoil along X (Horizontal) and Y (Vertical)
    [SerializeField] public Camera mainCam;

    // These variable are able to be passed between scripts 
    public static int magAmmo;                  // ammo currently in magazine
    public static int ammoRemaining;            // ammo remaining in inventory
    public static bool magEmpty = false;        // magazine empty state
    public static bool noAmmo = false;          // no ammo remaining state
    public static bool inHand = false;          // is currently equipped weapon              
    public static bool inBag = false;            // is this weapon in your bag
    
    private GameObject shotStart;               // game object to instantiate the projectilePrefab
    private float shotTime = 0f;

    /*  
        Function : reload()
        This function is responsible for adding ammo back into the magazine from the inventory.
     
        Is conditional on the player actually having ammo for this weapon.
       
        If they do the function iterates through adding ammo to the magazine, and subtracting it
        from the inventory, if they run out of inventory ammo before finishing the reload, the function
        terminates
    */
    void reload()
    {
        if (magAmmo < magSize && Input.GetButtonDown("Reload"))
        {
            if (noAmmo == false)
            {
                int reloadable = magSize - magAmmo;
                for (int i = 0; i < reloadable; i++)
                {
                    if (ammoRemaining > 0)
                    {
                        magAmmo++;
                        ammoRemaining--;
                    }
                    else break;
                }
            }
        }
    }

    /*
     * Function : checkAmmo()
     * This function checks to see if the player has any remaining ammo
     * if they do not it returns true, else returns false
     */
    bool checkAmmo()
    {
        if (ammoRemaining == 0 && magAmmo == 0) noAmmo = true;
        else noAmmo = false;
        return noAmmo;
    }

    bool checkMag()
    {
        if (magAmmo == 0) magEmpty = true;
        else magEmpty = false;
        return magEmpty;
    }

    // Function : fireWeapon()
    // If the current time is greater than or equal to that previous shot timestamp plus the shotDelay
    // Instantiate a network prefab of the projectile at the shotStart game object
    // Add force equal to the shotPower 
    // Timestamp this shot
    public void fireWeapon()
    {
        if (Time.time >= shotTime + shotDelay)
        {
            var projectile = PhotonNetwork.Instantiate(projectilePrefab, shotStart.transform.position, shotStart.transform.rotation, 0);
            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * shotPower;
            shotTime = Time.time;
            GetComponentInParent<CameraManager>().StartRecoil(20f, 50f, 10f, 2f);
        }
    }

    // Function : Start()
    /*
     * This function runs automatically on object creation and sets static variables
     * for use in other scripts
     */
    private void Start()
    {
        shotStart = transform.Find("shotStart").gameObject;
        magAmmo = magSize;
        ammoRemaining = ammoCount - magSize;
    }

    // Function : Update()
    /*
     * This function is responsible for updating and processing once per frame
    */
    private void Update()
    {
        checkAmmo();
        checkMag();
        reload();
    }

}
