using UnityEngine;

public class HelikopterIdleState : HelikopterBaseState
{
    public override void EnterState(HelikopterStateManager heliko){
        heliko.movement.NoNavMeshDestination();
    }

    public override void UpdateState(HelikopterStateManager heliko){
    }

    public override void UpdatePhysics(HelikopterStateManager heliko){
        heliko.movement.SetNavMeshDestination(heliko.movement.GetPatrolLocation());
        heliko.movement.RotateTowardsTargetPatrol();
        heliko.movement.UpdatePathTarget();
        heliko.movement.UpdateHeight();
        heliko.movement.UpdateAimTarget();
        if(heliko.movement.CanDetectPlayer() && heliko.playerDistance < heliko.sightDistance){
            heliko.SwitchState(heliko.distanceState);
        }
    }

    public override void OnCollisionEnter(HelikopterStateManager heliko){
        
    }

    public override void ExitState(HelikopterStateManager heliko){

    }
}