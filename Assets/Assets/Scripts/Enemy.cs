using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro; 

public class Enemy : MonoBehaviour
{
    public float speed;
    public int damage = 10;
    [SerializeField] protected Transform player;
    protected SpriteRenderer spriteRenderer;
    public int HP = 10;

    public float firstAttackDelay = 1.5f; // Задержка перед первой атакой
    private float spawnTime;

    public float damageCooldown = 1f;
    private float lastDamageTime;

    public GameObject coinPrefab;
    public float damageReceiveInterval = 0.3f;
    private float lastDamageReceiveTime;
    private Spawner spawner;

    public GameObject deathEffectPrefab;
    public GameObject experiencePrefab;

    private Animator animator;

    public int experienceAmount = 20;
    public int coinAmount = 1;

    private int enemyIndex;
    private bool isInitialized = false;
    public int EnemyIndex => enemyIndex;

    public GameObject damageTextPrefab; // Префаб текста урона

    private bool isDead = false;  // Флаг для проверки, мертв ли враг
    private List<GameObject> activeDamageTexts = new List<GameObject>();

    public DropOnKill dropOnKill; // Ссылка на предмет, который добавляет шанс выпадения

    private GameObject currentShieldVisual;
    private int shieldHP; // Здоровье щита
    private float shieldEndTime; // Время окончания действия щита
    public string GetEnemyTag()
        {
            return gameObject.tag; // Возвращает тег объекта
        }



    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetEnemyIndex(int index)
    {
        enemyIndex = index;
    }

    void OnEnable()
    {
        lastDamageTime = -damageCooldown;
        lastDamageReceiveTime = -damageReceiveInterval;
        spawnTime = Time.time; // Запоминаем время появления

        if (!isInitialized)
        {
            Debug.LogWarning("Enemy not initialized properly!");
            return;
        }

        ResetState();
    }

    public virtual void Initialize(Transform player, Spawner spawner, int hp, int experienceAmount, int coinAmount, int enemyIndex)
    {
        this.player = player;
        this.spawner = spawner;
        this.HP = hp;
        this.enemyIndex = enemyIndex;
        this.isInitialized = true; // Флаг инициализации

        Debug.Log($"Enemy initialized: HP = {hp}, XP = {experienceAmount}, Coins = {coinAmount}, Index = {enemyIndex}");
    }

    private void ResetState()
    {
        if (HP <= 0)
        {
            HP = 10;
        }
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            spriteRenderer.flipX = player.position.x < transform.position.x;
        }

