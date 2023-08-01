using UnityEngine;

public class MechDistanceState : MechBaseState
{
    public override void EnterState(MechStateManager mech){
        mech.combat.SetAiming(true);
    }

    public override void UpdateState(MechStateManager mech){

        mech.UpdateAnimations();
        mech.animationScript.DetectSliding(mech.movement.IsInAir());
    }

    public override void UpdatePhysics(MechStateManager mech){
        
        mech.movement.SetNavMeshDestination(mech.movement.goToTarget.position);
        mech.movement.RotateTowardsTarget();
        mech.movement.UpdateTarget();
        mech.movement.TryJumping();
        mech.movement.UpdateStrafing();
        mech.movement.Strafing();
        mech.movement.TestForStrafing();

        mech.combat.TestAiming();
        mech.combat.TryFire();

        if(mech.playerDistance < mech.meleeDistance){
            mech.SwitchState(mech.meleeState);
        }
        if(mech.playerDistance > mech.sightDistance){
            mech.SwitchState(mech.idleState);
        }
    }

    public override void OnCollisionEnter(MechStateManager mech){
        
    }
    
    public override void ExitState(MechStateManager mech){
        

    }
}
