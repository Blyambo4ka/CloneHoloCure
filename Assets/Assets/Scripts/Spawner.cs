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

    private float timeSinceStart = 0f;
    private float spawnInterval = 2f;
    private float timeSinceLastSpawn = 0f;
    private int enemyCounter = 0;

    private float enemy2SpawnTime = 15f;
    private float enemy3SpawnTime = 30f;
    private float enemy4SpawnTime = 45f;
    private float enemy5SpawnTime = 60f;
    private float enemy6SpawnTime = 80f; // Время появления Enemy6

    private bool bossSpawned = false;  // Флаг для Enemy4
    private bool enemy6Spawned = false; // Флаг для Enemy6

    void Update()
    {
        timeSinceStart += Time.deltaTime;
        timeSinceLastSpawn += Time.deltaTime;

        // Спавн Enemy6 один раз на 80 секунде
        if (!enemy6Spawned && timeSinceStart >= enemy6SpawnTime)
        {
            SpawnEnemy6();
            enemy6Spawned = true; // Enemy6 больше не спавнится
        }

        // Спавн босса (Enemy4) один раз
        if (!bossSpawned && timeSinceStart >= enemy4SpawnTime)
        {
            SpawnBoss();
            bossSpawned = true; // Босс больше не спавнится
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

        if (timeSinceStart >= enemy2SpawnTime) availableEnemies.Add("Enemy2");
        if (timeSinceStart >= enemy3SpawnTime) availableEnemies.Add("Enemy3");
        if (timeSinceStart >= enemy5SpawnTime) availableEnemies.Add("Enemy5");

        if (availableEnemies.Count == 0) return;

        string selectedEnemyTag = availableEnemies[Random.Range(0, availableEnemies.Count)];
        
        SpawnFromPool(selectedEnemyTag);
    }

    private void SpawnBoss()
    {
        SpawnFromPool("Enemy4"); // Спавним босса один раз
    }

    private void SpawnEnemy6()
    {
        SpawnFromPool("Enemy6"); // Спавним Enemy6 один раз
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
                // Если спавним Enemy6, даем ему больше HP
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
