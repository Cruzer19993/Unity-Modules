
//Author: Jakub "Cruzer" Woźniak

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class FPS_Player: MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField]
    float movementSpeed;    
    [Tooltip("0 = Forwards, 1 = Backwards, 2 = Left, 3 = Right")]
    [SerializeField]
     KeyCode[] movementKeys = new KeyCode[4];
     [SerializeField]
     KeyCode jumpKey = KeyCode.Space;
     [SerializeField]
     float jumpForce = 50f;
     [SerializeField]
     float fallMultiplier = 0.5f;

    [Header("FPS Camera settings")]
    [SerializeField]
    Vector3 cameraPositionInPlayer;
    [SerializeField]
    float sensitivity = 5f;
    [SerializeField]
    float maxPitch = 60f;
    [SerializeField]
    float minPitch = 20f;
    [SerializeField]
    bool useRawAxis = false;


    Collider col;
    Rigidbody rb;
    Camera cam; //Main Camera referenced
    float pitchOffset = 0f;
    bool inJump = false;

    void Start()
    {
        
        //check if keys are set.
        foreach(KeyCode x in movementKeys){
            if(x == KeyCode.None) Debug.LogError("FPS_Player: movementKey/s are not set up");
        }
        //grab reference to the main camera
        cam = Camera.main;

        //grab reference to the rigidbody
        rb = gameObject.GetComponent<Rigidbody>();

        //grab reference to the collider
        col = gameObject.GetComponent<Collider>();

        if(!cam.transform.IsChildOf(gameObject.transform)){ //check if camera is a child of FPS_Player gameobject body.
            cam.transform.SetParent(gameObject.transform);
            cam.transform.position = cameraPositionInPlayer;
        }
        //lock the cursor to the centre of screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        sensitivity *= 10; //Make the sensitivity actually usable.

    }

    void Update()
    {
        /*
               ** Mouse Movement vs axis **

                UP&DOWN gives -1 or 1 on Mouse Y axis
                LEFT&RIGHT gives -1 or 1 on Mouse X axis
                in case of FPS camera we want to map "Mouse Y" axis to X rotation in unity and "Mouse X" axis to Y rotation in unity.

            ** Movement keycodes array **
            0 = forwards, 1 = backwards, 2 = left, 3 = right
        */

        float MX = useRawAxis ? Input.GetAxisRaw("Mouse X") : Input.GetAxis("Mouse X"); //get the axis'
        float MY = useRawAxis ? Input.GetAxisRaw("Mouse Y") : Input.GetAxis("Mouse Y");

        pitchOffset += -MY * sensitivity * Time.deltaTime; //apply to the offset which is acutally the x rotation of the camera
        pitchOffset = Mathf.Clamp(pitchOffset,-minPitch,maxPitch); //clamp the x rotation.

        //rotate the player along Y axis and apply X rotation to the camera.
        transform.Rotate(new Vector3(0f,MX * sensitivity * Time.deltaTime,0f));
        cam.transform.rotation = Quaternion.Euler(pitchOffset,transform.eulerAngles.y,cam.transform.rotation.z); //rotate along X axis, set Y axis to match player body and leave the z intact

        //Detect keystrokes
        if(Input.GetKey(movementKeys[0])){
            Move(transform.forward);
        }
        if(Input.GetKey(movementKeys[1])){
            Move(-transform.forward);
        }
        if(Input.GetKey(movementKeys[2])){
            Move(-transform.right);
        }
        if(Input.GetKey(movementKeys[3])){
            Move(transform.right);
        }
        if(Input.GetKeyDown(jumpKey) && Mathf.Abs(rb.velocity.y) < 0.01f){
            Jump();
        }

    }

    void FixedUpdate(){
        if(rb.velocity.y < 0){
            rb.velocity += Physics.gravity * fallMultiplier;

        }
    }

    void Move(Vector3 direction){
        transform.Translate(direction * movementSpeed * Time.deltaTime,Space.World);
    }

    void Jump(){
        rb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
    }
}
