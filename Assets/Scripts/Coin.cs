  using UnityEngine;

  public class Coin : MonoBehaviour
    {
     public float coinValue = 1f;
     public void OnTriggerEnter2D(Collider2D collider)
        {
          if (collider.gameObject.tag == "Player")
            {
               MovementPlayer playerMovement = collider.gameObject.GetComponent<MovementPlayer>();
               if (playerMovement != null)
               {
                    playerMovement.AddMoney(coinValue);
                    Destroy(gameObject);
                 }
             }
          }
      }