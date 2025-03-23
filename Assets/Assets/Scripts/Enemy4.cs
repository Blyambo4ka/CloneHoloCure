using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy4 : Enemy
{
    public GameObject projectilePrefab; // Префаб снаряда
    public float shootCooldown = 10f; // Интервал стрельбы
    public int projectileCount = 8; // Количество снарядов
    public float projectileSpeed = 5f; // Скорость снарядов

    // Список предметов (типа Item1) для выпадения
    public List<Item1> lootTable;

    public override void Initialize(Transform player, Spawner spawner, int hp, int experienceAmount, int coinAmount, int enemyIndex)
    {
        base.Initialize(player, spawner, hp * 45, experienceAmount + 100, coinAmount + 25, enemyIndex);
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

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (HP <= 0) // Если босс умер
        {
            DropLoot(); // Выпадение предмета
           
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
}