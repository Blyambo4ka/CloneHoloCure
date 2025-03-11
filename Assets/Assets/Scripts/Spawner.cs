using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyType
{
    public string name;
    public GameObject prefab;
    public float spawnTime; // Через сколько секунд после старта он начинает появляться
}

public class Spawner : MonoBehaviour
{
    public List<EnemyType> enemyTypes;
    public Transform[] spawnPoints;
    public Transform player;

    public bool isBossSpawner = false; // Флаг для спавнера, который будет спавнить Enemy4 (босс)
    public bool isEnemy6Spawner = false; // Флаг для спавнера, который будет спавнить Enemy6 (второй босс)

    private float timeSinceStart = 0f;
    private float spawnInterval = 2f;
    private float timeSinceLastSpawn = 0f;
    private int enemyCounter = 0;

    private float enemy4SpawnTime = 45f; // Время появления Enemy4 (босс)
    private float enemy6SpawnTime = 80f; // Время появления Enemy6 (второй босс)

    private bool bossSpawned = false;  // Флаг для Enemy4
    private bool enemy6Spawned = false; // Флаг для Enemy6

    void Update()
    {
        timeSinceStart += Time.deltaTime;
        timeSinceLastSpawn += Time.deltaTime;

        // Спавн босса (Enemy4) только если это спавнер для босса
        if (!bossSpawned && isBossSpawner && timeSinceStart >= enemy4SpawnTime)
        {
            SpawnBoss("Enemy4");
            bossSpawned = true; // Босс больше не спавнится
        }

        // Спавн второго босса (Enemy6) только если это спавнер для Enemy6
        if (!enemy6Spawned && isEnemy6Spawner && timeSinceStart >= enemy6SpawnTime)
        {
            SpawnBoss("Enemy6");
            enemy6Spawned = true; // Enemy6 больше не спавнится
        }
        else if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }
    }

    public void SpawnEnemy()
    {
        List<string> availableEnemies = new List<string> { "Enemy" };

        // Добавляем другие врагов в зависимости от времени
        if (timeSinceStart >= 15f) availableEnemies.Add("Enemy2");
        if (timeSinceStart >= 30f) availableEnemies.Add("Enemy3");
        if (timeSinceStart >= 60f) availableEnemies.Add("Enemy5");
        if (timeSinceStart >= 10f) availableEnemies.Add("Enemy7");

        if (availableEnemies.Count == 0) return;

        string selectedEnemyTag = availableEnemies[Random.Range(0, availableEnemies.Count)];
        
        SpawnFromPool(selectedEnemyTag);
    }

    private void SpawnBoss(string enemyTag)
    {
        if ((enemyTag == "Enemy4" && isBossSpawner) || (enemyTag == "Enemy6" && isEnemy6Spawner))  // Проверка, что этот спавнер должен спавнить босса
        {
            SpawnFromPool(enemyTag); // Спавним босса
        }
    }

    private void SpawnFromPool(string enemyTag)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemyObject = ObjectPooler.Instance.SpawnFromPool(enemyTag, spawnPoint.position, Quaternion.identity);

        if (enemyObject != null)
        {
            Enemy enemyScript = enemyObject.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                // Инициализация врага
                int hp = (enemyTag == "Enemy4" || enemyTag == "Enemy6") ? 40 : 10;

                enemyScript.Initialize(player, this, hp, 20, 1, enemyCounter);
                enemyScript.SetEnemyIndex(enemyCounter);
                enemyCounter++;

                enemyObject.SetActive(true);
            }
            else
            {
                Debug.LogError($"Enemy script not found on spawned enemy of type {enemyTag}!");
            }
        }
    }

    public void ReturnEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
    }
}
