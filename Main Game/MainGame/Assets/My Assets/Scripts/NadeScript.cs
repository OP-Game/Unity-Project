using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NadeScript : MonoBehaviour {


    // Use this for initialization
    void Start ()
    {
        Destroy(gameObject, 3f);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 10f);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                if(hit.gameObject.tag == "Player")
                {

                }
                rb.AddExplosionForce(1000f, explosionPos, 10f, 3.0F);

            }
        }

        Destroy(gameObject);
    }
}
