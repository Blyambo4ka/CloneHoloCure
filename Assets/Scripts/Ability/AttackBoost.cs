using UnityEngine;
using System.Collections;

public class AttackBoost : MonoBehaviour
{
    private int upgradeLevel = 0;
    private const int maxUpgradeLevel = 5; // Максимальный уровень усиления
    public int baseAttackBonus = 5;
    

    public bool isUnlocked = false;
    private int currentAttackBonus;
    public Sprite abilityIcon; // Иконка способности
    

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
            
            Debug.Log($"Player attack boosted! Damage: {player.attackDamage}");
        }
        else
        {
            Debug.LogError("MovementPlayer not found in the scene!");
        }
    }
}
