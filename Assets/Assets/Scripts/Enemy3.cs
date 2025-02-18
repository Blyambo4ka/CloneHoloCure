using UnityEngine;

public class Enemy3 : Enemy
{
    public float keepDistance = 5f; // Минимальная дистанция до игрока
    public float shootingInterval = 2f; // Время между выстрелами
    public GameObject bulletPrefab; // Префаб пули
    public Transform firePoint; // Точка, где создаётся пуля
    public float bulletSpeed = 5f; // Скорость полёта пули
    private float lastShootTime; // Время последнего выстрела
    private Rigidbody2D rb;



    public override void Initialize(Transform player, Spawner spawner, int hp, int experienceAmount, int coinAmount, int enemyIndex)
    {
        base.Initialize(player, spawner, hp, experienceAmount + 5, coinAmount + 2, enemyIndex);
    }
    void Update()
    {

        rb = GetComponent<Rigidbody2D>();

        if (player != null)
        {
            HandleMovement(); // Движение врага

            if (Time.time >= lastShootTime + shootingInterval)
            {
                Shoot(); // Выстрел
                lastShootTime = Time.time;
            }
        }

        if (HP <= 0)
        {
            KillEnemy(); // Умирает, если здоровье <= 0
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

    private void Shoot()
{
    if (firePoint == null)
    {
        Debug.LogError("firePoint is NULL! Set it in the Inspector.");
    }
    if (bulletPrefab == null)
    {
        Debug.LogError("bulletPrefab is NULL! Set it in the Inspector.");
    }

    if (bulletPrefab != null && firePoint != null)
    {
       
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - firePoint.position).normalized;
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction, bulletSpeed);
        }
    }
}
}