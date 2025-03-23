using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyType
{
    public string name;
    public GameObject prefab;
    public float spawnTime; // Через сколько секунд после старта он начинает появляться
    public float minSpawnTime; // Минимальное время спавна
    public float maxSpawnTime; // Максимальное время спавна
}

public class Spawner : MonoBehaviour
{
    public List<EnemyType> enemyTypes;
    public Transform[] spawnPoints;
    public Transform player;

    public bool isBossSpawner = false; // Флаг для спавнера, который будет спавнить Enemy4 (босс)
    public bool isEnemy6Spawner = false; // Флаг для спавнера, который будет спавнить Enemy6 (второй босс)

    private float timeSinceStart = 0f;
    private float spawnInterval = 1f;
    private float timeSinceLastSpawn = 0f;
    private int enemyCounter = 0;

    private float enemy4SpawnTime = 90f; // Время появления Enemy4 (босс)
    private float enemy6SpawnTime = 220f; // Время появления Enemy6 (второй босс)

    private bool bossSpawned = false;  // Флаг для Enemy4
    private bool enemy6Spawned = false; // Флаг для Enemy6

    

    private Dictionary<string, int> enemyCounts = new Dictionary<string, int>();
    private Dictionary<string, int> enemyLimits = new Dictionary<string, int>

    {
        { "Enemy", 1000 },  
        { "Enemy2", 1000 },    
        { "Enemy3", 300 },    
        { "Enemy4", 1 },    // Максимум 1 босс Enemy6
        { "Enemy5", 1000 },    
        { "Enemy6", 1 },    // Максимум 1 босс Enemy6
        { "Enemy7", 150 },  
        { "Enemy9", 1000 },
        { "Enemy10", 1000 },
        { "Enemy8", 150 }  
    };

    void Start()
    {
        // Инициализация счетчиков для каждого типа врагов
        foreach (var key in enemyLimits.Keys)
        {
            enemyCounts.Add(key, 0);
            
        }
        
    }

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
        List<string> availableEnemies = new List<string>();

        foreach (var enemyType in enemyTypes)
        {
            // Проверяем, что текущее время находится в пределах minSpawnTime и maxSpawnTime
            if (timeSinceStart >= enemyType.minSpawnTime && timeSinceStart <= enemyType.maxSpawnTime)
            {
                availableEnemies.Add(enemyType.name);
            }
        }

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
        // Проверяем, не превышен ли лимит для данного типа врага
        if (enemyCounts[enemyTag] >= enemyLimits[enemyTag])
        {
            Debug.Log($"Cannot spawn {enemyTag}: limit reached ({enemyCounts[enemyTag]}/{enemyLimits[enemyTag]}).");
            return;
        }

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

                // Увеличиваем счетчик для данного типа врага
                enemyCounts[enemyTag]++;
            }
            else
            {
                Debug.LogError($"Enemy script not found on spawned enemy of type {enemyTag}!");
            }
        }
    }

    public void ReturnEnemyToPool(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            string enemyTag = enemyScript.GetEnemyTag(); // Добавьте метод GetEnemyTag в класс Enemy
            if (enemyCounts.ContainsKey(enemyTag))
            {
                enemyCounts[enemyTag]--;
            }
        }

        enemy.SetActive(false);
    }
}