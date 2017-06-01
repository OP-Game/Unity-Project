using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour {
    
    public bool doorOpen = false;
    public bool inRange;
    public float smooth = 1f;
    private Quaternion targetRotation;

    void OnTriggerEnter(Collider other)
    {
        inRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        inRange = false;
    }

    void Start()
    {
        targetRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update () {
        if (doorOpen == false && inRange == true && Input.GetKeyDown(KeyCode.F))
        {
            targetRotation *= Quaternion.AngleAxis(90, Vector3.up);
            doorOpen = true;
        }
        else if (doorOpen == true && inRange == true && Input.GetKeyDown(KeyCode.F))
        {
            targetRotation *= Quaternion.AngleAxis(-90, Vector3.up);
            doorOpen = false;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10 * smooth * Time.deltaTime);
    }
}
