using UnityEngine;
using UnityEngine.UI; // Для работы с UI

public class MenuManager : MonoBehaviour
{
    public Text totalCoinsText; // Поле для текста (ссылка на UI-элемент)

    private const string TotalCoinsKey = "TotalCoins"; // Ключ для сохранения (тот же, что и в CoinManager)

    void Start()
    {
        // Загружаем общее количество монет из сохранения
        int totalCoins = PlayerPrefs.GetInt(TotalCoinsKey, 0);

        // Устанавливаем текст на UI
        if (totalCoinsText != null)
        {
            totalCoinsText.text = totalCoins.ToString();
        }
        else
        {
            Debug.LogWarning("totalCoinsText не назначен на объекте MenuManager!");
        }

        Debug.Log("Общее количество монет, отображаемых в меню: " + totalCoins);
    }
}