using UnityEngine;

public class PlayerStateFlying : PlayerBaseState
{
    public override void ExitState(InputsManager inputs){
        inputs.animationScript.SetFlying(false);
    }
    
    public override void EnterState(InputsManager inputs){
        inputs.animationScript.SetFlying(true);
    }

    public override void UpdateState(InputsManager inputs){
        Vector2 animationAxis = inputs.UpdateAnimationAxis();
        inputs.animationScript.SetMovementAxis(animationAxis);
        inputs.animationScript.ModifyAllParticles();
        inputs.animationScript.NotAiming();

        CheckInputs(inputs);
    }

    public override void UpdatePhysics(InputsManager inputs){
        inputs.movementScript.Flight();
        inputs.cameraScript.FixedUpdatePlane(inputs.movementScript.speed);

        inputs.UIScript.UIAim();
        inputs.UpdateUI();

    }

    private void CheckInputs(InputsManager inputs){
        if(inputs.attackHeld == InputsManager.Inputs.Guard){
            inputs.SwitchState(inputs.guardState);
        }
        else if(inputs.inputBuffered == InputsManager.Inputs.Fly){
            inputs.SwitchState(inputs.inAirState);
            inputs.inputBuffered = InputsManager.Inputs.None;
        }
        else if(inputs.movementScript.PlaneRaycast()){
            inputs.SwitchState(inputs.inAirState);
        }
        else if(inputs.attackHeld == InputsManager.Inputs.Attack){
            inputs.combatScript.PlaneFire1Hold();
        }
    }

    public override void OnCollisionEnter(InputsManager inputs){
        
    }
}
