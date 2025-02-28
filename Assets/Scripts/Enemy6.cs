using UnityEngine;
using System.Collections;

public class Enemy6 : Enemy
{
    public GameObject projectilePrefab; // Префаб снаряда
    public GameObject damageZonePrefab; // Префаб зоны урона
    public GameObject warningCirclePrefab; // Префаб круга-предупреждения
    public float shootCooldown = 10f; // Интервал стрельбы
    public float zoneCooldown = 6f; // Интервал создания зоны урона
    public float warningDuration = 1f; // Время предупреждения перед уроном
    public int projectileCount = 8; // Количество снарядов
    public float projectileSpeed = 5f; // Скорость снарядов
    public float zoneDuration = 5f; // Время существования зоны урона
    public int zoneDamage = 4; // Урон от зоны
    public float zoneSpawnRadius = 3f; // Радиус появления зоны урона

    public override void Initialize(Transform player, Spawner spawner, int hp, int experienceAmount, int coinAmount, int enemyIndex)
    {
        base.Initialize(player, spawner, hp * 15, experienceAmount + 100, coinAmount + 25, enemyIndex);
        StartCoroutine(ShootRoutine());
        StartCoroutine(ZoneRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (HP > 0) // Пока босс жив, он стреляет
        {
            yield return new WaitForSeconds(shootCooldown);
            ShootInAllDirections();
        }
    }

    private IEnumerator ZoneRoutine()
    {
        while (HP > 0) // Пока босс жив, он создает зоны урона
        {
            yield return new WaitForSeconds(zoneCooldown);
            StartCoroutine(ShowWarningAndCreateZone());
        }
    }

    private IEnumerator ShowWarningAndCreateZone()
    {
        // Используем player из родительского класса Enemy
        if (player == null)
        {
            yield break;
        }

        // Выбираем случайную позицию вокруг игрока
        Vector2 spawnPosition = (Vector2)player.position + Random.insideUnitCircle.normalized * zoneSpawnRadius;

        // Показываем предупреждающий круг
        GameObject warningCircle = Instantiate(warningCirclePrefab, spawnPosition, Quaternion.identity);

        // Ждем перед созданием зоны урона
        yield return new WaitForSeconds(warningDuration);

        // Удаляем предупреждающий круг
        Destroy(warningCircle);

        // Создаем зону урона
        GameObject zone = Instantiate(damageZonePrefab, spawnPosition, Quaternion.identity);
        DamageZone damageZone = zone.GetComponent<DamageZone>();
        if (damageZone != null)
        {
            damageZone.Initialize(zoneDamage, zoneDuration);
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
