using UnityEngine;

public class AiKO : AiState
{
    private float koTimer;

    public override string Name => "K-O";

    public override void EnterState(AiContext actx)
    {
        if (actx.Agent.isActiveAndEnabled)
        {
            actx.Agent.isStopped = true;
        }

        koTimer = actx.Data.KoTime;

        if (actx.Behavior.KoSlider != null)
        {
            actx.Behavior.KoSlider.value = 0;
        }
        
        actx.AnimManager.SetKO(true);
    }

    public override void UpdateState(AiContext actx)
    {
        koTimer -= Time.deltaTime;

        if (actx.Behavior.KoSlider != null)
        {
            actx.Behavior.KoSlider.value = 1f - (koTimer / actx.Data.KoTime);
        }

        if (koTimer <= 0)
        {
            DetermineNextState(actx);
        }
    }

    private void DetermineNextState(AiContext actx)
    {
        if (actx.Behavior.IsCarry())
        {
            actx.Behavior.Eject(true);
        }
        else
        {
            actx.TransitionTo(actx.Behavior.PatrolState);
        }
    }

    public override void ExitState(AiContext actx)
    {
        actx.AnimManager.SetKO(false);
    }

    public override bool CanAttack => false;
    public override bool CanMove => false;
    public override bool CanTakeDamage => false; 
}