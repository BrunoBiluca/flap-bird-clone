using Assets.UnityFoundation.Systems.HealthSystem.DamageScripts;
using Assets.UnityFoundation.Systems.ObjectPooling;
using UnityEngine;

public class ShooterBirdScript : BirdScript
{
    [SerializeField] private ObjectPooling shootPool;

    private Transform shooterSpawnPoint;

    protected override void OnAwake()
    {
        shooterSpawnPoint = transform.Find("shooter_spawn_point");
        shootPool.InstantiateObjects();
    }

    public void Shoot()
    {
        shootPool
            .GetAvailableObject()
            .Some(obj =>
                obj.Activate<ProjectileController>((shoot) => {
                    shoot.transform.position = shooterSpawnPoint.transform.position;
                    shoot.Setup(1, gameObject);
                })
            );
    }
}
