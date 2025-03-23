using UnityEngine;
using System.Collections;

public class AttackBoost : MonoBehaviour
{
    private int upgradeLevel = 0;
    private const int maxUpgradeLevel = 5; // Максимальный уровень усиления
    public int baseAttackBonus = 0;

    public bool isUnlocked = false;
    private int currentAttackBonus;
    public Sprite abilityIcon; // Иконка способности

    private float baseAttackRange = 1.5f; // Базовая зона атаки
    private bool doubleStrike = false; // Флаг для двойного удара

    void Start()
    {
        currentAttackBonus = baseAttackBonus;
    }

    public void UnlockAbility()
    {
        if (isUnlocked) return;

        isUnlocked = true;

        // Добавляем способность в инвентарь
        InventoryUIManager.Instance.AddAbilityToUI("AttackBoost", abilityIcon, upgradeLevel);

        ApplyBoost();
        Debug.Log("AttackBoost unlocked!");
    }

    public void UpgradeAbility()
    {
        if (!isUnlocked || upgradeLevel >= maxUpgradeLevel) return;

        upgradeLevel++;
        currentAttackBonus += 5;

        // Применяем улучшения в зависимости от уровня
        if (upgradeLevel == 3)
        {
            IncreaseAttackRange();
        }
        else if (upgradeLevel == maxUpgradeLevel)
        {
            EnableDoubleStrike();
        }

        // Обновляем уровень способности в инвентаре
        InventoryUIManager.Instance.UpdateAbilityLevelInUI("AttackBoost", upgradeLevel);

        ApplyBoost();

        if (upgradeLevel == maxUpgradeLevel)
        {
            Debug.Log("AttackBoost upgraded to MAX level! Maximum damage boost applied.");
        }

        Debug.Log($"AttackBoost upgraded! Level: {upgradeLevel}, Attack Bonus: {currentAttackBonus}");
    }

     private void ApplyBoost()
    {
        MovementPlayer player = Object.FindFirstObjectByType<MovementPlayer>();
        if (player != null)
        {
            player.attackDamage += currentAttackBonus;

            // Применяем увеличение зоны атаки, если уровень >= 3
            if (upgradeLevel >= 3)
            {
                player.attackRange = baseAttackRange * 1.5f; // Увеличиваем зону атаки на 50%
                player.attackPrefabScale = 1.1f; // Увеличиваем размер префаба атаки на 10%
            }

            // Применяем двойной удар и ускорение анимации, если уровень >= 5
            if (upgradeLevel >= maxUpgradeLevel)
            {
                player.doubleStrike = true;
                player.attackAnimationSpeed = 1.25f; // Увеличиваем скорость анимации на 25%
            }

            Debug.Log($"Player attack boosted! Damage: {player.attackDamage}, Attack Range: {player.attackRange}, Double Strike: {player.doubleStrike}");
        }
        else
        {
            Debug.LogError("MovementPlayer not found in the scene!");
        }
    }

    private void IncreaseAttackRange()
    {
        Debug.Log("Attack range increased by 50%!");
    }

    private void EnableDoubleStrike()
    {
        Debug.Log("Double strike enabled!");
    }
}