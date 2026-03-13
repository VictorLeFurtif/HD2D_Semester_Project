using System.Collections;
using Manager;
using UnityEngine;

namespace Player.State
{
    public class PlayerAttackMeleeState : PlayerBaseState
    {
        #region Variables

        private bool bufferNextAttack = false;
        private bool bufferWindowOpen = false;
        private int comboIndex = 0;
        private Coroutine currentAttackRoutine;

        public override bool CanShoot => false;
        public override bool CanMove => false;
        public override string Name => "Attack Melee";

        #endregion

        #region Base

        public override void EnterState(PlayerStateContext psc)
        {
            comboIndex = 0;
            bufferNextAttack = false;
            StartAttackSequence(psc);
        }

        public override void ExitState(PlayerStateContext psc) 
        {
            if (currentAttackRoutine != null)
                psc.Controller.StopCoroutine(currentAttackRoutine);
            
            bufferWindowOpen = false;
            bufferNextAttack = false;
        }

        public override void UpdateState(PlayerStateContext psc)
        {
            HandleAnimation(psc);
        }

        public override void FixedUpdateState(PlayerStateContext psc) { }

        #endregion

        #region Public

        public void BufferAttack()
        {
            if (bufferWindowOpen)
                bufferNextAttack = true;
        }

        #endregion

        #region Combo Logic

        private void StartAttackSequence(PlayerStateContext psc)
        {
            if (currentAttackRoutine != null)
                psc.Controller.StopCoroutine(currentAttackRoutine);

            psc.AnimationManager.SetComboIndex(comboIndex);
            currentAttackRoutine = psc.Controller.RunRoutine(AttackMeleeIe(psc));
        }

        private void ResolveCombo(PlayerStateContext psc)
        {
            bool canCombo = bufferNextAttack && comboIndex < psc.PlayerData.ComboHits.Length - 1;

            if (canCombo)
            {
                comboIndex++;
                bufferNextAttack = false;
                StartAttackSequence(psc);
            }
            else
            {
                comboIndex = 0;
                psc.AnimationManager.ExitAttack();
                psc.StateMachine.TransitionTo(psc.StateMachine.LocomotionState);
            }
        }

        #endregion

        #region Coroutines

        private IEnumerator AttackMeleeIe(PlayerStateContext psc)
        {
            CombatHitData hit = psc.PlayerData.ComboHits[comboIndex];

            yield return DashIe(hit, psc);

            yield return new WaitUntil(() => psc.AnimationManager.IsInAttackAnimation());

            bufferWindowOpen = true;

            float clipLength = psc.AnimationManager.GetCurrentAnimatorStateInfo(0).length;
            
            yield return new WaitForSeconds(clipLength * 0.8f);

            bufferWindowOpen = false;
            ResolveCombo(psc);
        }

        private IEnumerator DashIe(CombatHitData data, PlayerStateContext psc)
        {
            float elapsed = 0f;
            psc.Rb.linearVelocity = Vector3.zero;

            while (elapsed < data.DashDuration)
            {
                psc.Rb.linearVelocity = Vector3.Lerp(
                    psc.PlayerTransform.forward * data.DashSpeed,
                    Vector3.zero,
                    elapsed / data.DashDuration);

                elapsed += Time.deltaTime;
                yield return null;
            }
            psc.Rb.linearVelocity = Vector3.zero;
        }

        #endregion
    }
}