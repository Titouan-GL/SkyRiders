using UnityEngine;

public class HelikopterIdleState : HelikopterBaseState
{
    public override void EnterState(HelikopterStateManager heliko){
        heliko.movement.NoNavMeshDestination();
    }

    public override void UpdateState(HelikopterStateManager heliko){
    }

    public override void UpdatePhysics(HelikopterStateManager heliko){
        heliko.combat.TryFire();
        heliko.movement.UpdateHeight();
        heliko.movement.UpdateAimTarget();
        if(heliko.playerDistance < heliko.sightDistance){
            heliko.SwitchState(heliko.distanceState);
        }
    }

    public override void OnCollisionEnter(HelikopterStateManager heliko){
        
    }

    public override void ExitState(HelikopterStateManager heliko){

    }
}