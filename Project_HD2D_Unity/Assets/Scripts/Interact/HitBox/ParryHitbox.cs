using UnityEngine;

public class ParryHitbox : BaseHitbox
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IDamageableEnemy target)) return;

        if (HasClearLineTo(other))
        {
            target.GettingParry();
        }
    }
}