using UnityEngine;
using System.Collections;

public class Enemy7 : Enemy
{
    public GameObject damageZonePrefab; // Префаб зоны урона
    public GameObject warningCirclePrefab; // Префаб круга-предупреждения
    public float zoneCooldown = 6f; // Интервал создания зоны урона
    public float warningDuration = 1f; // Время предупреждения перед уроном
    public float zoneDuration = 5f; // Время существования зоны урона
    public int zoneDamage = 4; // Урон от зоны
    public float zoneSpawnRadius = 3f; // Радиус появления зоны урона
    public float keepDistance = 5f; // Минимальная дистанция до игрока
    private Rigidbody2D rb;
    private GameObject currentWarningCircle;
    private Coroutine zoneCoroutine; // Ссылка на корутину зоны урона
    private bool isSpawningZone = false; // Флаг, чтобы не запускать корутину дважды

   public override void Initialize(Transform player, Spawner spawner, int hp, int experienceAmount, int coinAmount, int enemyIndex)
{
    base.Initialize(player, spawner, hp, experienceAmount, coinAmount, enemyIndex);
    rb = GetComponent<Rigidbody2D>();
    zoneCoroutine = StartCoroutine(ZoneRoutine());
}

    void Update()
    {
        if (player != null)
        {
            HandleMovement();
        }

        if (HP <= 0)
        {
            KillEnemy();
        }

         if (HP <= 0)
        {
            KillEnemy(); // Гарантируем, что удаление происходит при смерти
        }
    }

    private void HandleMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 targetPosition = transform.position;

        if (distanceToPlayer > keepDistance)
        {
            targetPosition = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (distanceToPlayer < keepDistance - 1f)
        {
            targetPosition = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }

        rb.MovePosition(targetPosition);
        spriteRenderer.flipX = player.position.x < transform.position.x;
    }

   private IEnumerator ZoneRoutine()
{
    while (HP > 0)
    {
        yield return new WaitForSeconds(zoneCooldown);
        if (HP > 0 && !isSpawningZone) // Проверяем, что враг жив и не спавнит зону
        {
            isSpawningZone = true;
            yield return StartCoroutine(ShowWarningAndCreateZone());
            isSpawningZone = false;
        }
    }
}

private IEnumerator ShowWarningAndCreateZone()
{
    if (player == null || HP <= 0) yield break;

    DestroyWarningCircle(); // Удаляем старый круг перед созданием нового

    Vector2 spawnPosition = (Vector2)player.position + Random.insideUnitCircle * zoneSpawnRadius;  
    currentWarningCircle = Instantiate(warningCirclePrefab, spawnPosition, Quaternion.identity); 

    yield return new WaitForSeconds(warningDuration);

    if (HP <= 0)
    {
        DestroyWarningCircle(); // Удаляем круг, если враг мертв
        yield break;
    }

    DestroyWarningCircle(); // Удаляем круг перед созданием зоны урона

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
