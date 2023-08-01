using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelikopterMovement : EnemyMovement
{

    [SerializeField] public Transform aimTowardsTarget;
    [SerializeField] public Transform rotatingObject;

    [SerializeField] private Vector2 clampedRotation;
    [SerializeField] private float MaxVerticalSpeed;
    private float currentVerticalSpeed;
    private float targetRotation;

    private float currentHeight;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        currentHeight = navMeshAgent.baseOffset;
        targetRotation = (clampedRotation.x + clampedRotation.y)/2;
    }

    public void UpdateHeight(){
        navMeshAgent.baseOffset = currentHeight;
    }

    public void RotateTowardsTarget(){
        Vector3 direction = aimTowardsTarget.position - rotatingObject.position;
        Quaternion nextRotation = Quaternion.Slerp(rotatingObject.rotation, Quaternion.LookRotation(direction), 0.2f);
        float xRotation = clampRotation(nextRotation.eulerAngles.x);
        
        if(xRotation < clampedRotation.y && xRotation > clampedRotation.x){
            rotatingObject.rotation = nextRotation;
        }
        else{
            rotatingObject.rotation = Quaternion.Euler(rotatingObject.rotation.eulerAngles.x, nextRotation.eulerAngles.y, nextRotation.eulerAngles.z);
        }
        
        if(xRotation < targetRotation-5){
            currentVerticalSpeed = Mathf.Lerp(currentVerticalSpeed, MaxVerticalSpeed, 0.03f);
        }
        else if(xRotation > targetRotation+5){
            currentVerticalSpeed = Mathf.Lerp(currentVerticalSpeed, -MaxVerticalSpeed, 0.03f);
        }
        else{
            currentVerticalSpeed = Mathf.Lerp(currentVerticalSpeed, 0f, 0.03f);
        }
        currentHeight += currentVerticalSpeed*Time.deltaTime;
    }

    public void UpdateTarget(){
        goToTarget.position = Vector3.Lerp(goToTarget.position, playerTransform.position, 0.05f);
    }
    
    public void UpdateAimTarget(){
        aimTowardsTarget.position = Vector3.Lerp(aimTowardsTarget.position, playerTransform.position, 0.3f);
    }

}
