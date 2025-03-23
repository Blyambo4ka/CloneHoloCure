using UnityEngine;

public class Enemy10 : Enemy
{
   
    public override void Initialize(Transform player, Spawner spawner, int hp, int experienceAmount, int coinAmount, int enemyIndex)
    {
        // Делаем Enemу10 сильнее
        base.Initialize(player, spawner, hp * 15, experienceAmount + 20, coinAmount + 3, enemyIndex);
        damage += 10; // Увеличиваем урон на 10
    }


}
