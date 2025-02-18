using UnityEngine;

public class Enemy2 : Enemy
{
    public override void Initialize(Transform player, Spawner spawner, int hp, int experienceAmount, int coinAmount, int enemyIndex)
    {
        // Делаем Enemy2 сильнее
        base.Initialize(player, spawner, hp * 2, experienceAmount + 10, coinAmount + 1, enemyIndex);
        damage += 5; // Увеличиваем урон на 5
        hp += 20;
    }
}
