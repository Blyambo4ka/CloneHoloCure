using UnityEngine;

[System.Serializable]
public class Item1
{
    public string itemName; // Название предмета
    public GameObject itemPrefab; // Префаб предмета
    public float rarity; // Редкость предмета (используется для выпадения)
    public int value; // Ценность предмета (например, количество монет или здоровья)

    // Конструктор для удобного создания предметов
    public Item1(string name, GameObject prefab, float rarity, int value)
    {
        this.itemName = name;
        this.itemPrefab = prefab;
        this.rarity = rarity;
        this.value = value;
    }
}