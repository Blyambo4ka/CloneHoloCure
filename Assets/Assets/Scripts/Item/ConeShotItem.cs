using UnityEngine;

public class ConeShotItem : MonoBehaviour
{
    public GameObject projectilePrefab; // Префаб объекта, который будет вылетать
    public float projectileSpeed = 10f; // Скорость объектов
    public float coneAngle = 30f; // Угол разлета объектов (в градусах)
    public int numberOfProjectiles = 3; // Количество объектов
    public float shootInterval = 4f; // Интервал между выстрелами (в секундах)

    public Sprite itemIcon; // Иконка предмета
    public string itemName = "Cone Shot"; // Название предмета
    public string itemDescription = "Стреляет конусом из нескольких снарядов."; // Описание предмета

    private Transform playerTransform; // Трансформ игрока
    private bool isActive = false; // Флаг активности предмета

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActive)
        {
            if (ItemInventoryUIManager.Instance.IsInventoryFull())
            {
                Debug.LogWarning("Инвентарь заполнен! Нельзя подобрать предмет.");
                return; // Предмет не активируется и остаётся на сцене
            }
            playerTransform = collision.transform;
            isActive = true;

            // Добавляем предмет в инвентарь
            ItemInventoryManager.Instance.AddItem(itemName, itemIcon, itemDescription);

            // Отключаем визуальное отображение предмета
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            // Запускаем стрельбу
            InvokeRepeating(nameof(ShootProjectiles), 0f, shootInterval);
        }
    }

    private void ShootProjectiles()
    {
        if (!isActive || playerTransform == null) return;

        // Определяем направление выстрела
        Vector2 baseDirection = playerTransform.right * Mathf.Sign(playerTransform.localScale.x);
        float angleStep = coneAngle / (numberOfProjectiles - 1);
        float startAngle = -coneAngle / 2;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 direction = RotateVector(baseDirection, angle);

            GameObject projectile = Instantiate(projectilePrefab, playerTransform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = direction * projectileSpeed;

            // Поворачиваем снаряд в направлении движения
            float angleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angleZ);
        }
    }

    private Vector2 RotateVector(Vector2 vector, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }

    public void StopShooting()
    {
        isActive = false;
        CancelInvoke(nameof(ShootProjectiles));
    }
}
