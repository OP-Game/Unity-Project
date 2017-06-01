using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    [SerializeField] public int health;


    //Getter fuction for health, simply returns the current health value when called
    public int GetHealth()
    {
        return health;
    }
    //Setter function for health recieves a modifier object and adjusts health accordingly
    public int SetHealth(HealthModifier modifier)
    {
        return health + modifier.value;
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
        {

        }
	}
}
