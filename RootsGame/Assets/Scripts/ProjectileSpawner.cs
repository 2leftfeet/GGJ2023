using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] RootableProjectile projectile;
    [SerializeField] float projectileSpeed;
    [SerializeField] Vector2 projectileDirection;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float spawnCooldown;

    float spawnTimer;

    void Start()
    {
        spawnTimer = spawnCooldown;
    }

    void Update()
    {
        if(spawnTimer > 0f)
        {
            spawnTimer -= Time.deltaTime;
            if(spawnTimer < 0f)
            {
                SpawnProjectile();
                spawnTimer = spawnCooldown;
            }
        }
    }

    void SpawnProjectile()
    {
        RootableProjectile proj = Instantiate(projectile, spawnPoint.position, Quaternion.identity);

        proj.speed = projectileSpeed;
        proj.direction = projectileDirection;
    }


}
