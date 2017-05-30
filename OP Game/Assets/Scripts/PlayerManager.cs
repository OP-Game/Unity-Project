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

    // Use this for initialization
    void Start()
    {

        equippedWep = "Pistol";

        shotStart = this.transform.Find("FirstPersonCharacter/Blaster Pistol/shotStart").gameObject;
        exploder = GameObject.FindGameObjectWithTag("ExploderMaster");
        exploderObject = exploder.GetComponent<Exploder.ExploderObject>();
        bulletPrefab = GameObject.Find("Tri_bullet");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if (Input.GetKeyDown("Equip"))
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

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 100;

        }
        
       
    }

    void Equip(string weapon)
    {

    }
}
