using UnityEngine;

public class DropOnKill : MonoBehaviour
{
    public GameObject dropPrefab; // Префаб, который будет выпадать
    public float dropChance = 0.1f; // Шанс выпадения (10%)
    public static bool isActive = false; // Флаг активности предмета

    // Метод для активации предмета
    public void Activate()
    {
        isActive = true;
        Debug.Log("Предмет активирован: шанс выпадения включен!");
    }

    // Метод, который вызывается при убийстве врага
    public void TryDrop(Vector3 position)
    {
        // Проверяем, активирован ли предмет
        if (!isActive)
        {
            Debug.Log("Предмет не активирован!");
            return;
        }

        // Проверяем шанс выпадения
        if (Random.value <= dropChance)
        {
            // Создаём префаб на месте смерти врага
            Instantiate(dropPrefab, position, Quaternion.identity);
            Debug.Log("Предмет выпал!");
        }
    }
}