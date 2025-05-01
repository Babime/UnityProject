using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory UI References")]
    public Image currentItemImage;
    public Image previousItemImage;
    public Image nextItemImage;

    [Header("Inventory Settings")]
    public List<ItemData> items = new List<ItemData>();
    private int currentIndex = 0;

    [Header("Player Stats References")]
    public PlayerStats playerStats; 

    private ItemHandler itemHandler;

    public bool canNavigate = true; 

    void Awake()
    {
        itemHandler = GetComponent<ItemHandler>();
        UpdateInventoryUI();
    }

    void Update()
    {
        if (!playerStats.isAlive) return;
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            NavigateRight();
        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            NavigateLeft();
        }
    }

    public void AddItem(ItemData newItem)
    {
        if (!playerStats.isAlive) return;
        items.Add(newItem);

        if (items.Count == 1)
        {
            currentIndex = 0;
            EquipCurrentItem();
        }

        UpdateInventoryUI();
    }

    private void NavigateRight()
    {
        if (items.Count == 0 || !canNavigate) return;

        currentIndex = (currentIndex + 1) % items.Count;
        EquipCurrentItem();
        UpdateInventoryUI();
    }

    private void NavigateLeft()
    {
        if (items.Count == 0 || !canNavigate) return;

        currentIndex = (currentIndex - 1 + items.Count) % items.Count;
        EquipCurrentItem();
        UpdateInventoryUI();
    }

    private void EquipCurrentItem()
    {
        if (!playerStats.isAlive) return;
        if (itemHandler != null && items.Count > 0)
        {
            itemHandler.EquipItem(items[currentIndex]);
        }
    }

    private void UpdateInventoryUI()
    {
        if (items.Count == 0)
        {
            currentItemImage.sprite = null;
            previousItemImage.sprite = null;
            nextItemImage.sprite = null;
            return;
        }

        currentItemImage.sprite = items[currentIndex].itemIcon;

        int previousIndex = (currentIndex - 1 + items.Count) % items.Count;
        int nextIndex = (currentIndex + 1) % items.Count;

        previousItemImage.sprite = items[previousIndex].itemIcon;
        nextItemImage.sprite = items[nextIndex].itemIcon;
    }

    public ItemData CurrentItem
    {
        get
        {
            if (items.Count > 0)
            {
                return items[currentIndex];
            }
            return null;
        }
    }
}
