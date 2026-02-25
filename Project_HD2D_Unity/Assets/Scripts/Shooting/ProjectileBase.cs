using System;
using DG.Tweening;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] protected float m_speed = 10f;
    [SerializeField] private int m_damage = 1;

    [SerializeField] private float m_amplitudeY    = 1f;
    [SerializeField] private float m_durationBezier = .15f;

    [SerializeField] protected Rigidbody m_rb;

    private const string PoolKey = "Projectile";

    public void Initialize(Vector2 direction, bool odd)
    {
        Initialize(direction);
        DoBezier(odd ? m_amplitudeY : -m_amplitudeY);
    }

    public virtual void Initialize(Vector2 direction)
    {
        m_rb.linearVelocity = new Vector3(direction.x, 0f, direction.y) * m_speed;
    }

    private void DoBezier(float amplitude)
    {
        transform
            .DOLocalMoveY(amplitude, m_durationBezier)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => DoBezier(-amplitude));
    }


    /*private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // TODO: retrieve IDamageable from other and apply m_damage
        }

        ImpactBehaviour();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // TODO: retrieve IDamageable from other and apply m_damage
        }

        ImpactBehaviour();
    }*/
    
    public virtual void ImpactBehaviour()
    {
        transform.DOKill();

        m_rb.linearVelocity = Vector3.zero;

        ObjectPooler.EnqueueObject(this, PoolKey);
    }
}
