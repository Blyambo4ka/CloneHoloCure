using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class MovementPlayer : MonoBehaviour
{
    public float speed;
    private Vector2 direction;
    private Rigidbody2D rb;
    public Animator animator;
    public float HP = 100;
    public GameObject attackPrefab;
    public float attackDistance = 0.8f;
    public float attackRate = 0.2f;
    private float attackTimer = 0f;
    private float money = 0f;
    public TextMeshProUGUI moneyText;
    public Transform feetPosition;
    public GameObject footstepEffectPrefab;
    public float footstepInterval = 0.3f;
    private float lastFootstepTime;
    public float footstepDuration = 0.2f;
    public float maxHealth = 200;
    public float experience = 0f; // Текущий опыт
    public float experienceToNextLevel = 300f; // Опыт для следующего уровня
    public Image experienceBar;
    public UIManager uiManager;
    public int attackDamage = 10;
    
    public int level = 1; // Уровень игрока
    public float healthIncreasePerLevel = 3f; // Увеличение здоровья за каждый уровень

    private bool isLevelingUp = false; // Флаг, чтобы избежать повторного появления панели

    public bool isInvincible = false; // Добавляем флаг неуязвимости
    public float maxShield = 100f; // Максимальный щит
    public float shield = 0f; // Текущий щит
    public float shieldRegenRate = 5f; // Скорость восстановления щита в секунду
    public float shieldRegenDelay = 4f; // Задержка перед восстановлением щита (в секундах)
    private float timeSinceLastDamage = 0f; // Время с момента последнего получения урона
    public bool isShieldActive = false; // Флаг для проверки, активен ли щит

   
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        lastFootstepTime = -footstepInterval;
    }

    void Update()
    {
        if (!isLevelingUp)
        {
            HandleMovement();
            HandleAttack();
            CheckHealth();
            UpdateMoneyText();
            UpdateExperienceBar();
        }
        if (isShieldActive && shield < maxShield && timeSinceLastDamage >= shieldRegenDelay)
        {
            shield += shieldRegenRate * Time.deltaTime;
            if (shield > maxShield)
            {
                shield = maxShield;
            }
        }

        // Обновляем таймер
        timeSinceLastDamage += Time.deltaTime;
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    private void HandleMovement()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        bool isMoving = direction.magnitude > 0;
        animator.SetBool("isMooving", isMoving);

        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (isMoving)
        {
            TryCreateFootstepEffect();
        }
    }

    public void Heal(float amount)
    {
        if (HP >= maxHealth) // Если здоровье уже максимальное, лечение не требуется
        {
            
            return;
        }

        HP += amount; // Увеличиваем здоровье
        HP = Mathf.Clamp(HP, 0, maxHealth); // Ограничиваем здоровье максимальным значением
        
    }

    private void ApplyMovement()
    {
        Vector2 targetVelocity = direction * speed;
        rb.linearVelocity = targetVelocity;
    }

    private void HandleAttack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackRate)
        {
            Attack();
            attackTimer = 0;
        }
    }

    private void Attack()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 attackDirection = (mousePosition - transform.position).normalized;
        Vector3 attackPosition = transform.position + (Vector3)attackDirection * attackDistance;
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;

        GameObject attack = Instantiate(attackPrefab, attackPosition, Quaternion.Euler(0, 0, angle));
        attack.transform.Rotate(0, 0, -90);

        AttackHit attackHit = attack.GetComponent<AttackHit>();
        attackHit.attackDirection = Mathf.Sign(attackDirection.x);
        attack.transform.localScale = new Vector3(Mathf.Sign(attackDirection.x), 1, 1);

        Destroy(attack, 0.4f);
    }

    public void IncreaseDamage(int amount)
    {
        attackDamage += amount;
    }

    public void AddShield(float amount)
    {
        if (!isShieldActive)
        {
            isShieldActive = true; // Активируем щит
        }

        shield += amount;

        if (shield > maxShield)
        {
            shield = maxShield;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // Если игрок неуязвим, урон не проходит

        // Сбрасываем таймер при получении урона
        timeSinceLastDamage = 0f;

        // Сначала урон поглощается щитом
        if (isShieldActive && shield > 0)
        {
            shield -= damage;
            if (shield < 0)
            {
                // Если щит закончился, остаток урона переходит на здоровье
                HP += shield; // shield отрицательный, поэтому +=
                shield = 0;
            }
        }
        else
        {
            // Если щита нет, урон наносится здоровью
            HP -= damage;
        }

        CheckHealth();
    }


   

    private void CheckHealth()
    {
        if (HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene("Menu");
        FindObjectOfType<CoinManager>().EndGameAndSaveCoins();
    }

    public void AddMoney(float value)
    {
        money += value;
    }

    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = "" + money;
        }
    }

    private void TryCreateFootstepEffect()
    {
        if (Time.time - lastFootstepTime >= footstepInterval)
        {
            if (footstepEffectPrefab != null && feetPosition != null)
            {
                GameObject footstepEffect = Instantiate(footstepEffectPrefab, feetPosition.position, Quaternion.identity);
                Destroy(footstepEffect, footstepDuration);
            }
            lastFootstepTime = Time.time;
        }
    }

    public void AddExperience(int value)
    {
        experience += value;
        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        isLevelingUp = true; // Включаем флаг, чтобы запретить действия во время повышения уровня

        // Приостанавливаем игру
        Time.timeScale = 0f;

        // Увеличиваем уровень
        level++;
        experience -= experienceToNextLevel;
        experienceToNextLevel *= 1.05f;

        // Увеличиваем здоровье
        HP = Mathf.Min(HP + healthIncreasePerLevel, maxHealth);

        // Показываем панель повышения уровня
        uiManager.ShowLevelUpPanel();

        Debug.Log("Level Up! Current level: " + level);
    }

    public void ResumeGame() // Этот метод вызывается для возобновления игры
    {
        // Возвращаем время в нормальное состояние
        Time.timeScale = 1f;
        isLevelingUp = false; // Снимаем флаг
    }

    private void UpdateExperienceBar()
    {
        if (experienceBar != null)
        {
            experienceBar.fillAmount = experience / experienceToNextLevel;
        }
    }
}
