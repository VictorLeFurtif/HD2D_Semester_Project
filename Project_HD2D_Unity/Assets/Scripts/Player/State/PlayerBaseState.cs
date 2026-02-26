using UnityEngine;

public abstract class PlayerBaseState : MonoBehaviour
{
    public abstract void EnterState(PlayerStateContext playerStateContext);
    public abstract void ExitState(PlayerStateContext playerStateContext);
    public abstract void UpdateState(PlayerStateContext playerStateContext);
    public abstract void FixedUpdateState(PlayerStateContext playerStateContext);
    
    public virtual bool CanJump => false;
    public virtual bool CanAttack => false;
    public virtual bool CanMove => true;
}
