using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthModifier : MonoBehaviour
{
    public int value;
    public bool damage;
    public bool boost;

    // Use this for initialization
    void Start()
    {
        if (damage) value = 0 - value;
        else if (boost) value = 0 + value;
    }
}