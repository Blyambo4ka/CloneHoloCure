using UnityEngine;

public class PlayerSorter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpriteRenderer playerRenderer = other.GetComponent<SpriteRenderer>();
            if (playerRenderer != null)
            {
                // Если игрок выше дерева - он за деревом
                if (other.transform.position.y > transform.position.y)
                {
                    spriteRenderer.sortingOrder = playerRenderer.sortingOrder - 1;
                }
                else // Если ниже - поверх дерева
                {
                    spriteRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
                }
            }
        }
    }
}
