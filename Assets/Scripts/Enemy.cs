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
        this.experienceAmount = experienceAmount;
        this.coinAmount = coinAmount;
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

   public void TakeDamage(int damageAmount)
    {
        if (Time.time - lastDamageReceiveTime >= damageReceiveInterval)
        {
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

        isDead = true;  // Отметим врага как мертвого

        // Уничтожаем все активные тексты урона
        foreach (GameObject damageText in activeDamageTexts)
        {
            if (damageText != null)
            {
                Destroy(damageText);
            }
        }
        activeDamageTexts.Clear(); // Очищаем список после уничтожения текстов

        // Всё, что связано с уничтожением врага, теперь нужно делать в этом методе
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // Разлетающиеся монеты
        for (int i = 0; i < coinAmount; i++)
        {
            if (coinPrefab != null)
            {
                Vector2 randomOffset = Random.insideUnitCircle * 0.5f; // Разлет в радиусе 0.5
                GameObject coin = Instantiate(coinPrefab, transform.position + (Vector3)randomOffset, Quaternion.identity);
                ApplyRandomForce(coin);
            }
        }

        // Создание одного объекта опыта с передачей ему общей суммы опыта
        if (experiencePrefab != null)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f; // Разлет в радиусе 0.5
            GameObject experience = Instantiate(experiencePrefab, transform.position + (Vector3)randomOffset, Quaternion.identity);

            // Передаем количество опыта в объект
            Experience experienceScript = experience.GetComponent<Experience>();
            if (experienceScript != null)
            {
                experienceScript.experienceValue = experienceAmount;
            }

            ApplyRandomForce(experience);
        }

        if (spawner != null)
        {
            spawner.ReturnEnemyToPool(gameObject);
        }
        else
        {
            Debug.LogError("Spawner not set!");
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
