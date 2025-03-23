using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    public float speedMultiplier = 1.2f; // Увеличение скорости на 20% (1.2 = 120%)
    public float attackRateMultiplier = 1.2f; // Увеличение скорости атаки на 20% (1.2 = 120%)
    public int damagePerSecond = 2; // Урон игроку каждую секунду

    public Sprite itemIcon; // Иконка предмета
    public string itemName = "Speed Boost"; // Название предмета
    public string itemDescription = "Увеличивает скорость и скорость атаки, но наносит 2 урона в секунду."; // Описание предмета

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
            if (ItemInventoryUIManager.Instance.IsInventoryFull())
            {
                Debug.LogWarning("Инвентарь заполнен! Нельзя подобрать предмет.");
                return; // Предмет не активируется и остаётся на сцене
            }
            if (player != null)
            {
                // Увеличиваем скорость движения
                player.speed *= speedMultiplier;

                // Увеличиваем скорость атаки
                player.attackRate /= attackRateMultiplier; // Уменьшаем интервал между атаками для увеличения скорости

                // Запускаем периодический урон
                player.StartCoroutine(ApplyPeriodicDamage(player));

                Debug.Log("Speed Boost applied! Speed and attack rate increased.");

                // Добавляем предмет в инвентарь
                ItemInventoryManager.Instance.AddItem(itemName, itemIcon, itemDescription);

                Destroy(gameObject); // Уничтожаем предмет после подбора
            }
        }
    }

    // Корутина для нанесения периодического урона
    private System.Collections.IEnumerator ApplyPeriodicDamage(MovementPlayer player)
    {
        while (true)
        {
            if (player.HP <= 0)
            {
                yield break; // Прекращаем корутину, если игрок мертв
            }

            player.TakeDamage(damagePerSecond); // Наносим урон
            Debug.Log($"Player took {damagePerSecond} damage. Current HP: {player.HP}");

            yield return new WaitForSeconds(1f); // Ждем 1 секунду перед нанесением следующего урона
        }
    }
}
