using UnityEngine;

public class ShootingSystem
{
    private const string ProjectilePoolKey = "Projectile";

    public void SpawnProjectile(Vector3 directionShoot, Transform origin)
    {
        ProjectileBase projectile = ObjectPooler.DequeueObject<ProjectileBase>(ProjectilePoolKey);

        projectile.transform.position = origin.position;
        
        projectile.gameObject.SetActive(true);

        projectile.Initialize(new Vector2(directionShoot.x,directionShoot.z));
    }

    
}