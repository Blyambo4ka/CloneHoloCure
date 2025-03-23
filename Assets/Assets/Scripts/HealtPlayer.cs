using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class HealtPlayer : MonoBehaviour
{
    public MovementPlayer player; // Ссылка на игрока

    public Image healthBar; // Полоска здоровья
    public Image shieldBar; // Полоска щита
    public TextMeshProUGUI healthText; // Текстовое поле для отображения здоровья

    public float maxHealth = 100f; // Максимальное здоровье
    private float lastHP; // Запоминаем предыдущее здоровье для отслеживания изменений
    private float lastShield; // Запоминаем предыдущий щит для отслеживания изменений

    private bool isHealing; // Флаг для отслеживания, идёт ли сейчас временное зеленение текста

    void Start()
    {
        maxHealth = player.maxHealth; // Устанавливаем максимальное здоровье из HP игрока
        lastHP = player.HP; // Запоминаем текущее здоровье
        lastShield = player.shield; // Запоминаем текущий щит
    }

    void Update()
    {
        // Обновляем полоску здоровья
        healthBar.fillAmount = player.HP / maxHealth;

        // Обновляем полоску щита
        if (shieldBar != null)
        {
            shieldBar.fillAmount = player.shield / player.maxShield;
        }

        // Обновляем текстовое отображение здоровья
        if (healthText != null)
        {
            healthText.text = $"{Mathf.Ceil(player.HP)} ! {Mathf.Ceil(maxHealth)}"; // Текущее здоровье / Максимальное здоровье

            // Если не идёт временное зеленение текста (исцеление), обновляем цвет в зависимости от здоровья
            if (!isHealing)
            {
                UpdateHealthTextColor();
            }
        }

        // Проверяем, было ли исцеление
        if (player.HP > lastHP)
        {
            StartCoroutine(HealEffect()); // Запускаем эффект зеленения текста
        }

        // Обновляем последнее здоровье и щит
        lastHP = player.HP;
        lastShield = player.shield;
    }

    // Метод для обновления цвета текста в зависимости от здоровья
    void UpdateHealthTextColor()
    {
        float healthPercentage = (player.HP / maxHealth) * 100;

        if (healthPercentage > 80f)
        {
            healthText.color = Color.white; // Белый цвет при здоровье больше 80%
        }
        else if (healthPercentage > 50f)
        {
            healthText.color = new Color(236f / 255f, 184f / 255f, 75f / 255f); // Желтый
        }
        else if (healthPercentage > 20f)
        {
            healthText.color = new Color(236f / 255f, 138f / 255f, 75f / 255f); // Оранжевый
        }
        else
        {
            healthText.color = new Color(236f / 255f, 86f / 255f, 75f / 255f); // Красный
        }
    }

    // Корутин для временного изменения цвета при исцелении
    private IEnumerator HealEffect()
    {
        isHealing = true;

        // Устанавливаем текст зелёным
        healthText.color = new Color(109f / 255f, 255f / 255f, 189f / 255f);

        yield return new WaitForSeconds(1f);

        isHealing = false;
        UpdateHealthTextColor();
    }
}