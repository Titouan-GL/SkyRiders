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
        
    }

    private void CheckInputs(InputsManager inputs){
        bool OnGround = inputs.movementScript.IsOnGround();
        if(inputs.inputBuffered == InputsManager.Inputs.Jump && OnGround){
            inputs.movementScript.Jump();
            inputs.inputBuffered = InputsManager.Inputs.None;
        }
        if(!(inputs.attackHeld == InputsManager.Inputs.Guard)){
            inputs.SwitchState(inputs.movingState);
        }
    }

    public override void OnCollisionEnter(InputsManager inputs){
        
    }
}
