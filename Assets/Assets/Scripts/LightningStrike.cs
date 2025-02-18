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
        Debug.Log("LightningStrike unlocked!");
    }

    public void UpgradeAbility()
    {
        if (!isUnlocked || upgradeLevel >= maxUpgradeLevel) return;

        upgradeLevel++;
        currentDamage += 5;
        currentCooldown = Mathf.Max(0.5f, currentCooldown - 0.5f);

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