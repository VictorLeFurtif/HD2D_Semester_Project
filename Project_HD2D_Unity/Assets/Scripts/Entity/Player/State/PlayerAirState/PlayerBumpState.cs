using UnityEngine;

public class PlayerBumpState : PlayerInAirBase
{
    private float requiredBumpGravity;

    public override string Name => "Bump";

    public override void EnterState(PlayerStateContext psc)
    {
        psc.Controller.SetGravity(false);
        psc.Controller.SetJumping(true);
        psc.AnimationManager.SetFalling(false);
    }

    public override void ExitState(PlayerStateContext psc) { }

    public override void UpdateState(PlayerStateContext psc)
    {
        if (psc.Rb.linearVelocity.y <= 0.1f)
        {
            psc.StateMachine.TransitionTo(psc.StateMachine.FallState);
            return;
        }

        HandleAirMovement(psc);
    }

    public override void FixedUpdateState(PlayerStateContext psc)
    {
        AirControl(psc);
    }
}