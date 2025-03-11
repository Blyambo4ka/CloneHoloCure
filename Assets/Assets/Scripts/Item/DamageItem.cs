using UnityEngine;

public class DamageItem : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
              MovementPlayer player = collider.GetComponent<MovementPlayer>();
            if (player != null)
            {
                // Активируем предмет
                DamageBoostManager.ActivateBoost(player);
            }
            // Активируем эффект замедления
            DamageBoostManager.isBoostActive = true;
            Debug.Log("Усиление урона активировано!");

            // Уничтожаем объект предмета
            Destroy(gameObject);
        }
    }
}