using UnityEngine;

public class SlowEffect : MonoBehaviour
{
    public float slowMultiplier = 0.5f; // Насколько замедлить скорость (50%)
    public Color slowColor = new Color(1f, 1f, 0f, 0.5f); // Прозрачно-жёлтый цвет
    private Color originalColor;
    private float originalSpeed;

    private SpriteRenderer spriteRenderer;
    private Enemy enemy;

    private bool isSlowed = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemy = GetComponent<Enemy>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        if (enemy != null)
        {
            originalSpeed = enemy.speed;
        }
    }

    public void ApplySlowEffect(float duration)
    {
        if (isSlowed) return; // Если эффект уже активен, ничего не делаем

        isSlowed = true;

        // Замедляем врага и перекрашиваем его
        if (enemy != null)
        {
            enemy.speed *= slowMultiplier;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = slowColor;
        }

        // Убираем эффект через заданное время
        Invoke(nameof(RemoveSlowEffect), duration);
    }

    private void RemoveSlowEffect()
    {
        isSlowed = false;

        // Возвращаем оригинальную скорость и цвет
        if (enemy != null)
        {
            enemy.speed = originalSpeed;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}