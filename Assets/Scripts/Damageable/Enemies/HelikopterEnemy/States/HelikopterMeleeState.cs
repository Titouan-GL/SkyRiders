using UnityEngine;

public class HelikopterMeleeState : HelikopterBaseState
{
    public override void EnterState(HelikopterStateManager heliko){
    }

    public override void UpdateState(HelikopterStateManager heliko){
    }

    public override void UpdatePhysics(HelikopterStateManager heliko){
        
        heliko.movement.UpdateAimTarget();
        heliko.movement.RotateTowardsTarget();
        heliko.combat.TryFire();
        heliko.movement.UpdateHeight();
        heliko.movement.SetNavMeshDestination(heliko.movement.goToTarget.position);
        heliko.movement.Flee();
        if(heliko.playerDistance > heliko.meleeDistance){
            heliko.SwitchState(heliko.distanceState);
        }
        
    }

    public override void OnCollisionEnter(HelikopterStateManager heliko){
        
    }
    
    public override void ExitState(HelikopterStateManager heliko){
    }
}

