using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelikopterStateManager : MonoBehaviour
{
    [SerializeField]public float loseSightDistance;
    [SerializeField]public float sightDistance;
    [SerializeField]public float fireDistance;
    [SerializeField]public float meleeDistance;
    [SerializeField]public float stopForward;

    protected HelikopterBaseState currentState;

    [HideInInspector]public HelikopterDistanceState distanceState = new HelikopterDistanceState();
    [HideInInspector]public HelikopterIdleState idleState = new HelikopterIdleState();
    [HideInInspector]public HelikopterMeleeState meleeState = new HelikopterMeleeState();
    
    [HideInInspector]public HelikopterMovement movement;
    [HideInInspector]public HelikopterAnimation animationScript;
    [HideInInspector]public HelikopterCombat combat;

    [HideInInspector]public float playerDistance;
    [HideInInspector]public AnimatorStateInfo asi;


    void Awake()
    {
        movement = GetComponent<HelikopterMovement>();
        animationScript = GetComponent<HelikopterAnimation>();
        combat = GetComponent<HelikopterCombat>();
        currentState = idleState;
    }

    void Start(){
        currentState.EnterState(this);
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

    public void SwitchState(HelikopterBaseState state){
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }
}
