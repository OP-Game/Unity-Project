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
    private Material barkMat;

    private GameObject bulletPrefab;

    private float waitTime = .2f;

    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;

    // Use this for initialization
    void Start()
    {

        shotStart = this.transform.Find("FirstPersonCharacter/Blaster Pistol/shotStart").gameObject;
        exploder = GameObject.FindGameObjectWithTag("ExploderMaster");
        exploderObject = exploder.GetComponent<Exploder.ExploderObject>();
        bulletPrefab = GameObject.Find("Bullet1");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
        */
        //  On click, performs a raycast in the direction of the camera
        //  Checks for hits, on a hit with any object tagged "Exploder", triggers explosion in the direction of the raycast.

        if (Input.GetMouseButtonUp(0))
        {
            Fire();
        }


    }

   

    void waitToDestroy()
    {
        shotStart.gameObject.active = false;
    }
    
    void Fire()
    {
        //spawn Bullet from prefab
        var bullet = PhotonNetwork.Instantiate("Bullet1", shotStart.transform.position, shotStart.transform.rotation, 0);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 100;


        Destroy(bullet, 5f);
       
    }
}
