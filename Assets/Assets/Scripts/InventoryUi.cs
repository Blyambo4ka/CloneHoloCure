using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityUI : MonoBehaviour
{
    public Image abilityIcon;
    public TextMeshProUGUI levelText;
    
    public void UpdateUI(Sprite icon, int level)
    {
        abilityIcon.sprite = icon;
        levelText.text = $"Уровень: {level}";
    }
}
