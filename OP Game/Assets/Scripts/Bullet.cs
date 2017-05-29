using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public void Start()
    {
        Destroy(this.gameObject, 5f);
    }

    private TreeHealth treeHealth;
    void OnCollisionEnter(Collision collision)
    {

        var hit = collision.gameObject;
        var contactPoint = collision.contacts[0];
       
        if(hit.tag == "Exploder" && this.tag == "Bomb")
        {
            if(hit.transform.parent.tag == "TerrainObj")
            {
                treeHealth = hit.GetComponent<TreeHealth>();
                treeHealth.TakeDamage(3, transform.forward, transform.position);
            }
            if (hit.tag != "Terrain")
            {
                Destroy(this.gameObject);
            }
        }

        if(gameObject.tag == "Bullet")
        {
            if(hit.tag == "Player")
            {
                PhotonNetwork.Instantiate("PlayerHit", contactPoint.point, this.transform.rotation, 0);
            }
            Destroy(gameObject);
        }
    }
}
