using UnityEngine;

[System.Serializable]
public class InventoryObject
{
    public string Name { get; set; } // Название предмета
    public Sprite Icon { get; set; } // Иконка предмета
    public string Description { get; set; } // Описание предмета

    public InventoryObject(string name, Sprite icon, string description)
    {
        Name = name;
        Icon = icon;
        Description = description;
    }
}
