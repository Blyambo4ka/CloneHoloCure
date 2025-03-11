using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInventoryUIManager : MonoBehaviour
{
    public static ItemInventoryUIManager Instance;

    public GameObject itemPanel; // Панель, содержащая слоты для предметов
    public GameObject itemPrefab; // Префаб предмета (иконка + текст)

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

    // Добавление предмета в UI
    public void AddItemToUI(InventoryObject item)
    {
        if (itemUIElements.ContainsKey(item.Name))
        {
            Debug.LogWarning($"Предмет {item.Name} уже отображается в UI!");
            return;
        }

        // Ищем первый свободный слот
        Transform freeSlot = FindFreeSlot();
        if (freeSlot == null)
        {
            Debug.LogError("Нет свободных слотов для предмета!");
            return;
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

    // Удалить предмет из UI
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
}