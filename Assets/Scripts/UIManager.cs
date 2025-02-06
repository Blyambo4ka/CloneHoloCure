using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } // Singleton

    public GameObject levelUpPanel;    // Панель прокачки
    public Transform cardParent;       // Родитель для карт
    public List<GameObject> cardPrefabs; // Список префабов карт
    public Transform[] cardPositions; // Позиции карт
    
    private bool isCardSelected = false;  // Флаг выбора карты
    private MovementPlayer player;
    private List<GameObject> spawnedCards = new List<GameObject>();

    public ParticleSystem levelUpEffect;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        levelUpPanel.SetActive(false);
        player = Object.FindFirstObjectByType<MovementPlayer>(); // Находим игрока
    }

    public void ShowLevelUpPanel()
    {
        levelUpPanel.SetActive(true);
        Time.timeScale = 0;
        isCardSelected = false;
        ShowLevelUpCards();

        if (levelUpEffect != null)
        {
            levelUpEffect.Play(); 
        }
    }

    public void ShowLevelUpCards()
    {
        List<GameObject> selectedCards = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            GameObject randomCard;
            do
            {
                randomCard = cardPrefabs[Random.Range(0, cardPrefabs.Count)];
            } while (selectedCards.Contains(randomCard));
            
            selectedCards.Add(randomCard);
        }

        for (int i = 0; i < selectedCards.Count; i++)
        {
            GameObject cardObj = Instantiate(selectedCards[i], cardPositions[i].position, Quaternion.identity, levelUpPanel.transform);
            spawnedCards.Add(cardObj);
            CardController cardController = cardObj.GetComponent<CardController>();
            if (cardController != null)
            {
                cardController.SetupCard(null, "Title", "Description");
                cardController.AnimateAppear();
                cardController.button.onClick.AddListener(() => OnCardSelected(cardController));
            }
        }
    }

    public void OnCardSelected(CardController selectedCard)
{
    if (isCardSelected) return;
    isCardSelected = true;

    selectedCard.ApplyCardEffect();
    selectedCard.SelectCard(); // Анимация выбора

    HideOtherCards(selectedCard); // Запускаем анимацию скрытия других карт

    StartCoroutine(ClosePanelAfterDelay(1.2f)); // Даем анимациям время
}

private void HideOtherCards(CardController selectedCard)
{
    foreach (GameObject cardObj in spawnedCards)
    {
        CardController card = cardObj.GetComponent<CardController>();
        if (card != null && card != selectedCard)
        {
            card.AnimateHide();
        }
    }
}

private IEnumerator ClosePanelAfterDelay(float delay)
{
    yield return new WaitForSecondsRealtime(delay); // Ждем завершения анимации
    levelUpPanel.SetActive(false);
    Time.timeScale = 1;
    foreach (GameObject card in spawnedCards)
    {
        Destroy(card);
    }
    spawnedCards.Clear();
    if (player != null)
    {
        player.ResumeGame();
    }
    else
    {
        Debug.LogWarning("UIManager: Player reference is missing!");
    }
}

}
