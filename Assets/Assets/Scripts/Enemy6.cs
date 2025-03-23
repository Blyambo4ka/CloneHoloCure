using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    // Список предметов (типа Item1) для выпадения
    public List<Item1> lootTable;

    private GameObject currentWarningCircle; // Текущий предупреждающий круг

    public override void Initialize(Transform player, Spawner spawner, int hp, int experienceAmount, int coinAmount, int enemyIndex)
    {
        base.Initialize(player, spawner, hp * 55, experienceAmount + 100, coinAmount + 25, enemyIndex);
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
            yield return StartCoroutine(ShowWarningAndCreateZone());
        }
    }

    private IEnumerator ShowWarningAndCreateZone()
    {
        // Используем player из родительского класса Enemy
        if (player == null || HP <= 0)
        {
            yield break;
        }

        // Выбираем случайную позицию вокруг игрока
        Vector2 spawnPosition = (Vector2)player.position + Random.insideUnitCircle.normalized * zoneSpawnRadius;

        // Показываем предупреждающий круг
        DestroyWarningCircle(); // Удаляем старый круг перед созданием нового
        currentWarningCircle = Instantiate(warningCirclePrefab, spawnPosition, Quaternion.identity);

        // Ждем перед созданием зоны урона
        yield return new WaitForSeconds(warningDuration);

        // Удаляем предупреждающий круг
        DestroyWarningCircle();

        // Создаем зону урона
        GameObject zone = Instantiate(damageZonePrefab, spawnPosition, Quaternion.identity);
        DamageZone damageZone = zone.GetComponent<DamageZone>();
        if (damageZone != null)
        {
            damageZone.Initialize(zoneDamage, zoneDuration);
        }
    }

    private void DestroyWarningCircle()
    {
        if (currentWarningCircle != null)
        {
            Destroy(currentWarningCircle);
            currentWarningCircle = null;
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

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (HP <= 0) // Если босс умер
        {
            
            DropLoot();
        }
    }

    private void DropLoot()
    {
        if (lootTable != null && lootTable.Count > 0)
        {
            Item1 selectedItem = GetRandomItem(); // Получение случайного предмета с учетом редкости
            if (selectedItem != null && selectedItem.itemPrefab != null)
            {
                Instantiate(selectedItem.itemPrefab, transform.position, Quaternion.identity); // Создание предмета на месте смерти босса
            }
        }
    }

    

    private Item1 GetRandomItem()
    {
        float totalWeight = 0f;

        // Считаем общий вес всех предметов
        foreach (Item1 item in lootTable)
        {
            totalWeight += item.rarity;
        }

        // Генерируем случайное число от 0 до totalWeight
        float randomValue = Random.Range(0, totalWeight);

        float cumulativeWeight = 0f;
        foreach (Item1 item in lootTable)
        {
            cumulativeWeight += item.rarity;
            if (randomValue <= cumulativeWeight)
            {
                return item; // Возвращаем предмет, который соответствует случайному числу
            }
        }

        return null; // Если ничего не найдено (редкий случай)
    }

    // ГАРАНТИРОВАННОЕ УДАЛЕНИЕ КРУГА ПРИ СМЕРТИ
    public override void KillEnemy()
    {
        DestroyWarningCircle(); // Удаляем предупреждающий круг
        base.KillEnemy(); // Вызываем стандартное уничтожение врага
    }

    // ГАРАНТИРОВАННОЕ УДАЛЕНИЕ КРУГА ПРИ УДАЛЕНИИ ОБЪЕКТА
    private void OnDestroy()
    {
        DestroyWarningCircle();
    }
}