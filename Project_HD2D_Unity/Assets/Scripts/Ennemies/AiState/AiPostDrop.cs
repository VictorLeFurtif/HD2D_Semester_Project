using UnityEngine;

public class AiPostDrop : AiState
{
    public override string Name => "Post_Drop";
    
    public override void EnterState(AiContext actx) { }

    public override void UpdateState(AiContext actx) { }

    public override void ExitState(AiContext actx) { }
    
    public virtual bool CanAttack => false;
    
    public virtual bool CanMove => false;
    
    public virtual bool CanTakeDamage => false;
}
