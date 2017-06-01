using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //These variables are directly set inside of Unity when applied to an asset
    [SerializeField] public int damage;         // damage modifier
    [SerializeField] public int magSize;        // magazine capacity
    [SerializeField] public int ammoCount;      // inventory ammo minus what is in the magazine

    // These variable are able to be passed between scripts 
    public static int magAmmo;                  // ammo currently in magazine
    public static int ammoRemaining;            // ammo remaining in inventory
    public static bool magEmpty = false;                // magazine empty state
    public static bool noAmmo = false;                  // no ammo remaining state
    public static bool inHand = false;

    public KeyCode reloadKey = KeyCode.R;       // KeyBinding for the reload function
    public KeyCode fire = KeyCode.Mouse0;       // KeyBinding for the fire function

    //  Function : reload()
    /*
     *  This function is responsible for adding ammo back into the magazine from the inventory.
     *
     *  Is conditional on the player actually having ammo for this weapon.
     *  
     *  If they do the function iterates through adding ammo to the magazine, and subtracting it
     *  from the inventory, if they run out of inventory ammo before finishing the reload, the function
     *  terminates
     */
    void reload()
    {
        if (magAmmo < magSize && Input.GetKeyDown(reloadKey))
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

    // Function : checkAmmo()
    /*
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
    // Fires the weapon if it has ammo in the Magazine and 
    void fireWeapon()
    {
        if(inHand == true && magEmpty == false && Input.GetKeyDown(fire))
        {
            magAmmo--;
        }
    }

    // Function : Start()
    /*
     * This function runs automatically on object creation and sets static variables
     * for use in other scripts
     */
    private void Start()
    {
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
        fireWeapon();
        reload();
    }
}
