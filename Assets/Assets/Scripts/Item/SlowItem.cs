using UnityEngine;

public class SlowItem : MonoBehaviour
{
    public static bool isActivated = false; // Статический флаг: активирован ли предмет

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Активируем эффект замедления
            isActivated = true;

            // Уничтожаем объект предмета
            Destroy(gameObject);
        }
    }
}