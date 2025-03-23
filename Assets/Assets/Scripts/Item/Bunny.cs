using UnityEngine;

public class Bunny : MonoBehaviour
{
    public DropOnKill dropOnKill; // Ссылка на систему выпадения предметов

    public Sprite itemIcon; // Иконка предмета
    public string itemName = "Bunny"; // Название предмета
    public string itemDescription = "Хилки будут выпадать чаще."; // Описание предмета

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

            // Применяем эффект предмета
            ApplyItemEffect();

            // Уничтожаем предмет после подбора
            Destroy(gameObject);
            Debug.Log("Bunny поднят и уничтожен.");
        }
    }

    private void ApplyItemEffect()
    {
        if (dropOnKill != null)
        {
            DropOnKill.isActive = true; // Вызов метода для активации выпадения хилок
            Debug.Log("Шанс выпадения хилок активирован.");
        }
        else
        {
            Debug.LogWarning("DropOnKill не назначен в инспекторе!");
        }
    }
}
