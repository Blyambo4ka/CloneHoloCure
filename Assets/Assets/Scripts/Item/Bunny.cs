using UnityEngine;

public class Bunny : MonoBehaviour
{
    public DropOnKill dropOnKill; // Ссылка на предмет, который добавляет шанс выпадения

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) // Проверяем, что предмет подобрал игрок
        {
            // Активируем предмет
            if (dropOnKill != null)
            {
                DropOnKill.isActive = true;
            }

            // Уничтожаем объект предмета после подбора
            Destroy(gameObject);
            Debug.Log("Предмет подобран и активирован!");
        }
    }
}