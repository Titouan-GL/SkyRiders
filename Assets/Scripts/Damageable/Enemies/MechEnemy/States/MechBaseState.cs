using UnityEngine;

public abstract class MechBaseState
{
    public abstract void ExitState(MechStateManager mech);
    
    public abstract void EnterState(MechStateManager mech);

    public abstract void UpdateState(MechStateManager mech);

    public abstract void UpdatePhysics(MechStateManager mech);

    public abstract void OnCollisionEnter(MechStateManager mech);
}
