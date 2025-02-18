using UnityEngine;

public class Enemy5 : Enemy
{
   
    public override void Initialize(Transform player, Spawner spawner, int hp, int experienceAmount, int coinAmount, int enemyIndex)
    {
        // Делаем Enemу5 сильнее
        base.Initialize(player, spawner, hp * 6, experienceAmount + 20, coinAmount + 3, enemyIndex);
        damage += 10; // Увеличиваем урон на 10
    }


}
