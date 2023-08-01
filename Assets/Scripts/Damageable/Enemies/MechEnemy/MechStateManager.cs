using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechStateManager : MonoBehaviour
{
    [SerializeField]public float loseSightDistance;
    [SerializeField]public float sightDistance;
    [SerializeField]public float fireDistance;
    [SerializeField]public float meleeDistance;
    [SerializeField]public float stopForward;
    [SerializeField]public float fleeDistance;
    
    MechBaseState currentState;
    [HideInInspector]public MechDistanceState distanceState = new MechDistanceState();
    [HideInInspector]public MechIdleState idleState = new MechIdleState();
    [HideInInspector]public MechMeleeState meleeState = new MechMeleeState();

    [HideInInspector]public EnemyMechMovement movement;
    [HideInInspector]public EnemyMechAnimation animationScript;
    [HideInInspector]public EnemyMechCombat combat;

    [HideInInspector]public float playerDistance;
    [HideInInspector]public AnimatorStateInfo asi;
    
    //methods called in multiple states
    public void UpdateAnimations(){
        asi = animationScript.GetAnimatorStateInfo();
        movement.ApplyGravity();
        animationScript.UpdateAnimationAxis(movement.positionMoved);
        animationScript.SetAnimationAxis();
        animationScript.UpdateParticles();
        animationScript.Aiming(combat.aiming);
    }

    void Awake()
    {
        movement = GetComponent<EnemyMechMovement>();
        animationScript = GetComponent<EnemyMechAnimation>();
        combat = GetComponent<EnemyMechCombat>();
        currentState = idleState;
    }

    void Start(){
        currentState.EnterState(this);
        animationScript.SetAimTarget(movement.goToTarget);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    void FixedUpdate()
    {
        playerDistance = movement.GetPlayerDistance();
        currentState.UpdatePhysics(this);
    }

    public void SwitchState(MechBaseState state){
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }
}
