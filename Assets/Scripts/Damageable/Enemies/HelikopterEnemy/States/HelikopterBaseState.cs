using UnityEngine;

public abstract class HelikopterBaseState
{
    public abstract void ExitState(HelikopterStateManager heliko);
    
    public abstract void EnterState(HelikopterStateManager heliko);

    public abstract void UpdateState(HelikopterStateManager heliko);

    public abstract void UpdatePhysics(HelikopterStateManager heliko);

    public abstract void OnCollisionEnter(HelikopterStateManager heliko);
}
