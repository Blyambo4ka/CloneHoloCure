using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    public Image abilityIcon;
    public Text abilityLevelText;
    private int abilityLevel = 1;

    public void SetupAbility(Sprite icon)
    {
        abilityIcon.sprite = icon;
        UpdateLevel(1);
    }

    public void UpdateLevel(int level)
    {
        abilityLevel = level;
        abilityLevelText.text = "Lv. " + abilityLevel;
    }
}
