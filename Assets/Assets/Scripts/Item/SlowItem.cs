using UnityEngine;

public class SlowItem : MonoBehaviour
{
    public static bool isActivated = false; // Статический флаг: активирован ли предмет

    public Sprite itemIcon; // Иконка предмета
    public string itemName = "Slow Effect"; // Название предмета
    public string itemDescription = "Замедляет врагов."; // Описание предмета

    void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.CompareTag("Player"))
        {
             if (ItemInventoryUIManager.Instance.IsInventoryFull())
        {
            Debug.LogWarning("Инвентарь заполнен! Нельзя подобрать предмет.");
            return; // Предмет не активируется и остаётся на сцене
        }
            // Активируем эффект замедления
            isActivated = true;

            // Добавляем предмет в инвентарь
            ItemInventoryManager.Instance.AddItem(itemName, itemIcon, itemDescription);

            // Уничтожаем объект предмета
            Destroy(gameObject);
        }
    }
}
