using UnityEngine;

public class PlayerStateMelee : PlayerBaseState
{
    public override void EnterState(InputsManager inputs){
        inputs.combatScript.meleeCombo = 1;
        inputs.animationScript.SetCombo(1);
    }

    public override void ExitState(InputsManager inputs){
        inputs.combatScript.ceaseMelee();
        inputs.animationScript.SetCombo(0);
    }

    public override void UpdateState(InputsManager inputs){
        Vector2 animationAxis = inputs.UpdateAnimationAxis();
        inputs.animationScript.SetMovementAxis(animationAxis);
        inputs.animationScript.ModifyAllParticles(animationAxis);
        inputs.animationScript.NotAiming();
        
        CheckInputs(inputs);
    }

    public override void UpdatePhysics(InputsManager inputs){
        Vector2 animationAxis = inputs.GetAnimationAxis();
        inputs.movementScript.TwoDimentionalMovement(animationAxis, 0.5f);
        inputs.movementScript.VerticalMovement(0.6f);
        inputs.movementScript.Displacement();
        inputs.movementScript.Rotation();

        inputs.cameraScript.FixedUpdateNormal(inputs.movementScript.speed);
        
        inputs.UIScript.UINotAim();
        inputs.UpdateUI();
    }

    private void CheckInputs(InputsManager inputs){

        if(inputs.combatScript.updateCombo){
            if(inputs.inputBuffered == InputsManager.Inputs.Attack){
                inputs.combatScript.ChangeAttack();
                inputs.animationScript.SetCombo(inputs.combatScript.meleeCombo);
            }
            else{
                inputs.SwitchState(inputs.movingState);
            }
            inputs.combatScript.UpdateCombo(false);
        }

        if(Input.GetButton("Jump")){
            inputs.movementScript.Jump();
        }
        if(inputs.inputBuffered == InputsManager.Inputs.Fly){
            inputs.SwitchState(inputs.flyingState);
            inputs.inputBuffered = InputsManager.Inputs.None;
        }
        if(inputs.attackHeld == InputsManager.Inputs.Guard){
            inputs.SwitchState(inputs.guardState);
        }
    }

    public override void OnCollisionEnter(InputsManager inputs){
        
    }
}
