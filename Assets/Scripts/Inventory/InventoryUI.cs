using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public InventorySlot[] slots;

    public InventorySlot[] mainInventorySlots;

    private int currentSelectedItem = 0;
    private int currentActiveItem = 0;

    private Color selectionColor = new Color(0.8584906f, 0.6508957f, 0.5790762f);
    private Color activeSelectColor = new Color(1f, 0.3071686f, 0.01568627f);


    void Awake()
    {
        slots = gameObject.transform.Find("BackpackPanel").GetComponentsInChildren<InventorySlot>();
        mainInventorySlots = gameObject.transform.Find("MainInventoryPanel").GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in mainInventorySlots)
        {
            slot.ShowSlot();
        }
        UpdateUI();
        UpdateSelectItem();
        UpdateActiveItem();
    }

    public void UpdateUI()
    {
        int lastSlotsColumn = (inventory.height - 1) * inventory.width;
        for (int i = 0; i < slots.Length; i++)
        {
            if (inventory.items[i].item)
            {
                slots[i].AddItem(inventory.items[i]);
                if (i >= lastSlotsColumn)
                {
                    mainInventorySlots[i - lastSlotsColumn].AddItem(inventory.items[i]);
                }
            }
            else
            {
                slots[i].ClearSlot();
                if (i >= lastSlotsColumn)
                {
                    mainInventorySlots[i - lastSlotsColumn].ClearSlot();
                }
            }
            UpdateSlotColor(i);
        }
    }

    private void UpdateSlotColor(int index)
    {
        Image slotImage = slots[index].GetComponent<Image>();
        ChangeSlotColor(slotImage, index);
    }

    public void SelectItem()
    {
        Image slotImage = slots[inventory.tempSelectedItem].GetComponent<Image>();
        slotImage.color = activeSelectColor;
    }

    public void UpdateActiveItem()
    {
        int index = inventory.currentUsingItem - 24;
        Image slotImage = mainInventorySlots[currentActiveItem].GetComponent<Image>();
        ChangeSlotColor(slotImage, currentActiveItem + 24);
        mainInventorySlots[index].GetComponent<Image>().color = selectionColor;
        currentActiveItem = index;
    }

    public void UpdateSelectItem()
    {
        int index = inventory.currentSelectedItem;
        Image slotImage = slots[currentSelectedItem].GetComponent<Image>();
        ChangeSlotColor(slotImage, currentSelectedItem);
        slots[index].GetComponent<Image>().color = selectionColor;
        currentSelectedItem = index;
    }

    private void ChangeSlotColor(Image slotImage, int condition)
    {
        if (condition != inventory.tempSelectedItem)
        {
            if (condition >= (inventory.height - 1) * inventory.width)
            {
                slotImage.color = Color.yellow;
            }
            else
            {
                slotImage.color = Color.white;
            }
        }
    }

    public void HideUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].HideSlot();
        }
    }
    public void ShowUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ShowSlot();
        }
    }

}
