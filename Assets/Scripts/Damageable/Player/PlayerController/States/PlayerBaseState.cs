using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void ExitState(InputsManager inputs);
    
    public abstract void EnterState(InputsManager inputs);

    public abstract void UpdateState(InputsManager inputs);

    public abstract void UpdatePhysics(InputsManager inputs);

    public abstract void OnCollisionEnter(InputsManager inputs);
}

