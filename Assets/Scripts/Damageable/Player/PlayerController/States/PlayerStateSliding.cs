using UnityEngine;

public class PlayerStateSliding : PlayerBaseState
{
    public override void ExitState(InputsManager inputs){
        inputs.animationScript.StopSliding();
    }
    
    public override void EnterState(InputsManager inputs){

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
        inputs.movementScript.TwoDimentionalMovement(animationAxis);
        inputs.movementScript.VerticalMovement(0.3f);
        inputs.movementScript.Displacement();
        inputs.movementScript.Rotation();

        bool onGround = inputs.movementScript.IsOnGround();
        bool isSliding = inputs.animationScript.Sliding();

        if(onGround){
            inputs.SwitchState(inputs.movingState);
        }
        else if(!isSliding){
            inputs.SwitchState(inputs.inAirState);
        }

        inputs.cameraScript.FixedUpdateNormal(inputs.movementScript.speed);
        
        inputs.UIScript.UINotAim();
    }

    private void CheckInputs(InputsManager inputs){
        if(inputs.inputBuffered == InputsManager.Inputs.Jump){
            inputs.movementScript.Jump();
            inputs.inputBuffered = InputsManager.Inputs.None;
        }

        if(inputs.attackHeld == InputsManager.Inputs.Guard){
            inputs.SwitchState(inputs.guardState);
        }
        else if(inputs.inputBuffered == InputsManager.Inputs.Fly){
            inputs.SwitchState(inputs.flyingState);
            inputs.inputBuffered = InputsManager.Inputs.None;
        }
        else if(inputs.inputHeld == InputsManager.Inputs.Aim){
            inputs.SwitchState(inputs.aimState);
        }
        else if(inputs.inputBuffered == InputsManager.Inputs.Attack){
            inputs.SwitchState(inputs.meleeState);
            inputs.inputBuffered = InputsManager.Inputs.None;
        }
    }

    public override void OnCollisionEnter(InputsManager inputs){
        
    }
}
