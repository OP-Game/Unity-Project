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


    // Use this for initialization
    void Start()
    {

        shotStart = this.transform.Find("FirstPersonCharacter/Blaster Pistol/shotStart").gameObject;
        exploder = GameObject.FindGameObjectWithTag("ExploderMaster");
        exploderObject = exploder.GetComponent<Exploder.ExploderObject>();
        bulletPrefab = GameObject.Find("Tri_bullet");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            Fire();
        }


    }

    void Fire()
    {
        //spawn Bullet from prefab
        var bullet = PhotonNetwork.Instantiate("Tri_bullet", shotStart.transform.position, shotStart.transform.rotation, 0);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 200;
       
    }
}
