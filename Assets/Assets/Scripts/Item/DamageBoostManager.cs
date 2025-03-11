using UnityEngine;

public class DamageBoostManager : MonoBehaviour
{
    public static bool isBoostActive = false; // Глобальный флаг: активирован ли предмет
    public static float boostRadius = 2f; // Радиус поиска врагов
    public static int damageIncreasePerEnemy = 1; // Увеличение урона за каждого врага

     // Переменные для замедления атаки
    private static float originalAttackRate; // Исходная скорость атаки игрока
    private const float attackSlowMultiplier = 1.4f; // Множитель замедления атаки (40% замедление = 60% от исходной скорости)

    // Метод для активации предмета
    public static void ActivateBoost(MovementPlayer player)
    {
        if (player != null)
        {
            isBoostActive = true;

            // Сохраняем исходную скорость атаки
            originalAttackRate = player.attackRate;

            // Уменьшаем скорость атаки на 40%
            player.attackRate *= attackSlowMultiplier;

            Debug.Log("Предмет активирован! Скорость атаки уменьшена на 40%.");
        }
    }

    public static int CalculateBoostedDamage(int baseDamage, Transform playerPosition)
    {
        if (!isBoostActive)
        {
            Debug.Log("Усиление не активно, базовый урон: " + baseDamage);
            return baseDamage;
        }

        // Визуализация радиуса
        Debug.DrawLine(playerPosition.position, playerPosition.position + Vector3.right * boostRadius, Color.red, 1f);
        Debug.DrawLine(playerPosition.position, playerPosition.position + Vector3.left * boostRadius, Color.red, 1f);
        Debug.DrawLine(playerPosition.position, playerPosition.position + Vector3.up * boostRadius, Color.red, 1f);
        Debug.DrawLine(playerPosition.position, playerPosition.position + Vector3.down * boostRadius, Color.red, 1f);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(playerPosition.position, boostRadius);
        int enemyCount = 0;

        foreach (Collider2D collider in enemies)
        {
            if (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Enemy2") ||
                collider.gameObject.CompareTag("Enemy3") || collider.gameObject.CompareTag("Enemy4") ||
                collider.gameObject.CompareTag("Enemy5") || collider.gameObject.CompareTag("Enemy6") ||
                collider.gameObject.CompareTag("Enemy7"))
            {
                enemyCount++;
            }
        }

        int newDamage = baseDamage + (enemyCount * damageIncreasePerEnemy);
        Debug.Log($"Усиление активно! Врагов: {enemyCount}, новый урон: {newDamage}");

        return newDamage;
    }
}