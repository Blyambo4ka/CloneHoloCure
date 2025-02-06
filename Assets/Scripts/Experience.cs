using System.Collections;
using UnityEngine;

public class Experience : MonoBehaviour
{
    public int experienceValue = 20;
    public float moveSpeed = 2f;
    private Transform player;
    public float delayBeforeReturn = 0.1f;
    public float startMoveDelay = 0.5f;
    public float startMoveRadius = 2f;
    private bool canMove = false;
    private float timeSpawned = 0f;
    private bool isMoving = false;
    private bool collected = false;

    private Animator animator; // Поле для аниматора

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        canMove = false;
        isMoving = false;
        timeSpawned = Time.time;

        animator = GetComponent<Animator>(); // Получаем аниматор

        StartCoroutine(PlayIdleAnimationLoop()); // Запускаем цикл анимации Idle
    }

    void Update()
    {
        if (Time.time - timeSpawned >= startMoveDelay)
        {
            CheckForMove();
            if (canMove)
            {
                MoveTowardsPlayer();
            }
        }
    }

    private IEnumerator PlayIdleAnimationLoop()
    {
        while (true)
        {
            if (animator != null)
            {
                animator.SetTrigger("exp"); // Запускаем анимацию Idle
            }
            yield return new WaitForSeconds(2f); // Ждем 2 секунды перед повтором
        }
    }

    private void CheckForMove()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            canMove = distance <= startMoveRadius;
            isMoving = canMove;
        }
        else
        {
            canMove = false;
            isMoving = false;
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player != null && isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collected) return;
        if (collider.CompareTag("Player"))
        {
            collected = true;
            MovementPlayer playerMovement = collider.GetComponent<MovementPlayer>();
            if (playerMovement != null)
            {
                playerMovement.AddExperience(experienceValue);
                StartCoroutine(ReturnToPoolDelayed());
            }
        }
    }

    private IEnumerator ReturnToPoolDelayed()
    {
        yield return new WaitForSeconds(delayBeforeReturn);
        ObjectPooler.Instance.ReturnToPool("Experience", gameObject);
    }

    // Метод для сброса состояния опыта перед возвратом в пул
    public void ResetExperience()
    {
        experienceValue = 20;  // Сбросить начальное значение опыта
        canMove = false;
        isMoving = false;
        collected = false;
        transform.position = Vector3.zero;  // Можно также сбросить позицию, если это необходимо
    }
}
