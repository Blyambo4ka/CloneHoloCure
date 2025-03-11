using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    public GameObject inventoryPanel; // Панель, содержащая слоты
    public GameObject abilityPrefab;  // Префаб способности (иконка + текст)

    public Dictionary<string, GameObject> abilityUIElements = new Dictionary<string, GameObject>();

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


    public void AddAbilityToUI(string abilityName, Sprite abilityIcon, int level)
    {
        if (abilityUIElements.ContainsKey(abilityName))
        {
            Debug.LogWarning($"Способность {abilityName} уже отображается в UI!");
            return;
        }

        // Ищем первый свободный слот
        Transform freeSlot = FindFreeSlot();
        if (freeSlot == null)
        {
            Debug.LogError("Нет свободных слотов для способности!");
            return;
        }

        // Создаём новый элемент UI для способности
        GameObject newAbilityUI = Instantiate(abilityPrefab, freeSlot);

        // Настраиваем изображение способности
        Image abilityImage = newAbilityUI.GetComponentInChildren<Image>(); // Ищем компонент Image
        if (abilityImage != null)
        {
            abilityImage.sprite = abilityIcon; // Устанавливаем уникальную картинку для способности

            // Устанавливаем размеры иконки в зависимости от её спрайта
            if (abilityIcon != null)
            {
                RectTransform rectTransform = abilityImage.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(abilityIcon.rect.width, abilityIcon.rect.height);
            }
        }
        else
        {
            Debug.LogError("Компонент Image не найден в префабе способности или его дочерних объектах!");
        }

        // Настраиваем текст уровня (с использованием TextMeshProUGUI)
        TextMeshProUGUI abilityText = newAbilityUI.GetComponentInChildren<TextMeshProUGUI>(); // Ищем TextMeshProUGUI
        if (abilityText != null)
        {
            abilityText.text = $"lv. {level}"; // Устанавливаем текст с уровнем
        }
        else
        {
            Debug.LogError("Компонент TextMeshProUGUI не найден в префабе способности!");
        }

        // Сохраняем элемент в словаре
        abilityUIElements.Add(abilityName, newAbilityUI);
    }




   // Обновить уровень способности в UI
    public void UpdateAbilityLevelInUI(string abilityName, int newLevel)
    {
        if (!abilityUIElements.ContainsKey(abilityName))
        {
            Debug.LogError($"Способность {abilityName} не найдена в UI!");
            return;
        }

        // Получаем UI элемент способности
        GameObject abilityUI = abilityUIElements[abilityName];

        // Ищем компонент TextMeshProUGUI для обновления текста
        TextMeshProUGUI abilityText = abilityUI.GetComponentInChildren<TextMeshProUGUI>();
        if (abilityText != null)
        {
            abilityText.text = $"lv. {newLevel}"; // Обновляем текст уровня
        }
        else
        {
            Debug.LogError("Компонент TextMeshProUGUI не найден в UI элементе способности!");
        }
    }



    // Найти первый свободный слот в инвентаре
    private Transform FindFreeSlot()
    {
        foreach (Transform slot in inventoryPanel.transform)
        {
            if (slot.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }
}