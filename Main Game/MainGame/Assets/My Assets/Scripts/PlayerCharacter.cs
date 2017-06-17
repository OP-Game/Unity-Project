using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CharacterController))]
public class PlayerCharacter : Photon.MonoBehaviour {


    #region Network Variables
    public Camera myCamera;                             //the Camera attached to this player
    private Vector3 position;                           //the position moving to, used for movement lerping via network
    private Quaternion rotation;                        //target rotation, for rotation lerping via network
    bool isAlive = true;                                //checks to see if this player is active in the network
    #endregion

    #region Movement Variables

    float walkSpeed = 8;
    float runSpeed = 15;
    float jumpSpeed = 8;
    float gravity = 5f;

    private float inputX, inputZ;

    private float refVelocity = 0f;
    private float currentSpeedX, currentSpeedZ;
    private float speedX, speedY, speedZ;

    private CharacterController myController;       // The Character Controller attached to this game object

    private Transform myTransform;          // The transform of this game object

    private Rigidbody myRigidbody;          // The rigidbody of this character, used for enabling physics

    private Vector3 moveDirection = Vector3.zero;       // The target movement direction, set to zero at start to prevent auto-moving
    private Vector3 jumpDirection = Vector3.zero;

    #endregion


    // Use this for initialization
    void Start () {
        // if the photonview on this object belongs to my player character
        if (photonView.isMine)
        {
            // enable my camera, get my Character Controller, and get my transform
            myCamera.enabled = true;

            myController = this.GetComponent<CharacterController>();
            myTransform = this.transform;

        }
        else
        {
            StartCoroutine(Alive());
        }
	}

    private void FixedUpdate()
    {

    }


    // Update is called once per frame
    void Update()
    {

        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        bool running = Input.GetButton("Run");

        if (myController.isGrounded)
        {
            speedY = 0f;

            if (!running)
            {
                speedX = Mathf.SmoothDamp(currentSpeedX, walkSpeed, ref refVelocity, .1f);
                speedZ = Mathf.SmoothDamp(currentSpeedZ, walkSpeed, ref refVelocity, .1f);

                moveDirection.x = speedX * inputX;
                moveDirection.z = speedZ * inputZ;

                //moveDirection = myTransform.TransformDirection(moveDirection);
                
                currentSpeedX = speedX;
                currentSpeedZ = speedZ;
            }
            if (running)
            {
                speedX = Mathf.SmoothDamp(currentSpeedX, runSpeed, ref refVelocity, .1f);
                speedZ = Mathf.SmoothDamp(currentSpeedZ, runSpeed, ref refVelocity, .1f);

                moveDirection.x = speedX * inputX;
                moveDirection.z = speedZ * inputZ;

                //moveDirection = myTransform.TransformDirection(moveDirection);

                currentSpeedX = speedX;
                currentSpeedZ = speedZ;

            }

           

            if (Input.GetButtonDown("Jump"))
            {
                speedY = jumpSpeed;
                jumpDirection = moveDirection;
            }

            moveDirection = myTransform.TransformDirection(moveDirection);

        }

        else
        {
            //speedX = Mathf.SmoothDamp(currentSpeedX, 1, ref refVelocity, 3f);
            //speedZ = Mathf.SmoothDamp(currentSpeedZ, 3f, ref refVelocity, 3f);

            moveDirection.z = jumpDirection.z;
            moveDirection.x = jumpDirection.x;

            moveDirection.z = moveDirection.z + (inputZ * 3);
            moveDirection.x = moveDirection.x + (inputX * 3);

            moveDirection = myTransform.TransformDirection(moveDirection);

            //currentSpeedX = speedX;
            //currentSpeedZ = speedZ;

            speedY -= gravity * Time.deltaTime;
        }

        moveDirection.y = speedY - .75f;
        myController.Move(moveDirection * Time.deltaTime);

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
