using UnityEngine;

public class AttackHit : MonoBehaviour
{
    public int damage = 20; // Урон врагу
    public float attackDirection;
    public float slowChance = 20f; // Шанс замедления в процентах
    public float slowDuration = 3f; // Длительность замедления

    void Start()
    {
        MovementPlayer player = FindObjectOfType<MovementPlayer>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Проверяем, попали ли мы во врага
        if (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Enemy2") ||
            collider.gameObject.CompareTag("Enemy3") || collider.gameObject.CompareTag("Enemy4") ||
            collider.gameObject.CompareTag("Enemy5") || collider.gameObject.CompareTag("Enemy6") ||
            collider.gameObject.CompareTag("Enemy7"))
        {
            MovementPlayer player = FindObjectOfType<MovementPlayer>();
            if (player != null)
            {
                damage = DamageBoostManager.CalculateBoostedDamage(player.attackDamage, player.transform);
                Debug.Log("Пересчитанный урон перед атакой: " + damage);
            }
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Наносим урон врагу
                enemy.TakeDamage(damage);

                // Проверяем, активирован ли предмет замедления
                   if (SlowItem.isActivated)
                    {
                        Debug.Log("Предмет активирован! Проверяем шанс замедления...");
                        if (Random.Range(0f, 100f) < slowChance)
                        {
                            Debug.Log("Замедление сработало!");
                            SlowEffect slowEffect = enemy.GetComponent<SlowEffect>();
                            if (slowEffect != null)
                            {
                                slowEffect.ApplySlowEffect(slowDuration);
                            }
                        }
                    }
            }
        }
    }
}