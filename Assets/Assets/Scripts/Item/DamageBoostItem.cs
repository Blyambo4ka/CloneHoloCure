using UnityEngine;

public class DamageBoostItem : MonoBehaviour
{
    public float damageMultiplier = 1.1f; // Увеличение урона на 10% (1.1 = 110%)
    public Sprite itemIcon; // Иконка предмета
    public string itemName = "DamageBoost"; // Название предмета
    public string itemDescription = "Увеличивает урон на 10%."; // Описание предмета

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
             if (ItemInventoryUIManager.Instance.IsInventoryFull())
        {
            Debug.LogWarning("Инвентарь заполнен! Нельзя подобрать предмет.");
            return; // Предмет не активируется и остаётся на сцене
        }
            // Добавляем предмет в инвентарь
            ItemInventoryManager.Instance.AddItem(itemName, itemIcon, itemDescription);

            // Применяем эффект предмета (например, увеличение урона)
            ApplyItemEffect();

            // Уничтожаем предмет после подбора
            Destroy(gameObject);
            Debug.Log("Предмет поднят и уничтожен.");
        }
    }

    private void ApplyItemEffect()
    {
        // Пример: Увеличиваем урон способностей
        LightningStrike lightningStrike = FindObjectOfType<LightningStrike>();
        DashAbility dashAbility = FindObjectOfType<DashAbility>();

        if (lightningStrike != null)
        {
            lightningStrike.IncreaseDamage(damageMultiplier);
            Debug.Log($"Урон LightningStrike увеличен на {damageMultiplier * 100 - 100}%.");
        }

        if (dashAbility != null)
        {
            dashAbility.IncreaseDamage(damageMultiplier);
            Debug.Log($"Урон DashAbility увеличен на {damageMultiplier * 100 - 100}%.");
        }
    }
}