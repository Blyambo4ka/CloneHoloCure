using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomEvents : MonoBehaviour
{
    private List<GameEvent> events = new List<GameEvent>();

    public GameObject warningCirclePrefab;
    public GameObject damageZonePrefab;

    private float gameTime = 0f;

    public Transform player;

    public float spawnRadius = 3f;

    public float warningDuration = 1f;
    public float zoneDuration = 0.8f;
    public int zoneDamage = 5;

    void Start()
    {
        ScheduleEvent(30f, () => StartCoroutine(SpawnCrossOfCircles()));
        ScheduleEvent(45f, () => StartCoroutine(SpawnCirclesAroundPlayer()));
        ScheduleEvent(60f, () => StartCoroutine(SpawnSpiralExplosion()));
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        CheckEvents();
    }

    private void ScheduleEvent(float time, System.Action action)
    {
        events.Add(new GameEvent { Time = time, Action = action });
    }

    private void CheckEvents()
    {
        for (int i = events.Count - 1; i >= 0; i--)
        {
            if (gameTime >= events[i].Time)
            {
                events[i].Action.Invoke();
                events.RemoveAt(i);
            }
        }
    }

  

    private IEnumerator SpawnCrossOfCircles()
    {
        if (warningCirclePrefab != null && damageZonePrefab != null && player != null)
        {
            Vector2 centerPosition = player.position;

            GameObject warningCircle = Instantiate(warningCirclePrefab, centerPosition, Quaternion.identity);

            yield return new WaitForSeconds(warningDuration);

            Destroy(warningCircle);

            int count = 15;
            float angleStep = 360f / count;

            for (int i = 0; i < count; i++)
            {
                float angle = i * angleStep;
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                Vector2 spawnPosition = centerPosition + direction * spawnRadius;

                GameObject zone = Instantiate(damageZonePrefab, spawnPosition, Quaternion.identity);
                DamageZone damageZone = zone.GetComponent<DamageZone>();
                if (damageZone != null)
                {
                    damageZone.Initialize(zoneDamage, zoneDuration);
                }
            }
        }
    }

    private IEnumerator SpawnCirclesAroundPlayer()
    {
        if (warningCirclePrefab != null && damageZonePrefab != null && player != null)
        {
            Vector2 centerPosition = player.position;

            GameObject warningCircle = Instantiate(warningCirclePrefab, centerPosition, Quaternion.identity);

            yield return new WaitForSeconds(warningDuration);

            Destroy(warningCircle);

            int count = 10;
            float angleStep = 360f / count;

            for (int i = 0; i < count; i++)
            {
                float angle = i * angleStep;
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                Vector2 spawnPosition = centerPosition + direction * spawnRadius;

                GameObject zone = Instantiate(damageZonePrefab, spawnPosition, Quaternion.identity);
                DamageZone damageZone = zone.GetComponent<DamageZone>();
                if (damageZone != null)
                {
                    damageZone.Initialize(zoneDamage, zoneDuration);
                }
            }
        }
    }

    private IEnumerator SpawnSpiralExplosion()
    {
        if (warningCirclePrefab != null && damageZonePrefab != null && player != null)
        {
            Vector2 centerPosition = player.position;

            GameObject warningCircle = Instantiate(warningCirclePrefab, centerPosition, Quaternion.identity);

            yield return new WaitForSeconds(warningDuration);

            Destroy(warningCircle);

            int spiralTurns = 3;
            int circlesPerTurn = 10;
            float angleStep = 360f / circlesPerTurn;
            float radiusStep = spawnRadius / (spiralTurns * circlesPerTurn);

            for (int turn = 0; turn < spiralTurns; turn++)
            {
                for (int i = 0; i < circlesPerTurn; i++)
                {
                    float angle = i * angleStep;
                    float radius = (turn * circlesPerTurn + i) * radiusStep;
                    Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                    Vector2 spawnPosition = centerPosition + direction * radius;

                    GameObject zone = Instantiate(damageZonePrefab, spawnPosition, Quaternion.identity);
                    DamageZone damageZone = zone.GetComponent<DamageZone>();
                    if (damageZone != null)
                    {
                        damageZone.Initialize(zoneDamage, zoneDuration);
                    }

                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    private Vector2 GetRandomPositionNearPlayer()
    {
        if (player == null)
        {
            Debug.LogError("Игрок не найден!");
            return Vector2.zero;
        }

        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float radius = Random.Range(1f, spawnRadius);

        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        return (Vector2)player.position + offset;
    }

    private class GameEvent
    {
        public float Time { get; set; }
        public System.Action Action { get; set; }
    }
}