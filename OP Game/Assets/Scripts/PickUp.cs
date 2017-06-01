using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
    public Transform onhand;
    //Boolean to establish if the item is within pickup range
    [SerializeField] public static bool inRange = false;
    //The KeyCode for pick up key binding
    public KeyCode pickUpKey = KeyCode.F;

    //This function returns TRUE when hand is in range of grabbable object
    void OnTriggerEnter(Collider other)
    {
        inRange = true;
    }

    //This function returns FALSE when hand isn't in range of grabbable object
    void OnTriggerExit(Collider other)
    {
        inRange = false;
    }

    //This function will allow the user to pick up / drop grabbable items
    void Update()
    {
        // If in range and not already in hand, pick up the object if keypress
        // else it will drop the item
        if (inRange == true && Weapon.inHand == false && Input.GetKeyDown(pickUpKey))
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
            this.transform.position = onhand.position;
            this.transform.rotation = onhand.rotation;
            this.transform.parent = GameObject.Find("FPS_Hand").transform;
            this.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1);
            PlayerStats.holdingItem = true;
            Weapon.inHand = true;
        }
        else if (Weapon.inHand == true && Input.GetKeyDown(pickUpKey))
        {
            this.transform.parent = null;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            PlayerStats.holdingItem = false;
            Weapon.inHand = false;
        }
    }
}







