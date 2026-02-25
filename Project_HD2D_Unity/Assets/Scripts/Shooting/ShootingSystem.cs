using UnityEngine;

public class ShootingSystem
{
    private const string ProjectilePoolKey = "Projectile";

    private int m_projectileCount = 0;

    public void SpawnProjectile(Vector3 directionShoot, Transform origin)
    {
        if (directionShoot != Vector3.zero)
        {
            directionShoot.Normalize();
        }

        ProjectileBase projectile = ObjectPooler.DequeueObject<ProjectileBase>(ProjectilePoolKey);

        projectile.transform.position = origin.position;
        
        projectile.gameObject.SetActive(true);

        bool odd = (m_projectileCount % 2) == 0;
        projectile.Initialize(directionShoot, odd);

        m_projectileCount++;

        Debug.DrawRay(origin.position, directionShoot * 5f, Color.red, 0.5f);
    }
}