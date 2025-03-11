using UnityEngine;

public class ShieldItem : MonoBehaviour
{
    public float shieldAmount = 50f; // Количество щита, которое добавляет предмет

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MovementPlayer player = collision.GetComponent<MovementPlayer>();
            if (player != null)
            {
                player.AddShield(shieldAmount);
                Debug.Log("Shield added! Current shield: " + player.shield);
                Destroy(gameObject); // Уничтожаем предмет после подбора
            }
        }
    }
}