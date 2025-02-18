using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour
{
    // Лист для трансформов слотов
    public List<Transform> slotTransforms;

    // Класс для хранения данных о способности
    [System.Serializable]
    public class Ability
    {
        public string name;
        public Sprite icon;
        public int level;
    }

    // Лист со способностями
    public List<Ability> abilities = new List<Ability>();

    // Список спрайтов для теста (привязать в инспекторе)
    public List<Sprite> abilityIcons;

    void Start()
    {
        // Добавляем тестовые способности
        AddAbility("Lightning Bolt", abilityIcons[0], 1);
        AddAbility("Dash", abilityIcons[1], 2);
        AddAbility("Attack Boost", abilityIcons[2], 3);
    }

    // Метод для добавления способности
    public void AddAbility(string abilityName, Sprite icon, int level)
    {
        // Проверяем, есть ли свободные слоты
        if (abilities.Count >= slotTransforms.Count)
        {
            Debug.LogWarning("Нет свободных слотов в инвентаре!");
            return;
        }

        // Создаем новую способность
        Ability newAbility = new Ability
        {
            name = abilityName,
            icon = icon,
            level = level
        };

        // Добавляем её в лист
        abilities.Add(newAbility);

        // Обновляем UI
        UpdateUI();
    }

    // Метод для обновления UI
    private void UpdateUI()
    {
        // Обновляем UI для всех способностей
        for (int i = 0; i < abilities.Count; i++)
        {
            Transform slot = slotTransforms[i];
            Image image = slot.GetComponentInChildren<Image>();
            TextMeshProUGUI levelText = slot.GetComponentInChildren<TextMeshProUGUI>();

            // Обновляем иконку
            image.sprite = abilities[i].icon;
            image.enabled = true;  // Включаем иконку

            // Обновляем уровень
            levelText.text = "Lvl " + abilities[i].level;
            levelText.enabled = true;
        }
    }
}
