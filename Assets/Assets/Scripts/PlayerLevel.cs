using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public int level = 1;
    public int experience = 0;
    public int expToNextLevel = 100;
    public UIManager uiManager;

    public void GainExperience(int amount)
    {
        experience += amount;
        if (experience >= expToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        experience = 0;
        expToNextLevel += 50;
        uiManager.ShowLevelUpPanel();
    }
}
