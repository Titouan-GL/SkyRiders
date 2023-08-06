using UnityEngine;

public class MechIdleState : MechBaseState
{
    public override void EnterState(MechStateManager mech){
        mech.movement.NoNavMeshDestination();
    }

    public override void UpdateState(MechStateManager mech){
        mech.UpdateAnimations();
    }

    public override void UpdatePhysics(MechStateManager mech){
        mech.movement.SetNavMeshDestination(mech.movement.GetPatrolLocation());
        Debug.Log(mech.movement.GetPatrolLocation());
        mech.movement.RotateTowardsTargetPatrol();
        mech.movement.UpdatePathTarget();
        Debug.Log(mech.playerDistance);
        if(mech.movement.CanDetectPlayer() && mech.playerDistance < mech.sightDistance){
            mech.SwitchState(mech.distanceState);
        }
    }

    public override void OnCollisionEnter(MechStateManager mech){
        
    }

    public override void ExitState(MechStateManager mech){

    }
}