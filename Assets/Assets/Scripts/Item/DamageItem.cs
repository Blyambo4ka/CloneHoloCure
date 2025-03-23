using UnityEngine;

public class DamageItem : MonoBehaviour
{
    public Sprite itemIcon; // Иконка предмета
    public string itemName = "Damage Boost"; // Название предмета
    public string itemDescription = "Временно увеличивает урон."; // Описание предмета

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (ItemInventoryUIManager.Instance.IsInventoryFull())
            {
                Debug.LogWarning("Инвентарь заполнен! Нельзя подобрать предмет.");
                return; // Предмет не активируется и остаётся на сцене
            }
            MovementPlayer player = collider.GetComponent<MovementPlayer>();
            if (player != null)
            {
                // Активируем предмет
                DamageBoostManager.ActivateBoost(player);
            }

            // Добавляем предмет в инвентарь
            ItemInventoryManager.Instance.AddItem(itemName, itemIcon, itemDescription);

            // Активируем эффект усиления урона
            DamageBoostManager.isBoostActive = true;
            Debug.Log("Усиление урона активировано!");

            // Уничтожаем объект предмета
            Destroy(gameObject);
        }
    }
}
