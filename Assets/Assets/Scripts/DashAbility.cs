using UnityEngine;
using System.Collections;

public class DashAbility : MonoBehaviour
{
    public GameObject fireTrailPrefab; // Обычный след огня (для уровней 1-4)
    public GameObject fireTrailMaxLevelPrefab; // Фиолетовый/альтернативный след огня для 5-го уровня
    public Transform feetstep; // Точка между ног для появления следов
    public float dashCooldown = 5f;

    private float dashDuration = 0.3f;
    private float fireDuration = 2f;
    private int fireDamage = 10;
    private bool canDash = true;

    private int upgradeLevel = 0;
    private const int maxUpgradeLevel = 5;

    public bool isUnlocked = false;
    public float dashSpeed = 10f;

    void Update()
    {
        if (!isUnlocked) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public void UnlockAbility()
    {
        if (isUnlocked) return;

        isUnlocked = true;
        Debug.Log("Dash unlocked!");
    }

    public void UpgradeAbility()
    {
        if (!isUnlocked || upgradeLevel >= maxUpgradeLevel) return;

        upgradeLevel++;
        fireDamage += 1;
        dashCooldown = Mathf.Max(0.5f, dashCooldown - 0.5f);
        dashSpeed += 2f;

        // Лог для максимального уровня:
        if (upgradeLevel == maxUpgradeLevel)
        {
            Debug.Log("Dash upgraded to MAX level! Using alternative fire trail!");
        }

        Debug.Log($"Dash upgraded! Level: {upgradeLevel}, Damage: {fireDamage}, Cooldown: {dashCooldown}, Speed: {dashSpeed}");
    }

    IEnumerator Dash()
    {
        canDash = false;

        Vector2 dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (dashDirection == Vector2.zero)
        {
            dashDirection = Vector2.right; // Если игрок не движется, даш вправо
        }

        float dashStartTime = Time.time;
        StartCoroutine(SpawnFireTrail()); // Запускаем создание следов

        while (Time.time < dashStartTime + dashDuration)
        {
            transform.position += (Vector3)dashDirection * dashSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator SpawnFireTrail()
    {
        for (int i = 0; i < 5; i++) // Создание 5 следов
        {
            if (feetstep != null)
            {
                Vector3 spawnPosition = feetstep.position;

                // Проверяем уровень способности. Если 5-й, используем fireTrailMaxLevelPrefab.
                GameObject fireTrail = Instantiate(
                    upgradeLevel >= maxUpgradeLevel ? fireTrailMaxLevelPrefab : fireTrailPrefab,
                    spawnPosition,
                    Quaternion.identity
                );

                // Инициализируем огненный след с текущими параметрами
                fireTrail.GetComponent<FireTrail>().Initialize(fireDamage, fireDuration);
            }
            else
            {
                Debug.LogWarning("Feetstep не назначен! Укажите Transform для появления следов.");
            }

            yield return new WaitForSeconds(0.3f); // Интервал между появлением следов
        }
    }
}