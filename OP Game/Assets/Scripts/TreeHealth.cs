using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TreeHealth : MonoBehaviour {

    public const int startHealth = 5;

    public int currentHealth = startHealth;

    public GameObject exploder;
    public Exploder.ExploderObject exploderObject;
    public Material barkMat;

    // Use this for initialization
    void Start () {

        

    }

	
	// Update is called once per frame
	void Update () {


		
	}

    public void TakeDamage(int amount, Vector3 hitDirection, Vector3 hitPosition)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;

            exploder.transform.position = hitPosition;
            exploderObject.ForceVector = hitDirection;
            exploderObject.ExplodeObject(this.gameObject);

            Debug.Log("Dead!");
        }
    }
}
