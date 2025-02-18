using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab; // Префаб карты
    public Transform cardParent; // Родительский объект для карт
    public List<CardData> allCards; // Список всех карт

    private List<CardController> activeCards = new List<CardController>();

    public void ShowLevelUpCards()
    {
        ClearPreviousCards();
        List<CardData> selectedCards = GetRandomCards(3);
        
        foreach (var cardData in selectedCards)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardParent);
            CardController card = cardObj.GetComponent<CardController>();
            card.SetupCard(cardData.icon, cardData.title, cardData.description);
            card.AnimateAppear();
            activeCards.Add(card);
        }
    }

    private List<CardData> GetRandomCards(int count)
    {
        List<CardData> availableCards = new List<CardData>(allCards);
        List<CardData> selectedCards = new List<CardData>();
        
        for (int i = 0; i < count && availableCards.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableCards.Count);
            selectedCards.Add(availableCards[randomIndex]);
            availableCards.RemoveAt(randomIndex);
        }
        
        return selectedCards;
    }

    private void ClearPreviousCards()
    {
        foreach (var card in activeCards)
        {
            Destroy(card.gameObject);
        }
        activeCards.Clear();
    }
}

