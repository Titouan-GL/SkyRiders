using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float maximumSpeed = 20f;
    private float maximumPlaneSpeed = 40f;
    [HideInInspector]public float speed;
    private float gravityScale = 10f;
    private float jumpSpeed = 50;
    private CharacterController characterController;
    [HideInInspector] public bool OnGround;
    private LayerMask layerMaskLevel;
    private UtilitiesNonStatic UNS;

    [HideInInspector] private InputsManager inputs;

    Vector3 velocity = new Vector3();

    [SerializeField]
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputs = GetComponent<InputsManager>();
        UNS = UtilitiesStatic.GetUNS();
    }

    public void TwoDimentionalMovement(Vector2 movementVector, float multiplier = 1){
        /*
        Handles the movement two dimentional movement regardless of other factor, and modifies velocity
        */
        Vector3 movementDirection = new Vector3(movementVector.x, 0, movementVector.y);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        speed = inputMagnitude * maximumSpeed;
        movementDirection.Normalize();
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;

        velocity.x = Mathf.Lerp(velocity.x, movementDirection.x * speed, 0.9f) * multiplier;
        velocity.z = Mathf.Lerp(velocity.z, movementDirection.z * speed, 0.9f) * multiplier;
        //velocity = Vector3.Lerp(velocity, movementDirection * speed, 0.9f);
        
    }

    public void Jump(){
        velocity.y = jumpSpeed;
    }

    public void VerticalMovement(float multiplier = 1f){
        /*
        Handles the basic physics of InAir movement, like gravity or having a roof over the head when jumping
        */
        velocity.y += Physics.gravity.y * Time.deltaTime * (velocity.y < 0 ? gravityScale*multiplier : gravityScale);
        
        RaycastHit hit;
        
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 2f, Color.blue);
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, 2f, UNS.layerMaskLevel)){//test if upcollision
            if(velocity.y > 0){
                velocity.y = 0.0f;
            }

        }
    }

    public bool IsOnGround(){
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(-1*Vector3.up) * 3.1f, Color.cyan);
        if(Physics.Raycast(transform.position, transform.TransformDirection(-1*Vector3.up), out hit, 3.1f, UNS.layerMaskLevel))//test if grounded
        {
            OnGround = true;
            transform.position = new Vector3(transform.position.x, hit.point.y + 3f, transform.position.z);
            if(velocity.y < 0){
                velocity.y = 0.0f;
            }
        }
        else{
            OnGround = false;
        }
        return OnGround;
    }

    public void Displacement(){
        /*
        actually moves the  by applying it's velocity
        */
        characterController.Move( velocity * Time.fixedDeltaTime); 
    }

    public void Rotation(){
        /*
        actually rotates the  by having it look where the camera looks
        */
        Vector3 MechRotation = new Vector3 (0, cameraTransform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(MechRotation), 0.1f);
        
    }

    public void Flight(){
        velocity.y = 0.0f;
        speed = Mathf.Lerp(speed, maximumPlaneSpeed, 0.1f);
        transform.rotation = Quaternion.Slerp(transform.rotation, cameraTransform.rotation, 0.5f);
        characterController.Move(transform.forward * speed * Time.fixedDeltaTime); 
    }

    public bool PlaneRaycast(){
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 3f, Color.green);
        if(Physics.Raycast(transform.position, transform.forward, out hit, 3f, UNS.layerMaskLevel)){
            return true;
        }
        else{
            return false;
        }
    }
}
