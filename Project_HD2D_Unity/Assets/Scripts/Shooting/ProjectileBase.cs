using System;
using DG.Tweening;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] protected float m_speed = 10f;
    [SerializeField] private int m_damage = 1;


    [SerializeField] protected Rigidbody m_rb;

    [SerializeField] protected string poolKey = "Projectile_";
    public string PoolKey => poolKey;
    

    public virtual void Initialize(Vector2 direction)
    {
        m_rb.linearVelocity = new Vector3(direction.x, 0f, direction.y) * m_speed;
    }

    
    private void OnCollisionEnter(Collision other)
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
    }
    
    public virtual void ImpactBehaviour()
    {

        m_rb.linearVelocity = Vector3.zero;

        ObjectPooler.EnqueueObject(this, PoolKey);
    }
}
