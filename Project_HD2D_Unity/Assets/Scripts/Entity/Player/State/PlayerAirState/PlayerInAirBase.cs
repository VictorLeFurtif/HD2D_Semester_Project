using UnityEngine;

public abstract class PlayerInAirBase : PlayerBaseState
{
    public override string Name => "InAir";
    public override bool CanDash => true;

    protected void HandleAirMovement(PlayerStateContext psc)
    {   
        HandleMovement(psc);
        HandleAnimation(psc);
    }

    protected void AirControl(PlayerStateContext psc)
    {
        HandlePhysics(psc);
    }
}