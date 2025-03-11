using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; // Синглтон для удобного доступа

    private Dictionary<string, InventoryItem> inventory = new Dictionary<string, InventoryItem>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Добавление новой способности в инвентарь
   public void AddAbility(string abilityName, int level, Sprite abilityIcon)
    {
        if (inventory.ContainsKey(abilityName))
        {
            Debug.LogWarning($"Ability {abilityName} уже есть в инвентаре!");
            return;
        }

        InventoryItem newItem = new InventoryItem(abilityName, level);
        inventory.Add(abilityName, newItem);

        // Отображаем способность в UI
        InventoryUIManager.Instance.AddAbilityToUI(abilityName, abilityIcon, level);

        Debug.Log($"Добавлена способность {abilityName} с уровнем {level} в инвентарь!");
    }

    public void UpdateAbilityLevel(string abilityName, int newLevel)
    {
        if (!inventory.ContainsKey(abilityName))
        {
            Debug.LogError($"Способность {abilityName} не найдена в инвентаре!");
            return;
        }

        inventory[abilityName].Level = newLevel;

        // Обновляем уровень способности в UI
        InventoryUIManager.Instance.UpdateAbilityLevelInUI(abilityName, newLevel);

        Debug.Log($"Обновлён уровень способности {abilityName}: теперь уровень {newLevel}");
    }
    
    // Показать содержимое инвентаря (для отладки)
    public void ShowInventory()
    {
        Debug.Log("=== Инвентарь ===");
        foreach (var item in inventory)
        {
            Debug.Log($"Способность: {item.Value.Name}, Уровень: {item.Value.Level}");
        }
    }
}

// Класс для представления предмета в инвентаре
[System.Serializable]
public class InventoryItem
{
    public string Name; // Название способности
    public int Level;   // Уровень способности

    public InventoryItem(string name, int level)
    {
        Name = name;
        Level = level;
    }

    
}