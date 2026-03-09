using System.Collections;
using UnityEngine;

namespace Player.State
{
    public class PlayerDashState : PlayerBaseState
    {
        private float dashSpeed    = 20f;
        private float dashDuration = 0.4f;

        public override string Name { get; protected set; } = "Dash";

        public override bool CanShoot  => false;
        public override bool CanMove   => false;
        public override bool CanJump   => false;
        public override bool CanAttack => false;

        public override void EnterState(PlayerStateContext psc)
        {
            psc.Controller.RunRoutine(DashRoutine(psc));
        }

        public override void ExitState(PlayerStateContext psc) { }

        public override void UpdateState(PlayerStateContext psc)
        {
            HandleCursor(psc);
            HandleAnimation(psc);
        }

        public override void FixedUpdateState(PlayerStateContext psc) { }

        private IEnumerator DashRoutine(PlayerStateContext psc)
        {
            float elapsed = 0f;

            Vector3 dashDirection = psc.PlayerTransform.forward;

            while (elapsed < dashDuration)
            {
                psc.Rb.linearVelocity = Vector3.Lerp(
                    dashDirection * dashSpeed,
                    Vector3.zero,
                    elapsed / dashDuration);

                elapsed += Time.deltaTime;
                yield return null;
            }

            psc.Rb.linearVelocity = Vector3.zero;
            psc.StateMachine.TransitionTo(new PlayerLocomotionState());
        }
    }
}