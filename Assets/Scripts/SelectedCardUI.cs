using UnityEngine;
using UnityEngine.UI;

public class SelectedCardUI : MonoBehaviour
{
    public Image cardIcon;
    public Text levelText;
    private SelectedCard currentCard;

    public void UpdateUI(Sprite icon, int level)
    {
        if (icon != null)
        {
            cardIcon.sprite = icon;
            cardIcon.enabled = true;
            levelText.text = "Level " + level;
            levelText.enabled = true;
        }
        else
        {
            cardIcon.enabled = false;
            levelText.enabled = false;
        }
    }

    public void SelectCard(SelectedCard card)
    {
        currentCard = card;
        UpdateUI(card.icon, card.level);
    }
    
    void Start() 
    {
        ClearUI(); // Чистим UI на старте
    }

    public void ClearUI()
    {
        cardIcon.enabled = false;
        levelText.enabled = false;
    }
}

public class SelectedCard
{
    public Sprite icon;
    public int level;
    
    public SelectedCard(Sprite icon, int level)
    {
        this.icon = icon;
        this.level = level;
    }
}

