using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Chest : MonoBehaviour
{
    public GameObject[] items; // Список префабов предметов (10 штук)
    private bool isOpened = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpened && collision.CompareTag("Player"))
        {
            isOpened = true;
            DropItems();
        }
    }

    private void DropItems()
{
    // Получаем 10 уникальных предметов
    System.Random random = new System.Random();
    GameObject[] uniqueItems = items.OrderBy(x => random.Next()).Take(10).ToArray();

    foreach (var item in uniqueItems)
    {
        // Добавляем в инвентарь, если есть место
        bool added = InventoryManager.Instance.AddItem(item);

        if (!added)
        {
            Debug.Log("Не удалось добавить предмет в инвентарь!");
        }
    }

    Destroy(gameObject); // Удаляем сундук после выпадения предметов
}

}
