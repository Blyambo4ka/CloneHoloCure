using UnityEngine;

[System.Serializable]
public class InventoryObject
{
    public string Name; // Название предмета
    public Sprite Icon; // Иконка предмета
    public string Description; // Описание предмета

    public InventoryObject(string name, Sprite icon, string description)
    {
        Name = name;
        Icon = icon;
        Description = description;
    }
}