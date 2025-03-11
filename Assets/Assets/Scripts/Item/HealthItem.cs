using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public float healAmount = 20f; // Количество здоровья, которое восстанавливает предмет
    public AudioClip healSound; // Звук лечения
    public ParticleSystem healEffect; // Эффект частиц при лечении

    private Animator animator; // Ссылка на Animator
    private bool isPickedUp = false; // Флаг для отслеживания, подобран ли предмет

    void Start()
    {
        animator = GetComponent<Animator>(); // Получаем компонент Animator
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !isPickedUp) // Проверяем, что предмет подобрал игрок и ещё не был подобран
        {
            MovementPlayer player = collider.GetComponent<MovementPlayer>();
            if (player != null)
            {
                player.Heal(healAmount); // Лечим игрока

                // Воспроизводим звук
                if (healSound != null)
                {
                    AudioSource.PlayClipAtPoint(healSound, transform.position);
                }

                // Воспроизводим эффект частиц
                if (healEffect != null)
                {
                    Instantiate(healEffect, transform.position, Quaternion.identity);
                }

                // Запускаем анимацию подбора
                isPickedUp = true;
                animator.SetBool("IsPickedUp", true);

                // Уничтожаем предмет после завершения анимации
                Destroy(gameObject, 0.1f); // Задержка для завершения анимации
            }
        }
    }
}