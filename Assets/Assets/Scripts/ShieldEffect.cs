using UnityEngine;

public class ShieldEffect : MonoBehaviour
{
    public float shieldDuration = 5f; // Длительность щита
    public GameObject shieldVisualPrefab; // Визуальный префаб щита (иконка над врагом)
    public float speed = 5f; // Скорость движения щита
    private Vector2 direction; // Направление движения

    public void SetDirection(Vector2 dir, float spd)
    {
        direction = dir;
        speed = spd;
    }

    void Update()
    {
        // Движение щита
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, столкнулся ли щит с врагом
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Накладываем щит на врага
            enemy.ApplyShield(shieldVisualPrefab, 30, 7f); // 30 HP, 7 секунд
            Destroy(gameObject); // Уничтожаем объект щита после применения
        }
    }
}