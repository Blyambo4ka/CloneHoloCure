using UnityEngine;

public class ExperienceBoostItem : MonoBehaviour
{
    public float experienceMultiplier = 0.9f; // Срез опыта до следующего уровня на 10%
    private bool isCollected = false;

    public Sprite itemIcon; // Иконка предмета
    public string itemName = "Experience Boost"; // Название предмета
    public string itemDescription = "Уменьшает необходимый опыт для следующего уровня на 10%."; // Описание предмета

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected)
        {
             if (ItemInventoryUIManager.Instance.IsInventoryFull())
        {
            Debug.LogWarning("Инвентарь заполнен! Нельзя подобрать предмет.");
            return; // Предмет не активируется и остаётся на сцене
        }
            MovementPlayer player = collision.GetComponent<MovementPlayer>();
            if (player != null)
            {
                ApplyExperienceBoost(player);
                isCollected = true;

                // Добавляем предмет в инвентарь
                ItemInventoryManager.Instance.AddItem(itemName, itemIcon, itemDescription);

                Destroy(gameObject); // Уничтожаем предмет после подбора
            }
        }
    }

    private void ApplyExperienceBoost(MovementPlayer player)
    {
        // Уменьшаем необходимый опыт для следующего уровня на 10%
        player.experienceToNextLevel *= experienceMultiplier;
        Debug.Log("Experience boost applied! New experience requirement: " + player.experienceToNextLevel);
    }
}
