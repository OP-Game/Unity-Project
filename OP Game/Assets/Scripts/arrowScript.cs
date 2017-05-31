using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 5f);
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.GetComponent<Rigidbody>().velocity != Vector3.zero && !this.GetComponent<Rigidbody>().isKinematic)
            this.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(this.GetComponent<Rigidbody>().velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.GetComponent<Rigidbody>().isKinematic = true;


    }
}
