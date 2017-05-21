using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{

    public Camera mainCam;
    private GameObject exploder;
    private Exploder.ExploderObject exploderObject;
    private GameObject shotStart;
    private Material barkMat;

    private GameObject bulletPrefab;

    private float waitTime = .2f;
    

    // Use this for initialization
    void Start()
    {
        shotStart = transform.Find("FirstPersonCharacter/Blaster Pistol/shotStart").gameObject;
        exploder = GameObject.FindGameObjectWithTag("ExploderMaster");
        exploderObject = exploder.GetComponent<Exploder.ExploderObject>();
        bulletPrefab = GameObject.Find("Bullet1");

    }

    // Update is called once per frame
    void Update()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        //  On click, performs a raycast in the direction of the camera
        //  Checks for hits, on a hit with any object tagged "Exploder", triggers explosion in the direction of the raycast.

        if (Input.GetMouseButtonUp(0))
        {
            CmdFire();
        }


    }

    void waitToDestroy()
    {
        shotStart.gameObject.active = false;
    }

    [Command]
    void CmdFire()
    {
        //spawn Bullet from prefab
        var bullet = (GameObject)Instantiate(bulletPrefab, shotStart.transform.position, shotStart.transform.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 100;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 5f);
       
    }
}
