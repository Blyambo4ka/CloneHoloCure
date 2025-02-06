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
    private float enemy2SpawnTime = 15f; // Через сколько секунд начнется спавн Enemy2
    

    void Update()
    {
        timeSinceStart += Time.deltaTime;
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }
    }

    
    public void SpawnEnemy()
    {
        string selectedEnemyTag;

        if (Time.timeSinceLevelLoad >= enemy2SpawnTime) 
        {
            // Если прошло 30 секунд, спавним случайного врага
            string[] enemyTypes = { "Enemy", "Enemy2", "Enemy3" };
            selectedEnemyTag = enemyTypes[Random.Range(0, enemyTypes.Length)];
        }
        else
        {
            // До 30 секунд спавним только обычного врага
            selectedEnemyTag = "Enemy";
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemyObject = ObjectPooler.Instance.SpawnFromPool(selectedEnemyTag, spawnPoint.position, Quaternion.identity);

        if (enemyObject != null)
        {
            Enemy enemyScript = enemyObject.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                enemyScript.Initialize(player, this, 10, 20, 1, enemyCounter);
                enemyScript.SetEnemyIndex(enemyCounter);
                enemyCounter++;

                enemyObject.SetActive(true);
            }
            else
            {
                Debug.LogError($"Enemy script not found on spawned enemy of type {selectedEnemyTag}!");
            }
        }
    }

    

    public void ReturnEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
    }
}
