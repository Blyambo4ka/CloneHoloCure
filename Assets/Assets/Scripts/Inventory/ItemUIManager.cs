using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIManager : MonoBehaviour
{
    public static ItemUIManager Instance { get; private set; }

    public GameObject itemPanel;    // Панель выбора предметов
    public Transform itemParent;    // Родительский объект для предметов
    public List<GameObject> itemPrefabs; // Список префабов предметов
    public Transform[] itemPositions; // Позиции предметов

    private bool isItemSelected = false;
    private List<GameObject> spawnedItems = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        itemPanel.SetActive(false);
    }

    public void ShowItemPanel()
    {
        itemPanel.SetActive(true);
        Time.timeScale = 0;
        isItemSelected = false;
        ShowItems();
    }

    public void ShowItems()
    {
        List<GameObject> selectedItems = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            GameObject randomItem;
            do
            {
                randomItem = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
            } while (selectedItems.Contains(randomItem));

            selectedItems.Add(randomItem);
        }

        for (int i = 0; i < selectedItems.Count; i++)
        {
            GameObject itemObj = Instantiate(selectedItems[i], itemPositions[i].position, Quaternion.identity, itemPanel.transform);
            spawnedItems.Add(itemObj);
            ItemController itemController = itemObj.GetComponent<ItemController>();
            if (itemController != null)
            {
                itemController.SetupItem(null, "Item Title", "Item Description");
                itemController.AnimateAppear();
                itemController.button.onClick.AddListener(() => OnItemSelected(itemController));
            }
        }
    }

    public void OnItemSelected(ItemController selectedItem)
    {
        if (isItemSelected) return;
        isItemSelected = true;

        selectedItem.ApplyItemEffect();
        selectedItem.SelectItem();

        HideOtherItems(selectedItem);

        StartCoroutine(ClosePanelAfterDelay(1.2f));
    }

    private void HideOtherItems(ItemController selectedItem)
    {
        foreach (GameObject itemObj in spawnedItems)
        {
            ItemController item = itemObj.GetComponent<ItemController>();
            if (item != null && item != selectedItem)
            {
                item.AnimateHide();
            }
        }
    }

    private IEnumerator ClosePanelAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        itemPanel.SetActive(false);
        Time.timeScale = 1;
        foreach (GameObject item in spawnedItems)
        {
            Destroy(item);
        }
        spawnedItems.Clear();
    }
}