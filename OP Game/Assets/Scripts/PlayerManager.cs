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

        if(equippedWep == "Bow" && Input.GetButtonDown("Fire1"))
        {

           startTime = Time.time;

        }
        if(equippedWep == "Bow" && Input.GetButtonUp("Fire1"))
        {

            power = Time.time - startTime;
            if(power * 75 >= 250f)
            {
                power = 250f;
            }
            else
            {
                power = power * 75f;
            }

            Fire();

            startTime = 0f;
        }

        if (Input.GetMouseButtonDown(0) && equippedWep != "Bow")
        {
            Fire();
        }

        if (Input.GetButtonDown("Equip"))
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, transform.forward, out hit, 10f))
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

        if(equippedWep == "Bow")
        {

            var bullet = PhotonNetwork.Instantiate("Arrow", shotStart.transform.position, shotStart.transform.rotation, 0);



            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * power;

        }
        
       
    }

    void Equip(string weapon)
    {
        var currentWep = this.transform.Find("FirstPersonCharacter/" + equippedWep).gameObject;

        currentWep.SetActive(false);


        equippedWep = weapon;
        currentWep = this.transform.Find("FirstPersonCharacter/" + equippedWep).gameObject;
        currentWep.SetActive(true);

        shotStart = this.transform.Find("FirstPersonCharacter/" + weapon + "/shotStart").gameObject;

    }
}
