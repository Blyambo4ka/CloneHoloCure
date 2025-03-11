using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    public float speedMultiplier = 1.2f; // Увеличение скорости на 20% (1.2 = 120%)
    public float attackRateMultiplier = 1.2f; // Увеличение скорости атаки на 20% (1.2 = 120%)
    public int damagePerSecond = 2; // Урон игроку каждую секунду

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MovementPlayer player = collision.GetComponent<MovementPlayer>();
            if (player != null)
            {
                // Увеличиваем скорость движения
                player.speed *= speedMultiplier;

                // Увеличиваем скорость атаки
                player.attackRate /= attackRateMultiplier; // Уменьшаем интервал между атаками для увеличения скорости

                // Запускаем периодический урон
                player.StartCoroutine(ApplyPeriodicDamage(player));

                Debug.Log("Speed Boost applied! Speed and attack rate increased.");
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