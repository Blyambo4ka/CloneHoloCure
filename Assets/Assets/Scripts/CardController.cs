using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardController : MonoBehaviour
{
    public Image icon;
    public Text title;
    public Text description;
    public Button button;
    public LightningStrike linkedAbility;
    public DashAbility linkedDashAbility;
    public AttackBoost linkedAttackBoost;

    private Animator animator;
    private bool isSelected = false;





    void Awake()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(SelectCard);
        }
        if (icon == null)
         icon = GetComponentInChildren<Image>();

    }

    public void ApplyCardEffect()
    {
        if (linkedDashAbility != null)
        {
            if (!linkedDashAbility.isUnlocked)
                linkedDashAbility.UnlockAbility();
            else
                linkedDashAbility.UpgradeAbility();
        }

        if (linkedAbility != null)
        {
            if (!linkedAbility.isUnlocked)
                linkedAbility.UnlockAbility();
            else
                linkedAbility.UpgradeAbility();
        }

        if (linkedAttackBoost != null)
        {
            if (!linkedAttackBoost.isUnlocked)
                linkedAttackBoost.UnlockAbility();
            else
                linkedAttackBoost.UpgradeAbility();
        }
    }

    public void SetupCard(Sprite iconSprite, string cardTitle, string cardDescription)
    {
        if (icon != null) icon.sprite = iconSprite;
        if (title != null) title.text = cardTitle;
        if (description != null) description.text = cardDescription;
    }

    public void AnimateAppear()
    {
        if (animator != null)
        {
            animator.SetTrigger("TriggerAppear");
        }
    }

    public void SelectCard()
    {
        if (isSelected) return;
        isSelected = true;

        if (animator != null)
        {
            animator.SetTrigger("TriggerSelect");
        }

        UIManager.Instance.OnCardSelected(this);
    }

    public void AnimateHide()
    {
        if (animator != null)
        {
            animator.SetTrigger("TriggerHide");
            StartCoroutine(WaitForAnimationToEnd());
        }
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        // Ждём завершения анимации "TriggerHide"
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject); // Уничтожаем карту
    }
}
