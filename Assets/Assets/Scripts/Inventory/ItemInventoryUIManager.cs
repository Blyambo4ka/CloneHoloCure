using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInventoryUIManager : MonoBehaviour
{
    public static ItemInventoryUIManager Instance;

    public GameObject itemPanel; // Панель, содержащая слоты для предметов
    public GameObject itemPrefab; // Префаб предмета (иконка + текст)
    public int maxSlots = 4; // Максимальное количество слотов

    private Dictionary<string, GameObject> itemUIElements = new Dictionary<string, GameObject>();

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

    // Проверка, есть ли свободные слоты
    public bool HasFreeSlot()
    {
        return GetUsedSlotsCount() < maxSlots;
    }

    // Проверка, заполнен ли инвентарь
    public bool IsInventoryFull()
    {
        return GetUsedSlotsCount() >= maxSlots;
    }

    // Получаем количество занятых слотов
    private int GetUsedSlotsCount()
    {
        int count = 0;
        foreach (Transform slot in itemPanel.transform)
        {
            if (slot.childCount > 0) count++;
        }
        return count;
    }

    // Добавление предмета в UI
    public bool AddItemToUI(InventoryObject item)
    {
        if (IsInventoryFull())
        {
            Debug.LogWarning($"Инвентарь полон! Нельзя добавить {item.Name}");
            return false;
        }

        if (itemUIElements.ContainsKey(item.Name))
        {
            Debug.LogWarning($"Предмет {item.Name} уже отображается в UI!");
            return false;
        }

        // Ищем первый свободный слот
        Transform freeSlot = FindFreeSlot();
        if (freeSlot == null)
        {
            Debug.LogError("Нет свободных слотов для предмета!");
            return false;
        }

        // Создаём новый элемент UI для предмета
        GameObject newItemUI = Instantiate(itemPrefab, freeSlot);

        // Настраиваем изображение предмета
        Image itemImage = newItemUI.GetComponentInChildren<Image>();
        if (itemImage != null)
        {
            itemImage.sprite = item.Icon;
        }

        // Настраиваем текст (название предмета)
        TextMeshProUGUI itemText = newItemUI.GetComponentInChildren<TextMeshProUGUI>();
        if (itemText != null)
        {
            itemText.text = item.Name;
        }

        // Сохраняем элемент в словаре
        itemUIElements.Add(item.Name, newItemUI);
        return true;
    }

    // Удаление предмета из UI
    public void RemoveItemFromUI(string itemName)
    {
        if (!itemUIElements.ContainsKey(itemName))
        {
            Debug.LogError($"Предмет {itemName} не найден в UI!");
            return;
        }

        GameObject itemUI = itemUIElements[itemName];
        Destroy(itemUI);
        itemUIElements.Remove(itemName);
    }

    // Найти первый свободный слот в панели предметов
    private Transform FindFreeSlot()
    {
        foreach (Transform slot in itemPanel.transform)
        {
            if (slot.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }
}
