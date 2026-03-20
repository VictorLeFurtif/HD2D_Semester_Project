using System.Collections;
using UnityEngine;

public class AiAttack : AiState
{
    private Coroutine attackRoutine;
    private bool isBusy = false; 

    public override string Name => "Attacking";

    public override void EnterState(AiContext actx)
    {
        isBusy = false;
        if (actx.Agent.isActiveAndEnabled) 
            actx.Agent.isStopped = true;
    }

    public override void UpdateState(AiContext actx)
    {
        if (isBusy || attackRoutine != null) return;

        if (actx.Target == null) 
        { 
            actx.TransitionTo(actx.Behavior.SearchState); 
            return; 
        }

        if (!actx.IsPlayerInAttackRange)
        {
            actx.TransitionTo(actx.Behavior.ChaseState);
            return;
        }

        RotateTowardsTarget(actx);
        attackRoutine = actx.Behavior.StartCoroutine(AttackSequence(actx));
    }

    private IEnumerator AttackSequence(AiContext actx)
    {
        isBusy = true;
        var data = actx.Data; 

        yield return new WaitForSeconds(data.AnticipationTime);
        actx.AnimManager.TriggerAttack(); 

        actx.AnimManager.AttackOn();
    
        float elapsed = 0f;
        Vector3 strikeDir = actx.Behavior.transform.forward;
        float activePhaseDuration = Mathf.Max(data.AttackDashDuration, data.HitboxActiveDuration);

        while (elapsed < activePhaseDuration)
        {
            elapsed += Time.deltaTime;

            if (elapsed <= data.AttackDashDuration)
            {
                actx.Behavior.transform.position += strikeDir * data.AttackDashSpeed * Time.deltaTime;
            }

            if (elapsed >= data.HitboxActiveDuration)
            {
                actx.AnimManager.AttackOff();
            }

            yield return null;
        }

        actx.AnimManager.AttackOff();

        yield return new WaitForSeconds(data.AttackCooldown); 
    
        isBusy = false;
        attackRoutine = null;
    }

    private void RotateTowardsTarget(AiContext actx)
    {
        Vector3 lookDir = (actx.Target.transform.position - actx.Behavior.transform.position).normalized;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
        {
            actx.Behavior.transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }

    public override void ExitState(AiContext actx) 
    { 
        if (attackRoutine != null) 
            actx.Behavior.StopCoroutine(attackRoutine);
        
        isBusy = false;
        actx.AnimManager.AttackOff();

        if (actx.Agent.isActiveAndEnabled) 
            actx.Agent.isStopped = false;
    }

    public override bool CanMove => !isBusy;
}