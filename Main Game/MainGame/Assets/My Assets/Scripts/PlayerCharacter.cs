using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;


public class PlayerCharacter : Photon.MonoBehaviour {


    #region Network Variables
    public Camera myCamera;                             //the Camera attached to this player
    private Vector3 position;                           //the position moving to, used for movement lerping via network
    private Quaternion rotation;                        //target rotation, for rotation lerping via network
    bool isAlive = true;                                //checks to see if this player is active in the network
    private GameObject myPlayer;
    public CameraController cameraControls;
    #endregion

    #region Character Control Variables

    private string currentWep;
    string projectilePrefab = "NadeProjectile";

    private bool grounded = false;      // Bool for testing if we are currently grounded
    bool isGrappling = false;           // Bool for testing if we are grappling

    private Vector3 groundVelocity = Vector3.zero;
    public CapsuleCollider capsule;
    private Vector3 inputVector = Vector3.zero;
    
    
    // Inputs Cache
    private bool jumpFlag = false;

    // Speeds
    float walkSpeed = 8.0f;
    float walkBackwardSpeed = 4.0f;
    float runSpeed = 14.0f;
    float runBackwardSpeed = 6.0f;
    float sidestepSpeed = 8.0f;
    float runSidestepSpeed = 12.0f;
    float maxVelocityChange = 10.0f;

    // Air
    float inAirControl = 0.1f;
    float jumpHeight = 3f;

    // Can Flags
    bool canRunSidestep = true;
    bool canJump = true;
    bool canRun = true;
    bool canControl = false;

    private Transform myTransform;          // The transform of this game object
    private Transform shotStart;            // Transform of the start location for a fired projectile

    public Rigidbody myRigidbody;          // The rigidbody of this character, used for enabling physics

    private GameObject nadeLauncher, grappleBow;        // gameObjects for each weapon

    private Vector3 moveDirection = Vector3.zero;       // The target movement direction, set to zero at start to prevent auto-moving
    private Vector3 jumpDirection = Vector3.zero;       // The direction that we jumped towards, set to zero to prevent auto-moving

    #endregion


