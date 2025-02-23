using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Позиции для предметов")]
    public Transform[] itemSlots; // Пустышки для позиций предметов

    [Header("Префаб иконки")]
    public GameObject itemIconPrefab; // Префаб иконки (Image)

    private List<GameObject> inventoryItems = new List<GameObject>();

    private void Awake()
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

    public bool AddItem(GameObject itemPrefab)
    {
        // Ищем первую свободную позицию
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i >= inventoryItems.Count || inventoryItems[i] == null)
            {
                // Создаем предмет на свободной позиции
                GameObject newItem = Instantiate(itemPrefab, itemSlots[i].position, Quaternion.identity);
                newItem.transform.SetParent(itemSlots[i]);
                inventoryItems.Insert(i, newItem);

                // Добавляем иконку к предмету
                Sprite itemIcon = itemPrefab.GetComponent<Item>().itemIcon;
                if (itemIcon != null)
                {
                    GameObject icon = Instantiate(itemIconPrefab, itemSlots[i].position, Quaternion.identity);
                    icon.GetComponent<Image>().sprite = itemIcon;
                    icon.transform.SetParent(itemSlots[i]);
                }

                return true;
            }
        }

        Debug.Log("Инвентарь полон!");
        return false;
    }
}
