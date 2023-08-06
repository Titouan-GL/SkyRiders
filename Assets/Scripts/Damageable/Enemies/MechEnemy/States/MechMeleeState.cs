using UnityEngine;

public class MechMeleeState : MechBaseState
{
    public override void EnterState(MechStateManager mech){
        mech.movement.NoNavMeshDestination();
        mech.combat.UpdateCombo();
        mech.combat.SetAiming(false);
    }

    public override void UpdateState(MechStateManager mech){
        mech.UpdateAnimations();
        mech.animationScript.SetComboInteger(mech.combat.meleeCombo);
    }

    public override void UpdatePhysics(MechStateManager mech){
        mech.movement.UpdateTarget();
        if(mech.combat.updateCombo){
            if(mech.playerDistance > mech.meleeDistance || !mech.movement.isInRangeOfMelee()){
                mech.SwitchState(mech.distanceState);
            }
            else{
                mech.combat.ChangeAttack();
            }
            mech.combat.updateCombo = false;
        }
    }

    public override void OnCollisionEnter(MechStateManager mech){
        
    }
    
    public override void ExitState(MechStateManager mech){
        mech.combat.ceaseMelee();
        mech.animationScript.SetComboInteger(0);
    }
}