        if (HP <= 0)
        {
            KillEnemy();
        }
        
    }
    public void ApplyShield(GameObject shieldVisualPrefab, int shieldHP, float shieldDuration)
    {
        // Если щит уже активен, отменяем предыдущий
        if (currentShieldVisual != null)
        {
            Destroy(currentShieldVisual);
        }

        // Создаем визуальный префаб щита
        currentShieldVisual = Instantiate(shieldVisualPrefab, transform.position, Quaternion.identity, transform);
        currentShieldVisual.transform.localPosition = Vector3.up * 0f; // Размещаем над врагом

        // Устанавливаем параметры щита
        this.shieldHP = shieldHP;
        shieldEndTime = Time.time + shieldDuration;

        // Запускаем проверку состояния щита
        StartCoroutine(CheckShield());
    }

    private IEnumerator CheckShield()
    {
        while (Time.time < shieldEndTime && shieldHP > 0)
        {
            yield return null; // Ждем следующего кадра
        }

        // Удаляем щит, если время истекло или HP щита <= 0
        RemoveShield();
    }

    private void RemoveShield()
    {
        if (currentShieldVisual != null)
        {
            Destroy(currentShieldVisual);
            currentShieldVisual = null;
        }
    }

    public void TakeShieldDamage(int damage)
    {
        if (currentShieldVisual != null)
        {
            shieldHP -= damage;
            if (shieldHP <= 0)
            {
                RemoveShield(); // Удаляем щит, если его HP <= 0
            }
        }
    }



    void OnTriggerStay2D(Collider2D collider)
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

    public virtual void TakeDamage(int damageAmount)
    {
        if (Time.time - lastDamageReceiveTime >= damageReceiveInterval)
        {
            if (currentShieldVisual != null)
            {
                // Если щит активен, урон наносится щиту
                TakeShieldDamage(damageAmount);
            }
            else
            {
                // Если щита нет, урон наносится врагу
                HP -= damageAmount;
                lastDamageReceiveTime = Time.time;

                if (animator != null)
                {
                    animator.SetTrigger("Hit"); // Запуск анимации
                }

                // Создаём текст урона
                ShowDamageText(damageAmount);
            }
        }
    }



    public void ShowDamageText(int damageAmount)
    {
        if (isDead)
            return;  // Если враг мертв, текст не показывается

        if (damageTextPrefab != null)
        {
            // Находим Canvas в сцене
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("Canvas не найден в сцене!");
                return;
            }

            // Создаём экземпляр текста
            GameObject damageText = Instantiate(damageTextPrefab, canvas.transform, false);

            // Позиция над врагом
            RectTransform rectTransform = damageText.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Переводим мировые координаты в локальные координаты Canvas
                Vector3 worldPosition = transform.position + new Vector3(0, 1f, 0); // Координата над врагом
                Vector2 localPosition;
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    Camera.main.WorldToScreenPoint(worldPosition),
                    canvas.worldCamera,
                    out localPosition))
                {
                    rectTransform.localPosition = localPosition; // Устанавливаем локальную позицию
                }
                else
                {
                    Debug.LogError("Не удалось преобразовать координаты в локальные!");
                }
            }

            // Устанавливаем текст
            TextMeshProUGUI damageTextComponent = damageText.GetComponentInChildren<TextMeshProUGUI>();
            if (damageTextComponent != null)
            {
                damageTextComponent.text = damageAmount.ToString();
            }
            else
            {
                Debug.LogError("Компонент TextMeshProUGUI отсутствует на префабе текста урона!");
            }

            // Анимацию запускает сам Animator, никаких дополнительных действий не требуется.
        }
        else
        {
            Debug.LogError("Префаб текста урона не привязан!");
        }
    }

    public virtual void KillEnemy()
    {
        if (isDead)
            return;  // Если враг уже мертв, не повторяем убийство

        isDead = true;  // Отмечаем врага как мертвого

        

        // Уничтожаем все активные тексты урона
        foreach (GameObject damageText in activeDamageTexts)
        {
            if (damageText != null)
            {
                Destroy(damageText);
            }
        }
        activeDamageTexts.Clear(); // Очищаем список после уничтожения текстов

        // Запускаем анимацию смерти
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // Отключаем взаимодействие врага, чтобы он не мог больше наносить урон
        GetComponent<Collider2D>().enabled = false; // Отключение коллайдера
        speed = 0; // Останавливаем движение

        // Задержка перед уничтожением объекта для завершения анимации
        StartCoroutine(DeathMovement());
    }   

    private IEnumerator DeathMovement()
    {
        float duration = 0.1f; // Длительность движения вправо
        float elapsedTime = 0f;
        float moveDistance = 0.2f; // Расстояние, на которое нужно сместиться вправо
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(moveDistance, 0, 0); // Конечная позиция

        while (elapsedTime < duration)
        {
            // Линейно интерполируем позицию от startPosition до targetPosition
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // После завершения движения запускаем эффекты уничтожения
        StartCoroutine(HandleDeathEffects());
    }
    private IEnumerator HandleDeathEffects()
    {
        // Ждём завершения анимации смерти
        yield return new WaitForSeconds(0.3f); // Замените 1.5f на длительность вашей анимации

        // Эффекты уничтожения врага (монеты, опыт и т.п.)
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        for (int i = 0; i < coinAmount; i++)
        {
            if (coinPrefab != null)
            {
                Vector2 randomOffset = Random.insideUnitCircle * 0.5f; // Разлет в радиусе 0.5
                GameObject coin = Instantiate(coinPrefab, transform.position + (Vector3)randomOffset, Quaternion.identity);
                ApplyRandomForce(coin);
            }
        }

        if (experiencePrefab != null)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f; // Разлет в радиусе 0.5
            GameObject experience = Instantiate(experiencePrefab, transform.position + (Vector3)randomOffset, Quaternion.identity);

            Experience experienceScript = experience.GetComponent<Experience>();
            if (experienceScript != null)
            {
                experienceScript.experienceValue = experienceAmount;
            }

            ApplyRandomForce(experience);
        }

        if (dropOnKill != null)
        {
            dropOnKill.TryDrop(transform.position);
        }

        // Возвращаем врага в пул или уничтожаем его
        if (spawner != null)
        {
            spawner.ReturnEnemyToPool(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    // Метод для добавления случайного импульса объекту
    private void ApplyRandomForce(GameObject obj)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomForce = Random.insideUnitCircle * 2f; // Сила импульса
            rb.AddForce(randomForce, ForceMode2D.Impulse);
        }
    }
}
