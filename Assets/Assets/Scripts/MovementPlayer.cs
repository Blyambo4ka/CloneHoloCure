using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

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
    public float experience = 0f; // Текущий опыт
    public float experienceToNextLevel = 300f; // Опыт для следующего уровня
    public Image experienceBar;
    public UIManager uiManager;
    public int attackDamage = 10;
    
    public int level = 1; // Уровень игрока
    public float healthIncreasePerLevel = 3f; // Увеличение здоровья за каждый уровень

    private bool isLevelingUp = false; // Флаг, чтобы избежать повторного появления панели

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

    public void TakeDamage(int damage)
    {
        HP -= damage;
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
        experienceToNextLevel *= 1.5f;

        // Увеличиваем здоровье и урон
        HP += healthIncreasePerLevel;

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
