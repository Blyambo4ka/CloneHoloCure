using UnityEngine;
using System.Collections;

public class Enemy4 : Enemy
{
    public GameObject projectilePrefab; // Префаб снаряда
    public float shootCooldown = 10f; // Интервал стрельбы
    public int projectileCount = 8; // Количество снарядов
    public float projectileSpeed = 5f; // Скорость снарядов

    public override void Initialize(Transform player, Spawner spawner, int hp, int experienceAmount, int coinAmount, int enemyIndex)
    {
        base.Initialize(player, spawner, hp * 12, experienceAmount + 100, coinAmount + 25, enemyIndex);
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (HP > 0) // Пока босс жив, он стреляет
        {
            yield return new WaitForSeconds(shootCooldown);
            ShootInAllDirections();
        }
    }

    private void ShootInAllDirections()
    {
        float angleStep = 360f / projectileCount;
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            ShootProjectile(direction);
        }
    }

    private void ShootProjectile(Vector2 direction)
    {
        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }
        }
    }
}
