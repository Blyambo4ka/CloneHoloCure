using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 moveDirection; // Направление движения пули
    private float speed; // Скорость пули
    public int damage = 10; // Урон от пули
    public float lifetime = 3f; // Время жизни пули 

    void Start()
    {
        Destroy(gameObject, lifetime); // Уничтожаем пулю через указанное время
    }

    void Update()
    {
        // Двигаем пулю
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 direction, float bulletSpeed)
    {
        moveDirection = direction.normalized; // Нормализуем направление
        speed = bulletSpeed; // Устанавливаем скорость
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Если пуля попадает в игрока
            MovementPlayer player = collision.GetComponent<MovementPlayer>();
            if (player != null)
            {
                player.TakeDamage(damage); // Наносим урон игроку
            }

            Destroy(gameObject); // Уничтожаем пулю
        }
        
        
    }
}