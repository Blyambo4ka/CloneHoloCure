using UnityEngine;

public class DamageBoostItem : MonoBehaviour
{
    public float damageMultiplier = 1.1f; // Увеличение урона на 10% (1.1 = 110%)

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Ищем объекты способностей в сцене
            LightningStrike lightningStrike = FindObjectOfType<LightningStrike>();
            DashAbility dashAbility = FindObjectOfType<DashAbility>();

            // Увеличиваем урон способностей
            if (lightningStrike != null)
            {
                lightningStrike.IncreaseDamage(damageMultiplier);
                Debug.Log($"Урон LightningStrike увеличен на {damageMultiplier * 100 - 100}%.");
            }
            else
            {
                Debug.Log("LightningStrike не найден в сцене.");
            }

            if (dashAbility != null)
            {
                dashAbility.IncreaseDamage(damageMultiplier);
                Debug.Log($"Урон DashAbility увеличен на {damageMultiplier * 100 - 100}%.");
            }
            else
            {
                Debug.Log("DashAbility не найден в сцене.");
            }

            Destroy(gameObject);
            Debug.Log("Предмет уничтожен.");
        }
    }
}