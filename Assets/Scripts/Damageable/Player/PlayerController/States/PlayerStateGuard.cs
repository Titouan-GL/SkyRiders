using UnityEngine;

public class PlayerStateGuard : PlayerBaseState
{
    public override void EnterState(InputsManager inputs){
        inputs.animationScript.SetGuard(true);
        inputs.combatScript.GuardEnter();
    }

    public override void ExitState(InputsManager inputs){
        inputs.animationScript.SetGuard(false);
        inputs.combatScript.GuardExit();
    }

    public override void UpdateState(InputsManager inputs){
        Vector2 animationAxis = inputs.UpdateAnimationAxis();
        inputs.animationScript.SetMovementAxis(animationAxis);
        inputs.animationScript.ModifyAllParticles(animationAxis);
        inputs.animationScript.Guard();
        inputs.animationScript.NotAiming();
        
        CheckInputs(inputs);
    }

    public override void UpdatePhysics(InputsManager inputs){
        Vector2 animationAxis = inputs.GetAnimationAxis();
        inputs.movementScript.TwoDimentionalMovement(animationAxis, 0.3f);
        inputs.movementScript.VerticalMovement(0.6f);
        inputs.movementScript.Displacement();
        inputs.movementScript.Rotation();

        inputs.cameraScript.FixedUpdateNormal(inputs.movementScript.speed);

        inputs.UIScript.UINotAim();
        inputs.UpdateUI();
        
    }

    private void CheckInputs(InputsManager inputs){
        if(Input.GetButton("Jump")){
            inputs.movementScript.Jump();
        }
        if(!(inputs.attackHeld == InputsManager.Inputs.Guard)){
            inputs.SwitchState(inputs.movingState);
        }
    }

    public override void OnCollisionEnter(InputsManager inputs){
        
    }
}
