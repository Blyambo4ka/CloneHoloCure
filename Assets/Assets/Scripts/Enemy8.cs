using UnityEngine;

public class Enemy8 : Enemy
{
    public float shieldCooldown = 5f; // Время перезарядки щита
    public GameObject shieldPrefab; // Префаб щита (для визуального эффекта на враге)
    public float shieldRange = 10f; // Дистанция, на которой хиллер может наложить щит
    private new Animator animator; // Аниматор хиллера
    private Rigidbody2D rb;
    private float lastShieldTime; // Время последнего наложения щита
    public float keepDistance = 4f;

    

    void Update()
    {
       

        rb = GetComponent<Rigidbody2D>();

        if (player != null)
        {
            HandleMovement(); // Движение врага

            if (Time.time >= lastShieldTime + shieldCooldown)
            {
                TryApplyShield();
                lastShieldTime = Time.time;
            }
        }

        if (HP <= 0)
        {
            KillEnemy(); // Умирает, если здоровье <= 0
        }
    }

    private void TryApplyShield()
    {
        // Ищем ближайшего врага в радиусе
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, shieldRange);
        foreach (var collider in nearbyEnemies)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && enemy != this) // Проверяем, чтобы не наложить щит на себя
            {
                ApplyShieldToEnemy(enemy); // Накладываем щит на врага
                break; // Накладываем щит только на одного врага
            }
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

    private void ApplyShieldToEnemy(Enemy enemy)
    {
        // Проигрываем анимацию с триггером "Active"
        if (animator != null)
        {
            animator.SetTrigger("Active");
        }

        // Накладываем щит на врага
        enemy.ApplyShield(shieldPrefab, 30, 7f); // 30 HP, 7 секунд
    }
}