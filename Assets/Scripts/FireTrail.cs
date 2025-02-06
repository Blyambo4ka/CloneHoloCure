using UnityEngine;
using System.Collections;

public class FireTrail : MonoBehaviour
{
    private int fireDamage;
    private float duration;
    private float damageInterval = 0.5f;

    private Animator anim; // Ссылка на аниматор

    public void Initialize(int damage, float trailDuration)
    {
        fireDamage = damage;
        duration = trailDuration;
        anim = GetComponent<Animator>(); // Получаем аниматор
        StartCoroutine(DealDamage());
        StartCoroutine(DestroyAfterAnimation()); // Запускаем удаление после анимации
    }

    IEnumerator DealDamage()
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += damageInterval;

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (Collider2D enemyCollider in hitEnemies)
            {
                if (enemyCollider.CompareTag("Enemy"))
                {
                    Enemy enemy = enemyCollider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(fireDamage);
                        Debug.Log($"FireTrail dealt {fireDamage} damage to {enemy.name}");
                    }
                }
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }

    IEnumerator DestroyAfterAnimation()
    {
        if (anim != null)
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // Ждём конец анимации
        }
        else
        {
            yield return new WaitForSeconds(duration); // Если анимации нет, удаляем через fireDuration
        }

        Destroy(gameObject);
    }
}
