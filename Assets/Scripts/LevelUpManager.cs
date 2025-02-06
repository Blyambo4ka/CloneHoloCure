using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    public GameObject levelUpPanel; // Панель прокачки
    public List<GameObject> cardPrefabs; // Список префабов карт
    public Transform[] cardPositions; // Массив позиций для карт

    private void Start()
    {
        levelUpPanel.SetActive(false);
    }

    public void ShowLevelUpCards()
    {
        levelUpPanel.SetActive(true);
        List<GameObject> selectedCards = new List<GameObject>();

        // Выбираем 3 случайные карты
        for (int i = 0; i < 3; i++)
        {
            GameObject randomCard;
            do
            {
                randomCard = cardPrefabs[Random.Range(0, cardPrefabs.Count)];
            } while (selectedCards.Contains(randomCard)); // Убедиться, что карта не дублируется
            
            selectedCards.Add(randomCard);
        }

        // Спавним карты на сцене
        for (int i = 0; i < selectedCards.Count; i++)
        {
            Instantiate(selectedCards[i], cardPositions[i].position, Quaternion.identity, levelUpPanel.transform);
        }
    }
}
