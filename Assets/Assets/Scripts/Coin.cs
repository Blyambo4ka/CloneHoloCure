using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public float coinValue = 1f;
    public float moveSpeed = 2f; // Скорость движения к игроку
    public float startMoveDelay = 0.5f; // Задержка перед началом движения
    public float startMoveRadius = 2f; // Радиус, при котором монетка начинает двигаться к игроку
    private Transform player; // Ссылка на игрока
    private bool canMove = false; // Флаг, может ли монетка двигаться
    private float timeSpawned = 0f; // Время появления монетки
    private bool collected = false; // Флаг, собрана ли монетка

    private Animator animator; // Компонент аниматора

    void Start()
    {
        // Получаем компонент Animator
        animator = GetComponent<Animator>();

        // Находим игрока
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Запоминаем время появления
        timeSpawned = Time.time;

        // Запускаем корутину для анимации IdleM
        StartCoroutine(PlayIdleMCoroutine());
    }

    void Update()
    {
        // Если прошло достаточно времени, проверяем, может ли монетка двигаться
        if (Time.time - timeSpawned >= startMoveDelay)
        {
            CheckForMove();
            if (canMove)
            {
                MoveTowardsPlayer();
            }
        }
    }

    // Проверка, может ли монетка двигаться к игроку
    private void CheckForMove()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            canMove = distance <= startMoveRadius;
        }
        else
        {
            canMove = false;
        }
    }

    // Движение монетки к игроку
    private void MoveTowardsPlayer()
    {
        if (player != null && canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    // Обработка столкновения с игроком
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collected) return; // Если монетка уже собрана, выходим

        if (collider.CompareTag("Player"))
        {
            collected = true; // Отмечаем, что монетка собрана

            MovementPlayer playerMovement = collider.GetComponent<MovementPlayer>();
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

    // Корутина для анимации IdleM
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