using UnityEngine;

public class ExperienceBoostItem : MonoBehaviour
{
    public float experienceMultiplier = 0.9f; // Срез опыта до след уровня на 10% 
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected)
        {
            MovementPlayer player = collision.GetComponent<MovementPlayer>();
            if (player != null)
            {
                ApplyExperienceBoost(player);
                isCollected = true;
                Destroy(gameObject); // Уничтожаем предмет после подбора
            }
        }
    }

    private void ApplyExperienceBoost(MovementPlayer player)
    {
        // Увеличиваем количество получаемого опыта на 10%
        player.experienceToNextLevel *= experienceMultiplier;
        Debug.Log("Experience boost applied! New experience multiplier: " + experienceMultiplier);
    }
}