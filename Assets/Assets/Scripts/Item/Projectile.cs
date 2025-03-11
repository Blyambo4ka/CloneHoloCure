using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 moveDirection; // Направление движения пули
    private float speed; // Скорость пули
    public int damage = 10; // Урон от пули
    public float lifetime = 3f; // Время жизни пули
    public float rotationSpeed = 200f; // Скорость вращения пули (градусов в секунду)

    void Start()
    {
        Destroy(gameObject, lifetime); // Уничтожаем пулю через указанное время
    }

    void Update()
    {
        // Двигаем пулю
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // Вращаем пулю
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    public void SetDirection(Vector2 direction, float bulletSpeed)
    {
        moveDirection = direction.normalized; // Нормализуем направление
        speed = bulletSpeed; // Устанавливаем скорость
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, есть ли компонент Collider2D
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogError("Collider2D отсутствует на объекте!");
            return;
        }

        // Проверяем тег объекта, с которым произошло столкновение
        if (collision.CompareTag("Enemy") || collision.CompareTag("Enemy2") ||
            collision.CompareTag("Enemy3") || collision.CompareTag("Enemy4") ||
            collision.CompareTag("Enemy5") || collision.CompareTag("Enemy6") ||
            collision.CompareTag("Enemy7"))
        {
            // Получаем компонент Enemy
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Наносим урон врагу
                enemy.TakeDamage(damage);
            }

            // Уничтожаем пулю
            Destroy(gameObject);
        }
    }
}