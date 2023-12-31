/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float maximumSpeed = 20f;
    private float maximumPlaneSpeed = 40f;
    public float speed;
    private float gravityScale = 10f;
    private float jumpSpeed = 50;
    private float ySpeed;
    Animator animator;
    private CharacterController characterController;
    [SerializeField] private LayerMask layerMaskLevel;

    [SerializeField] private MultiAimConstraint aimData;

    [SerializeField] private InputAndAnimations inputs;

    Vector3 velocity = new Vector3();

    [SerializeField]
    private Transform cameraTransform;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        aimData = GetComponentInChildren<MultiAimConstraint>();
    }

    void FixedUpdate(){
        
        //animation for gun because it's annoying in regular updates
        if(inputs.aiming){
            aimData.weight = Mathf.Lerp(aimData.weight, 1f, 0.3f);
            animator.SetLayerWeight(1, aimData.weight);
        }
        else{
            aimData.weight = Mathf.Lerp(aimData.weight, 0f, 0.3f);
            animator.SetLayerWeight(1, aimData.weight);
        }
        if(!inputs.flying){
            Vector3 movementDirection = new Vector3(inputs.horizontalInput, 0, inputs.verticalInput);
            float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

            speed = inputMagnitude * maximumSpeed;
            movementDirection.Normalize();
            movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;

            velocity = Vector3.Lerp(velocity, movementDirection * speed, 0.9f);
            if(inputs.guard || inputs.combo != 0 || inputs.aiming){
                velocity /= 2;
            }
            
            if(!inputs.sliding){
                ySpeed += Physics.gravity.y * Time.deltaTime * gravityScale;
            }
            else{//jumpsliding and gravity
                ySpeed += Physics.gravity.y * Time.deltaTime * (ySpeed < 0 ? gravityScale/3 : gravityScale);
                if(inputs.jump){
                    ySpeed = jumpSpeed;
                    inputs.jump = false;
                    inputs.sliding = false;
                }
            }

            RaycastHit hit;
            
            Debug.DrawRay(transform.position, transform.TransformDirection(-1*Vector3.up) * 3.1f, Color.cyan);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 2f, Color.blue);
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, 2f, layerMaskLevel)){
                if(ySpeed > 0){
                    ySpeed = 0.0f;
                }

            }
            if(Physics.Raycast(transform.position, transform.TransformDirection(-1*Vector3.up), out hit, 3.1f, layerMaskLevel))
            {
                inputs.sliding = false;
                if(ySpeed < 0){
                    ySpeed = 0.0f;
                }
                if(inputs.jump){
                    transform.position = new Vector3(transform.position.x, hit.point.y + 3.2f, transform.position.z);
                    ySpeed = jumpSpeed;
                    inputs.jump = false;
                }
            }
            else if (inputs.combo == 0){//if not on ground
                inputs.jump = false;
                //slide detection
                RaycastHit hit2;
                inputs.sliding = false;
                animator.SetFloat("slideX", 0);
                Debug.DrawRay(transform.position, transform.right * 2f, Color.white);
                if(Physics.Raycast(transform.position, transform.right, out hit2, 2f, layerMaskLevel)){
                    inputs.sliding = true;
                    animator.SetFloat("slideX", 1);
                    transform.rotation = Quaternion.LookRotation(hit2.normal)* Quaternion.Euler(0f, 90f, 0f);
                }
                Debug.DrawRay(transform.position, transform.right * -2f, Color.white);
                if(Physics.Raycast(transform.position, transform.right*-1, out hit2, 2f, layerMaskLevel)){
                    inputs.sliding = true;
                    animator.SetFloat("slideX", -1);
                    transform.rotation = Quaternion.LookRotation(hit2.normal)* Quaternion.Euler(0f, -90f, 0f);
                }
            }

            velocity.y = ySpeed;
            characterController.Move( velocity * Time.fixedDeltaTime); 

            Vector3 MechRotation = new Vector3 (0, cameraTransform.rotation.eulerAngles.y, 0);
            if(!inputs.sliding){
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(MechRotation), 0.1f);
            }
            
        }
        else{
            ySpeed = 0.0f;
            speed = Mathf.Lerp(speed, maximumPlaneSpeed, 0.1f);
            transform.rotation = Quaternion.Slerp(transform.rotation, cameraTransform.rotation, 0.5f);
            characterController.Move(transform.forward * speed * Time.fixedDeltaTime); 

            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward * 3f, Color.green);
            if(Physics.Raycast(transform.position, transform.forward, out hit, 3f, layerMaskLevel)){
                inputs.sliding = true;
                inputs.flying = false;
                
            }
        }
    }

    private void OnApplicationFocus(bool focus){
        if(focus){
            Cursor.lockState = CursorLockMode.Locked;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
*/