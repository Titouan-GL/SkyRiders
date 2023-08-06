using UnityEngine;

public class PlayerStateAiming : PlayerBaseState
{
    public override void EnterState(InputsManager inputs){
        
    }

    public override void ExitState(InputsManager inputs){
        inputs.combatScript.ReloadAll();
    }

    public override void UpdateState(InputsManager inputs){
        Vector2 animationAxis = inputs.UpdateAnimationAxis();
        inputs.animationScript.SetMovementAxis(animationAxis);
        inputs.animationScript.ModifyAllParticles(animationAxis);
        inputs.animationScript.Aiming();
        
        CheckInputs(inputs);
    }

    public override void UpdatePhysics(InputsManager inputs){
        Vector2 animationAxis = inputs.GetAnimationAxis();
        inputs.movementScript.TwoDimentionalMovement(animationAxis, 0.5f);
        inputs.movementScript.VerticalMovement(0.6f);
        inputs.movementScript.IsOnGround();
        inputs.movementScript.Displacement();
        inputs.movementScript.Rotation();

        inputs.cameraScript.FixedUpdateAiming();

        inputs.UIScript.UIAim();
        inputs.UpdateUI();
    }

    private void CheckInputs(InputsManager inputs){
        if(!(inputs.inputHeld == InputsManager.Inputs.Aim) && inputs.combatScript.FinishedFiring()){
            inputs.SwitchState(inputs.movingState);
        }
        else{
            if(inputs.attackReleased == InputsManager.Inputs.Attack){
                inputs.combatScript.Fire1Release();
            }
            else if(inputs.attackReleased == InputsManager.Inputs.Guard){
                inputs.combatScript.Fire2Release();
            }

            if(inputs.attackHeld == InputsManager.Inputs.Attack){
                inputs.combatScript.Fire1Hold();
                inputs.UIScript.RocketUnAim();
            }
            else if(inputs.attackHeld == InputsManager.Inputs.Guard){
                inputs.combatScript.Fire2Hold();
            }
            else{
                inputs.UIScript.RocketUnAim();
            }

        }

        if(Input.GetButton("Jump")){
            inputs.movementScript.Jump();
        }
        else if(inputs.inputBuffered == InputsManager.Inputs.Fly && inputs.combatScript.FinishedFiring()){
            inputs.SwitchState(inputs.flyingState);
            inputs.inputBuffered = InputsManager.Inputs.None;
        }
    }

    public override void OnCollisionEnter(InputsManager inputs){
        
    }
}
