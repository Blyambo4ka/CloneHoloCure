using UnityEngine;
using System.Collections;

public class LightningStrike : MonoBehaviour
{
    public GameObject lightningEffect; // Обычная молния (синяя)
    public GameObject lightningEffectMaxLevel; // Молния для 5 уровня (фиолетовая)
    public int baseDamage = 10; 
    public float baseCooldown = 4f;
    
    private int upgradeLevel = 0;
    private const int maxUpgradeLevel = 5; // Максимальный уровень способности
    
    public bool isUnlocked = false;  // Способность активна?
    private float currentCooldown;
    private int currentDamage;

    public Sprite abilityIcon; // Добавляем поле для иконки способности

    void Start()
    {
        currentCooldown = baseCooldown;
        currentDamage = baseDamage;

        if (!isUnlocked)
        {
            StopAllCoroutines(); 
        }
    }

    public void UnlockAbility() 
    {
        if (isUnlocked) return;

        isUnlocked = true;
        StartCoroutine(StrikeRandomEnemy());

        // Добавляем способность в инвентарь и UI
        InventoryManager.Instance.AddAbility("Lightning Strike", upgradeLevel, abilityIcon);

        Debug.Log("LightningStrike unlocked!");
    }

    public void UpgradeAbility()
    {
        if (!isUnlocked || upgradeLevel >= maxUpgradeLevel) return;

        upgradeLevel++;
        currentDamage += 5;
        currentCooldown = Mathf.Max(0.5f, currentCooldown - 0.5f);

        // Обновляем уровень способности в инвентаре и UI
        InventoryUIManager.Instance.UpdateAbilityLevelInUI("Lightning Strike", upgradeLevel);

        if (upgradeLevel == maxUpgradeLevel)
        {
            Debug.Log("LightningStrike upgraded to MAX level! Now using purple lightning!");
        }

        Debug.Log($"LightningStrike upgraded! Level: {upgradeLevel}, Damage: {currentDamage}, Cooldown: {currentCooldown}");
    }

    IEnumerator StrikeRandomEnemy()
    {
        while (isUnlocked)
        {
            yield return new WaitForSeconds(currentCooldown);

            Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);

            if (enemies.Length > 0)
            {
                int numberOfStrikes = 1; // По умолчанию 1 молния

                if (upgradeLevel >= 3)
                {
                    numberOfStrikes = 2; // На 3 уровне 2 молнии
                }

                if (upgradeLevel >= maxUpgradeLevel)
                {
                    numberOfStrikes = 3; // На 5 уровне 3 молнии
                }

                for (int i = 0; i < numberOfStrikes; i++)
                {
                    Enemy target = enemies[Random.Range(0, enemies.Length)];

                    // Выбираем молнию в зависимости от уровня
                    GameObject lightningPrefab = (upgradeLevel >= maxUpgradeLevel) ? lightningEffectMaxLevel : lightningEffect;

                    // Спавним молнию
                    GameObject lightning = Instantiate(lightningPrefab, target.transform.position, Quaternion.identity);

                    // Удаляем молнию после анимации
                    float animationLength = lightning.GetComponent<Animator>()?.GetCurrentAnimatorStateInfo(0).length ?? 1f;
                    Destroy(lightning, animationLength);

                    // Наносим урон
                    target.TakeDamage(currentDamage);
                    Debug.Log($"LightningStrike hit enemy with {currentDamage} damage!");
                }
            }
        }
    }

    public void IncreaseDamage(float multiplier)
    {
        baseDamage = Mathf.RoundToInt(baseDamage * multiplier); // Увеличиваем базовый урон
        currentDamage = Mathf.RoundToInt(currentDamage * multiplier); // Увеличиваем текущий урон
        Debug.Log($"Урон LightningStrike увеличен! Текущий урон: {currentDamage}");
    }
}