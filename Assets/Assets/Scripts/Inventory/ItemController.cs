using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemController : MonoBehaviour
{
    public Image icon;
    public Text title;
    public Text description;
    public Button button;

    public Item1 linkedItem;

    private Animator animator;
    private bool isSelected = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(SelectItem);
        }
        if (icon == null)
            icon = GetComponentInChildren<Image>();
    }

    public void ApplyItemEffect()
    {
        if (linkedItem != null)
        {
            ItemInventoryManager.Instance.AddItem(linkedItem.itemName, linkedItem.itemPrefab.GetComponent<Image>().sprite, "Item Description");
        }
    }

    public void SetupItem(Sprite iconSprite, string itemTitle, string itemDescription)
    {
        if (icon != null) icon.sprite = iconSprite;
        if (title != null) title.text = itemTitle;
        if (description != null) description.text = itemDescription;
    }

    public void AnimateAppear()
    {
        if (animator != null)
        {
            animator.SetTrigger("TriggerAppear");
        }
    }

    public void SelectItem()
    {
        if (isSelected) return;
        isSelected = true;

        if (animator != null)
        {
            animator.SetTrigger("TriggerSelect");
        }

        ItemUIManager.Instance.OnItemSelected(this);
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
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}