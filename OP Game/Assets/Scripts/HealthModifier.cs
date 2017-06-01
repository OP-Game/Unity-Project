using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthModifier : MonoBehaviour
{
    [SerializeField] public int value;
    [SerializeField] public bool damage = false;
    [SerializeField] public bool boost = false;

    // Use this for initialization
    void Update()
    {
        if (damage) value = 0 - value;
        else if (boost) value = 0 + value;
    }
}