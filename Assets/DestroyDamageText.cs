using UnityEngine;

public class DestroyDamageText : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator не найден на объекте текста урона!");
        }
    }

    void Update()
    {
        if (animator != null)
        {
            // Проверяем, завершилась ли анимация
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && 
                !animator.IsInTransition(0))
            {
                Destroy(gameObject); // Удаляем текст, если анимация завершилась
            }
        }
    }
}

