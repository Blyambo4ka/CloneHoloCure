using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryManager : MonoBehaviour
{
    public static ItemInventoryManager Instance; // Синглтон для предметного инвентаря

    private Dictionary<string, InventoryObject> itemInventory = new Dictionary<string, InventoryObject>();

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

    // Добавление предмета в инвентарь
    public void AddItem(string itemName, Sprite itemIcon, string description)
    {
        if (itemInventory.ContainsKey(itemName))
        {
            Debug.LogWarning($"Предмет {itemName} уже есть в инвентаре!");
            return;
        }

        InventoryObject newItem = new InventoryObject(itemName, itemIcon, description);
        itemInventory.Add(itemName, newItem);

        // Отображаем предмет в UI
        ItemInventoryUIManager.Instance.AddItemToUI(newItem);

        Debug.Log($"Добавлен предмет {itemName}: {description}");
    }

    // Показать содержимое предметного инвентаря (для отладки)
    public void ShowItemInventory()
    {
        Debug.Log("=== Инвентарь предметов ===");
        foreach (var item in itemInventory)
        {
            Debug.Log($"Предмет: {item.Value.Name}, Описание: {item.Value.Description}");
        }
    }
}