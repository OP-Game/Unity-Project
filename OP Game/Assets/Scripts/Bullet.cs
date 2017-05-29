using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public void Awake()
    {
        Destroy(this.gameObject, 5f);
    }

    private TreeHealth treeHealth;
    void OnCollisionEnter(Collision collision)
    {

        var hit = collision.gameObject;
        
       
        if(hit.tag == "Exploder")
        {
            if(hit.transform.parent.tag == "TerrainObj")
            {
                treeHealth = hit.GetComponent<TreeHealth>();
                treeHealth.TakeDamage(3, transform.forward, transform.position);
            }
        }

        if (hit.tag != "Terrain")
        {
            Destroy(gameObject);
        }
    }
}
