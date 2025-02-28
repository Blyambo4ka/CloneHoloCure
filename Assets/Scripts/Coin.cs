using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public float coinValue = 1f;
    private Animator animator;

    void Start()
    {
        // Получаем компонент Animator
        animator = GetComponent<Animator>();

        // Запускаем корутину для анимации IdleM
        StartCoroutine(PlayIdleMCoroutine());
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            MovementPlayer playerMovement = collider.gameObject.GetComponent<MovementPlayer>();
            if (playerMovement != null)
            {
                // Добавляем монетки игроку
                playerMovement.AddMoney(coinValue);

                // Ищем объект CoinManager на сцене и добавляем монетки туда
                CoinManager coinManager = FindObjectOfType<CoinManager>();
                if (coinManager != null)
                {
                    coinManager.AddCoins((int)coinValue); // Приводим к int, если нужно
                }

                // Уничтожаем монетку
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator PlayIdleMCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f); // Ждём 3 секунды
            if (animator != null)
            {
                animator.SetTrigger("PlayIdleM");
            }
        }
    }
}
