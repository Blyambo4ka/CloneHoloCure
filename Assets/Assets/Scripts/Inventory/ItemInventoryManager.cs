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
    public bool AddItem(string itemName, Sprite itemIcon, string description)
    {
        if (itemInventory.ContainsKey(itemName))
        {
            Debug.LogWarning($"Предмет {itemName} уже есть в инвентаре!");
            return false;
        }

        if (ItemInventoryUIManager.Instance.IsInventoryFull())
        {
            Debug.LogWarning($"Инвентарь переполнен! {itemName} не может быть добавлен.");
            return false;
        }

        InventoryObject newItem = new InventoryObject(itemName, itemIcon, description);
        itemInventory.Add(itemName, newItem);

        // Отображаем предмет в UI
        if (ItemInventoryUIManager.Instance.AddItemToUI(newItem))
        {
            Debug.Log($"Добавлен предмет {itemName}: {description}");
            return true;
        }
        else
        {
            itemInventory.Remove(itemName); // Удаляем из списка, если не добавили в UI
            return false;
        }
    }

    // Удаление предмета из инвентаря
    public void RemoveItem(string itemName)
    {
        if (!itemInventory.ContainsKey(itemName))
        {
            Debug.LogWarning($"Предмет {itemName} не найден в инвентаре!");
            return;
        }

        // Удаляем предмет из инвентаря
        itemInventory.Remove(itemName);

        // Удаляем предмет из UI
        ItemInventoryUIManager.Instance.RemoveItemFromUI(itemName);

        Debug.Log($"Предмет {itemName} удалён из инвентаря.");
    }

    // Проверка наличия предмета в инвентаре
    public bool HasItem(string itemName)
    {
        return itemInventory.ContainsKey(itemName);
    }
}
