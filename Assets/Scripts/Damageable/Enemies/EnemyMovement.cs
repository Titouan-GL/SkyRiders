using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement values")]
    [SerializeField] protected float gravityScale;
    [SerializeField] protected Vector2 navMeshSpeedRandomRange;
    [SerializeField] public Transform goToTarget;


    [Header("NavMesh")]
    [SerializeField] protected List<Transform> pathFollowTransforms;


    protected UnityEngine.AI.NavMeshAgent navMeshAgent;
    protected UtilitiesNonStatic UNS;
    protected bool navMeshControl;
    protected Transform playerTransform;
    protected LayerMask layerMaskLevel;

    protected float ySpeed;
    [SerializeField] protected float fleeingMagnitude;
    protected float baseNavMeshOffset;
    protected Vector3 addedVelocity;

    [HideInInspector]public Vector3 positionMoved;
    protected Vector3 previousPosition;

    protected int pathIndex;
    
    public virtual void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        UNS = UtilitiesStatic.GetUNS();
        navMeshAgent.speed = Random.Range(navMeshSpeedRandomRange.x, navMeshSpeedRandomRange.y);
        baseNavMeshOffset = navMeshAgent.baseOffset;
        playerTransform = UNS.player.transform;
        layerMaskLevel = UNS.layerMaskLevel;
    }

    public void UpdatePathTarget(){
        if((pathFollowTransforms[pathIndex].position - transform.position).magnitude < 15f){
            pathIndex += 1;
            if(pathIndex >= pathFollowTransforms.Count){
                pathIndex = 0;
            }
        }
    }

    public Vector3 GetPatrolLocation(){
        return pathFollowTransforms[pathIndex].position;
    }

    public void RotateTowardsTargetPatrol(){
        Vector3 direction = pathFollowTransforms[pathIndex].position - transform.position;
        direction.y = 0f;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.05f);
    }

    public void Start(){
        goToTarget.transform.SetParent(null);
    }

    public virtual void FixedUpdate(){
        positionMoved = previousPosition - transform.position;
        previousPosition = transform.position;
    }

    public virtual void SetNavMeshDestination(Vector3 destination){
        navMeshAgent.destination = destination;
    }

    public virtual void NoNavMeshDestination(){
        transform.position = transform.position;
    }

    public void ApplyGravity(){
        ySpeed += Physics.gravity.y * Time.deltaTime * gravityScale;
        navMeshAgent.baseOffset += ySpeed * Time.fixedDeltaTime;

        if(navMeshAgent.baseOffset < baseNavMeshOffset)
        {
            navMeshAgent.baseOffset = baseNavMeshOffset;
            ySpeed = 0;
        }
    }

    public void Flee(){
        float lerpSpeed = 0.2f;
        Vector3 direction = transform.position - playerTransform.position;
        direction.y = 0;
        goToTarget.position = Vector3.Lerp(goToTarget.position, playerTransform.position + direction.normalized * fleeingMagnitude, lerpSpeed);
    }

    public float GetPlayerDistance(){
        return Vector3.Distance(transform.position, playerTransform.position);
    }

    public bool IsInAir(){
        return navMeshAgent.baseOffset > baseNavMeshOffset;
    }

    public float clampRotation(float rotation){
        if(rotation > 180){
            return rotation - 360;
        }
        else{
            return rotation;
        }
    }

    public Vector3 GetRotationToTarget(){
        Vector3 direction = goToTarget.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        return targetRotation.eulerAngles - transform.eulerAngles;
    }

    public bool CanDetectPlayer(){
        RaycastHit hit;
        Vector3 raycastDirection = playerTransform.position - transform.position;
        Debug.DrawRay(transform.position, raycastDirection, Color.magenta);
        if(!Physics.Raycast(transform.position, raycastDirection.normalized, out hit, raycastDirection.magnitude, UNS.layerMaskLevel)){
            return true;
        }
        else{
            return false;
        }
    }

}
