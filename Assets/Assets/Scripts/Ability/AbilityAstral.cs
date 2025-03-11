using UnityEngine;
using UnityEngine.UI; // Для работы с UI элементами
using System.Collections;

public class AstralAbility : MonoBehaviour
{
    public float baseDuration = 1f;  // Базовая длительность астрала
    public float baseCooldown = 12f;  // Базовый кулдаун

    private float currentDuration;
    private float currentCooldown;
    private int upgradeLevel = 0;
    private const int maxUpgradeLevel = 5;

    public bool isUnlocked = false;
    private bool isAstral = false;
    private float lastUsedTime = -Mathf.Infinity;

    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;

    private MovementPlayer player; // Ссылка на скрипт игрока

    // Ссылка на UI панель
    public GameObject astralEffectPanel; // Панель для фиолетового экрана
    private Image astralImage; // Ссылка на компонент Image панели
    public Sprite abilityIcon; // Иконка способности

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();

        player = GetComponent<MovementPlayer>(); // Получаем компонент игрока

        // Получаем компонент Image из панели
        astralImage = astralEffectPanel.GetComponent<Image>();

        currentDuration = baseDuration;
        currentCooldown = baseCooldown;

        // Изначально панель прозрачная
        astralImage.color = new Color(0.5f, 0f, 0.5f, 0f); // Фиолетовый с нулевой прозрачностью
    }

    void Update()
    {
        if (isUnlocked && Input.GetKeyDown(KeyCode.E))
        {
            TryActivateAstral();
        }
    }



    public void UnlockAbility()
    {
        if (isUnlocked) return;

        isUnlocked = true;

        // Добавляем способность в инвентарь
        InventoryUIManager.Instance.AddAbilityToUI("AstralAbility", abilityIcon, upgradeLevel);

        // Находим UI-элемент способности и изменяем его размер
        if (InventoryUIManager.Instance.abilityUIElements.TryGetValue("AstralAbility", out GameObject abilityUI))
        {
            Image abilityImage = abilityUI.GetComponentInChildren<Image>();
            if (abilityImage != null)
            {
                RectTransform rectTransform = abilityImage.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(46, 50);
            }
        }

        Debug.Log("Astral ability unlocked!");
    }


    public void UpgradeAbility()
    {
        if (!isUnlocked || upgradeLevel >= maxUpgradeLevel) return;

        upgradeLevel++;
        currentDuration += 0.2f;
        currentCooldown = Mathf.Max(7f, currentCooldown - 1f);

        // Обновляем уровень способности в инвентаре
        InventoryUIManager.Instance.UpdateAbilityLevelInUI("AstralAbility", upgradeLevel);

        Debug.Log($"Astral upgraded! Level: {upgradeLevel}, Duration: {currentDuration}, Cooldown: {currentCooldown}");
    }


        private void TryActivateAstral()
        {
            if (isAstral || Time.time - lastUsedTime < currentCooldown)
                return;

            StartCoroutine(ActivateAstral());
        }

    private IEnumerator ActivateAstral()
    {
        isAstral = true;
        lastUsedTime = Time.time;

        playerCollider.enabled = false;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        player.isInvincible = true; // Делаем игрока неуязвимым

        // Плавное включение фиолетового экрана
        float lerpTime = 0.5f; // Время для плавного перехода
        float elapsedTime = 0f;

        while (elapsedTime < lerpTime)
        {
            astralImage.color = Color.Lerp(new Color(0.5f, 0f, 0.5f, 0f), new Color(0.5f, 0f, 0.5f, 0.2f), elapsedTime / lerpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // После окончания активации астрала, ждем в течение duration
        yield return new WaitForSeconds(currentDuration);

        playerCollider.enabled = true;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        player.isInvincible = false; // Убираем неуязвимость

        // Плавное выключение фиолетового экрана
        elapsedTime = 0f;

        while (elapsedTime < lerpTime)
        {
            astralImage.color = Color.Lerp(new Color(0.5f, 0f, 0.5f, 0.2f), new Color(0.5f, 0f, 0.5f, 0f), elapsedTime / lerpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isAstral = false;
    }
}