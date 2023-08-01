using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMechMovement : EnemyMovement
{
    [SerializeField] protected Vector2 randomStrafeTime;
    [SerializeField] protected Vector2 randomJumpInterval;
    [SerializeField] protected float jumpSpeed;


    protected Vector2 strafe; //direction of strafe
    protected Vector2 nextStrafe; //strafe towards which we lerp
    protected float strafeTime = 0f; // time before changing the strafing
    protected float jumpIntervalCurrent = 10f; //time before jumping again
    protected float strafeDistanceMax = 10f;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        RandomizeJumpTime();
    }

    void RandomizeJumpTime(){
        jumpIntervalCurrent = Random.Range(randomJumpInterval.x, randomJumpInterval.y);
    }

    public void UpdateStrafing(){
        if(strafeTime > 0){
            strafeTime -= Time.fixedDeltaTime;
            strafe = Vector2.Lerp(strafe, nextStrafe, 0.1f);
        }
        else{
            strafeTime = Random.Range(randomStrafeTime.x, randomStrafeTime.y);
            nextStrafe = new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 1f)).normalized * strafeDistanceMax;
        }
    }

    public void TestForStrafing(){
        float groundDetectionRange = 10f;
        RaycastHit hit;
        if(navMeshAgent.baseOffset <= baseNavMeshOffset){
            Debug.DrawRay(transform.position, (new Vector3(0f, -1f, 0f) + transform.forward) * groundDetectionRange, Color.red);
            if(!Physics.Raycast(transform.position, new Vector3(0f, -1f, 0f) + transform.forward, out hit, groundDetectionRange, layerMaskLevel)){
                if(strafe.y > 0){
                    strafe.y = 0;
                    nextStrafe.y = 0;
                }
            }
            Debug.DrawRay(transform.position, (new Vector3(0f, -1f, 0f) + transform.right) * groundDetectionRange, Color.red);
            if(!Physics.Raycast(transform.position, new Vector3(0f, -1f, 0f) + transform.right, out hit, groundDetectionRange, layerMaskLevel)){
                if(strafe.x > 0){
                    strafe.x = 0;
                    nextStrafe.x = 0;
                }
            }
            Debug.DrawRay(transform.position, (new Vector3(0f, -1f, 0f) + -transform.right) * groundDetectionRange, Color.red);
            if(!Physics.Raycast(transform.position, new Vector3(0f, -1f, 0f) - transform.right, out hit, groundDetectionRange, layerMaskLevel)){
                if(strafe.x < 0){
                    strafe.x = 0;
                    nextStrafe.x = 0;
                }
            }
        }
    }

    public void TryJumping(){
        if(jumpIntervalCurrent <= 0){
            ySpeed = jumpSpeed;
            RandomizeJumpTime();
        }
        else{
            jumpIntervalCurrent -= Time.fixedDeltaTime;
        }
    }

    public void Strafing(){
        transform.position += transform.right * strafe.x * Time.fixedDeltaTime;
        transform.position += transform.forward * strafe.y * Time.fixedDeltaTime;
    }

    public void RotateTowardsTarget(){
        Vector3 direction = goToTarget.position - transform.position;
        direction.y = 0f;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.05f);
    }

    public void UpdateTarget(){
        goToTarget.position = Vector3.Lerp(goToTarget.position, playerTransform.position, 0.05f);
    }

}
