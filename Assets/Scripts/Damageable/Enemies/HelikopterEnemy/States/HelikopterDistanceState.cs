using UnityEngine;

public class HelikopterDistanceState : HelikopterBaseState
{
    public override void EnterState(HelikopterStateManager heliko){
    }

    public override void UpdateState(HelikopterStateManager heliko){
    }

    public override void UpdatePhysics(HelikopterStateManager heliko){
        
        heliko.movement.SetNavMeshDestination(heliko.movement.goToTarget.position);
        heliko.movement.UpdateAimTarget();
        heliko.movement.RotateTowardsTarget();
        heliko.movement.UpdateTarget();
        heliko.movement.UpdateHeight();

        heliko.combat.TryFire();

        if(heliko.playerDistance < heliko.meleeDistance){
            heliko.SwitchState(heliko.meleeState);
        }
        if(heliko.playerDistance > heliko.sightDistance){
            heliko.SwitchState(heliko.idleState);
        }
    }

    public override void OnCollisionEnter(HelikopterStateManager heliko){
        
    }
    
    public override void ExitState(HelikopterStateManager heliko){
        

    }
}
