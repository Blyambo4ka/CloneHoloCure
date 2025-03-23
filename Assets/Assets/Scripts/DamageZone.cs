using UnityEngine;
using System.Collections;

public class DamageZone : MonoBehaviour
{
    private int damage;
    private float duration;
    private float spawnTime; // Время создания зоны
    private float lastDamageTime; // Время последнего нанесенного урона
    public float firstAttackDelay = 0.2f; // Задержка перед первым уроном
    public float damageCooldown = 1f; // КД между ударами

    public void Initialize(int zoneDamage, float zoneDuration)
    {
        damage = zoneDamage;
        duration = zoneDuration;
        spawnTime = Time.time; // Запоминаем время создания зоны
        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            TryApplyDamage(collider.gameObject);
        }
    }

    private void TryApplyDamage(GameObject playerObject)
    {
        // Проверяем, прошло ли достаточно времени с момента появления
        if (Time.time - spawnTime < firstAttackDelay)
        {
            return; // Если время меньше задержки первой атаки, не наносим урон
        }

        // Проверяем КД между ударами
        if (Time.time - lastDamageTime >= damageCooldown)
        {
            MovementPlayer playerMovement = playerObject.GetComponent<MovementPlayer>();
            if (playerMovement != null)
            {
                playerMovement.TakeDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }
}
