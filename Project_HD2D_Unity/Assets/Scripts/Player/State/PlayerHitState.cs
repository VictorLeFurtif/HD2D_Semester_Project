using UnityEngine;

namespace Player.State
{
    public class PlayerHitState : PlayerBaseState
    {
        
        public override void EnterState(PlayerStateContext psc)
        {
            psc.AnimationManager.SetIsHit(true);
            Hit(psc);
        }

        public override void ExitState(PlayerStateContext psc)
        {
            psc.AnimationManager.SetIsHit(false);
        }

        public override void UpdateState(PlayerStateContext psc)
        {
            if (!psc.AnimationManager.IsInHitAnimation())
            {
                psc.StateMachine.TransitionTo(psc.StateMachine.LocomotionState);
            }
        }

        public override void FixedUpdateState(PlayerStateContext psc)
        {
            
        }

        private void Hit(PlayerStateContext psc)
        {
            psc.Rb.AddForce(-psc.PlayerTransform.forward * 5f, ForceMode.Impulse);
        }
        

        public override bool CanMove { get; } = false;
        public override bool CanTakeDamage { get; } = false;
    }
}