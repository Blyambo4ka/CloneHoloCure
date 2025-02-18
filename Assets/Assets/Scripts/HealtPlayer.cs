using UnityEngine;
using UnityEngine.UI;

public class HealtPlayer : MonoBehaviour
{
    public MovementPlayer player;

    Image healthBar;
    public float maxHealth = 100f;
    public float HP;
    
    void Start()
    {
        healthBar = GetComponent<Image>();
        maxHealth = player.HP;
    }

   
    void Update()
    {
        healthBar.fillAmount = player.HP / maxHealth;
    }
}
