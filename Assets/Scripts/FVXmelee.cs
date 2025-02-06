using UnityEngine;

public class AttackHit : MonoBehaviour
{
    public int damage = 20;
    public float attackDirection;

    void Start()
    {
        MovementPlayer player = FindObjectOfType<MovementPlayer>();
        if (player != null)
        {
            damage = player.attackDamage; // Берем урон от игрока
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Enemy2") || collider.gameObject.CompareTag("Enemy3"))
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Теперь вызываем TakeDamage на полученном компоненте enemy
            }
        }
    }
}

