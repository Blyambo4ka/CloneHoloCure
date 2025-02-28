using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int coinsInGame = 0; // Счётчик монет для текущей игровой сессии
    public TMPro.TextMeshProUGUI coinsText; // Для TMP

    // Ключи для сохранения
    private const string TotalCoinsKey = "TotalCoins";

    void Start()
    {
        // В начале каждой игровой сессии монеты всегда 0
        coinsInGame = 0;
        Debug.Log("Новая сессия. Монет в игре: " + coinsInGame);
    }

    public void AddCoins(int amount)
    {
        coinsInGame += amount;
        Debug.Log("Монет добавлено: " + amount + ". Текущий счёт: " + coinsInGame);
        UpdateUI(); // Обновляем текст
    }

    private void UpdateUI()
    {
        if (coinsText != null)
        {
            coinsText.text = "" + coinsInGame;
        }
    }


    public void EndGameAndSaveCoins()
    {
        // Берём общее количество монет из сохранения
        int totalCoins = PlayerPrefs.GetInt(TotalCoinsKey, 0);
        
        // Прибавляем монеты из текущей игровой сессии
        totalCoins += coinsInGame;

        // Сохраняем новое значение
        PlayerPrefs.SetInt(TotalCoinsKey, totalCoins);
        PlayerPrefs.Save();

        Debug.Log($"Игра завершена. Монет в игре: {coinsInGame}. Всего монет теперь: {totalCoins}");

        // Обнуляем монеты для следующей игры
        coinsInGame = 0;
    }

    public void ResetAllCoins()
    {
        // Обнуляем монеты в текущей сессии
        coinsInGame = 0;
        UpdateUI();

        // Удаляем сохраненные данные
        PlayerPrefs.DeleteKey(TotalCoinsKey);
        PlayerPrefs.Save();

        Debug.Log("Все монеты сброшены.");
    }
}