using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    [SerializeField] public static bool holdingItem;
    [SerializeField] public int health;
    public Weapon itemSlot1;
    public Weapon itemSlot2;


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

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