    // Use this for initialization
    void Start () {
        // if the photonview on this object belongs to my player character
        if (photonView.isMine)
        {
            // enable my camera, get my Character Controller, and get my transform
            myRigidbody.isKinematic = false;

            myTransform = this.transform;

            canControl = true;
            myCamera.enabled = true;
            cameraControls.enabled = true;

            myRigidbody.freezeRotation = true;
            myRigidbody.useGravity = true;

            grappleBow = myTransform.Find("Main Camera/Grapple Bow").gameObject;
            nadeLauncher = myTransform.Find("Main Camera/Nade Launcher").gameObject;

            shotStart = myTransform.Find("Main Camera/Nade Launcher/shotStart");

            currentWep = "Grapple Bow";
        }
        else
        {
            StartCoroutine(Alive());
        }
	}
    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Alpha1) && currentWep != "Grapple Bow")
        {
            SetWeapon(grappleBow);
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha2) && currentWep != "Nade Launcher")
        {
            SetWeapon(nadeLauncher);
        }

        // Check if we fired the weapon
        if (Input.GetButtonDown("Fire1"))
        {
            FireWeapon(currentWep);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (isGrappling)
            {
                myRigidbody.useGravity = true;
                canControl = true;
                isGrappling = false;
            }
        }

        inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // Cache the input
        if (Input.GetButtonDown("Jump"))
            jumpFlag = true;

    }

    void FixedUpdate()
    {
        // If we are in control of our character
        if (canControl)
        {
            // On the ground
            if (grounded)
            {
                // Apply a force that attempts to reach our target velocity
                var velocityChange = CalculateVelocityChange(inputVector);
                myRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

                // Jump
                if (canJump && jumpFlag)
                {
                    jumpFlag = false;
                    myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, myRigidbody.velocity.y + CalculateJumpVerticalSpeed(), myRigidbody.velocity.z);
                }

                // By setting the grounded to false in every FixedUpdate we avoid
                // checking if the character is not grounded on OnCollisionExit()
                grounded = false;
            }
            // In mid-air
            else
            {
                // Uses the input vector to affect the mid air direction
                var velocityChange = transform.TransformDirection(inputVector) * inAirControl;
                myRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
            }

        }
    }
    // Unparent if we are no longer standing on our parent
    void OnCollisionExit(Collision collision)
    {
        if (collision.transform == transform.parent)
            transform.parent = null;
    }
    
    // If there are collisions check if the character is grounded
    void OnCollisionStay(Collision col)
    {
        TrackGrounded(col);
    }

    void OnCollisionEnter(Collision col)
    {
        if (isGrappling)
        {
            isGrappling = false;
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.useGravity = true;

            canControl = true;
        }

        TrackGrounded(col);
    }

    // From the user input calculate using the set up speeds the velocity change
    private Vector3 CalculateVelocityChange(Vector3 inputVector)
    {
        // Calculate how fast we should be moving
        var relativeVelocity = transform.TransformDirection(inputVector);
        if (inputVector.z > 0)
        {
            relativeVelocity.z *= (canRun && Input.GetButton("Run")) ? runSpeed : walkSpeed;
        }
        else
        {
            relativeVelocity.z *= (canRun && Input.GetButton("Run")) ? runBackwardSpeed : walkBackwardSpeed;
        }
        relativeVelocity.x *= (canRunSidestep && Input.GetButton("Run")) ? runSidestepSpeed : sidestepSpeed;

        // Calcualte the delta velocity
        var currRelativeVelocity = myRigidbody.velocity - groundVelocity;
        var velocityChange = relativeVelocity - currRelativeVelocity;

        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        return velocityChange;
    }

    // From the jump height and gravity we deduce the upwards speed for the character to reach at the apex.
    private float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt(2f * jumpHeight * Mathf.Abs(Physics.gravity.y));
    }
    // Check if the base of the capsule is colliding to track if it's grounded
    private void TrackGrounded(Collision collision)
    {
        var maxHeight = capsule.bounds.min.y + .5f;
        foreach (var contact in collision.contacts)
        {
            if (contact.point.y < maxHeight)
            {
                if (isKinematic(collision))
                {
                    // Get the ground velocity and we parent to it
                    groundVelocity = collision.rigidbody.velocity;

                 
                    myTransform.parent = collision.transform;
                }
                else if (isStatic(collision))
                {
                    
                    // Just parent to it since it's static
                    myTransform.parent = collision.transform;
                }
                else
                {
                    
                    // We are standing over a dinamic object,
                    // set the groundVelocity to Zero to avoid jiggers and extreme accelerations
                    groundVelocity = Vector3.zero;
                }

                // Esta en el suelo
                grounded = true;
            }

            break;
        }
    }

    private bool isKinematic(Collision collision)
{
    return isKinematic(collision.transform);
}

    private bool isKinematic(Transform transform)
    {
        return transform.GetComponent<Rigidbody>() && transform.GetComponent<Rigidbody>().isKinematic;
    }

    private bool isStatic(Collision collision)
    {
        return isStatic(collision.transform);
    }

    private bool isStatic(Transform transform)
    {
        return transform.gameObject.isStatic;
    }

    void Grappling(Vector3 hitLocation)
    {
        // Check if we are supposed to be grappling, and if we haven't yet reached our target location
        if (isGrappling && myTransform.position != hitLocation)
        {
            // Zero out my velocity, disable character controls, and turn gravity off
            myRigidbody.velocity = Vector3.zero;
            canControl = false;
            myRigidbody.useGravity = false;

            // Dir is the direction that we should be traveling to reach the grapple point
            // Normalize the direction to prevent physics bugs, then add force in the movement direction
            Vector3 dir = hitLocation - myTransform.position;
            dir = dir.normalized;
            myRigidbody.AddForce(dir * 4000f);
        }

    }

    void SetWeapon(GameObject weapon)
    {
        myTransform.Find("Main Camera/" + currentWep).gameObject.SetActive(false);
        currentWep = weapon.name;
        
        myTransform.Find("Main Camera/" + currentWep).gameObject.SetActive(true);
    }

    void FireWeapon(string wepName)
    {
        if (wepName == "Grapple Bow")
        {
            // Perform a raycast in the direction of the camera, and if we hit set grappling to true
            // Calls Grappling() and passes the world position that was hit 
            RaycastHit hit;
            if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit))
            {
                if (hit.transform.tag != "WorldBounds")
                {
                    isGrappling = true;
                    Grappling(hit.point);
                }
            }
        }
        if (wepName == "Nade Launcher")
        {
            RaycastHit hit;
            if(Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit))
            {
                print(hit.transform.gameObject.name);
                var shotDir = hit.point - shotStart.position;
                shotDir = shotDir.normalized;
                var projectile = PhotonNetwork.Instantiate("NadeProjectile", shotStart.position, shotStart.rotation, 0);
                projectile.GetComponent<Rigidbody>().AddForce(shotDir * 2000f);
            }
            
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    IEnumerator Alive()
        {
            while (isAlive)
            {
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 5f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 5f);

                yield return null;
            }
        }
}
