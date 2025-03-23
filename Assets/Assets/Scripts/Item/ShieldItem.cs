using UnityEngine;

public class ShieldItem : MonoBehaviour
{
    public float shieldAmount = 50f; // Количество щита, которое добавляет предмет
    public Sprite itemIcon; // Иконка предмета
    public string itemName = "Shield Boost"; // Название предмета
    public string itemDescription = "Добавляет 50 единиц щита."; // Описание предмета

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
             if (ItemInventoryUIManager.Instance.IsInventoryFull())
        {
            Debug.LogWarning("Инвентарь заполнен! Нельзя подобрать предмет.");
            return; // Предмет не активируется и остаётся на сцене
        }
            MovementPlayer player = collision.GetComponent<MovementPlayer>();
            if (player != null)
            {
                player.AddShield(shieldAmount);
                Debug.Log("Shield added! Current shield: " + player.shield);

                // Добавляем предмет в инвентарь
                ItemInventoryManager.Instance.AddItem(itemName, itemIcon, itemDescription);

                Destroy(gameObject); // Уничтожаем предмет после подбора
            }
        }
    }
}
