using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : MonoBehaviour
{

    public Camera mainCam;
    private GameObject exploder;
    private Exploder.ExploderObject exploderObject;
    private GameObject shotStart;
    private GameObject bulletPrefab;
    public string equippedWep;
    public bool isAiming;
    private float startTime, power;


    // Use this for initialization
    void Start()
    {

        equippedWep = "Pistol";

        shotStart = this.transform.Find("FirstPersonCharacter/Pistol/shotStart").gameObject;
        exploder = GameObject.FindGameObjectWithTag("ExploderMaster");
        exploderObject = exploder.GetComponent<Exploder.ExploderObject>();
        bulletPrefab = GameObject.Find("Tri_bullet");
    }

    // Update is called once per frame
    void Update()
    {
        if(this.GetComponentInChildren<Camera>().fieldOfView >= 79.8f && !this.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().isSprinting && !isAiming)
        {
            this.GetComponentInChildren<Camera>().fieldOfView = 80f;
        }

        //When you PRESS the "Fire1" input, capture the current time
        //Set aiming to true, starts the FOV coroutin
        if (equippedWep == "Bow" && Input.GetButtonDown("Fire1"))            
        {
            isAiming = true;
            startTime = Time.time;
            StopCoroutine(aimFOV());
            StopCoroutine(returnFOV());
            StartCoroutine(aimFOV());
        }

        //When you RELEASE the "Fire1" input, get the difference in time since you pressed fire, then multiply it by 125 up to a max of 250, which is then used as the force for the arrow.
        if (equippedWep == "Bow" && Input.GetButtonUp("Fire1"))              
        {
            isAiming = false;
            StopCoroutine(aimFOV());
            StopCoroutine(returnFOV());
            StartCoroutine(returnFOV());

            power = Time.time - startTime;
            if(power * 125 >= 250f)
            {
                power = 250f;
            }
            else
            {
                power = power * 125f;
            }

            Fire();

            //Reset start time
            startTime = 0f;

            
        }

        if (Input.GetMouseButtonDown(0) && equippedWep != "Bow")            //If you aren't holding a bow, use the standard "Fire" script
        {
            Fire();                                 
        }

        //Equip section performs a 2f Raycast to see if an equippable weapon is in front of the player, if so it runs Equip(weaponName)

        if (Input.GetButtonDown("Equip"))           
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                if(hit.transform.name == "Bow")
                {
                    Equip("Bow");
                }
            }
        }

    }

    void Fire()
    {
        //spawn Bullet from prefab
        if(equippedWep == "Pistol")
        {
            var bullet = PhotonNetwork.Instantiate("Tri_bullet", shotStart.transform.position, shotStart.transform.rotation, 0);

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 200;
        }


        //if bow, spawn an arrow, apply force calculated earlier
        if(equippedWep == "Bow")
        {

            var bullet = PhotonNetwork.Instantiate("Arrow", shotStart.transform.position, shotStart.transform.rotation, 0);



            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * power;

        }
        
       
    }


    //Equip script checks the current weapon. Then it deactivates it, and gets the weapon the player is trying to equip. Then sets the shotStart gameObject to the shotStart transform attached to the weapon.
    void Equip(string weapon)
    {

        var currentWep = this.transform.Find("FirstPersonCharacter/" + equippedWep).gameObject;

        currentWep.SetActive(false);


        equippedWep = weapon;
        currentWep = this.transform.Find("FirstPersonCharacter/" + equippedWep).gameObject;
        currentWep.SetActive(true);

        shotStart = this.transform.Find("FirstPersonCharacter/" + weapon + "/shotStart").gameObject;

    }

    IEnumerator aimFOV()
    {
        while (isAiming)
        {
            this.GetComponentInChildren<Camera>().fieldOfView = Mathf.Lerp(this.GetComponentInChildren<Camera>().fieldOfView, 40f, Time.deltaTime / 1f);
            yield return null;
        }

    }
    IEnumerator returnFOV()
    {
        while(!isAiming)
        {
            this.GetComponentInChildren<Camera>().fieldOfView = Mathf.Lerp(this.GetComponentInChildren<Camera>().fieldOfView, 80f, Time.deltaTime / .25f);
            yield return null;
        }
    }

}
