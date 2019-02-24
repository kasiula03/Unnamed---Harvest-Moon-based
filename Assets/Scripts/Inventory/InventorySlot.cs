using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public Image icon;
    public Text amount;

    private Item item;
    private bool uiHidden = true;

    public void AddItem(Inventory.InventoryItem inventoryItem)
    {
        item = inventoryItem.item;

        if(item)
        {
            icon.sprite = item.icon;
            amount.text = inventoryItem.amount.ToString();
            if (!uiHidden)
            {
                icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1);
                icon.enabled = true;
                amount.enabled = true;
            }

        }
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        amount.enabled = false;
    }

    public void HideSlot()
    {
        icon.enabled = false;
        amount.enabled = false;
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0);
        GetComponent<Image>().enabled = false;
        uiHidden = true;
    }

    public void ShowSlot()
    {
        if (item)
        {
            icon.enabled = true;
            amount.enabled = true;
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1);
        }
        GetComponent<Image>().enabled = true;
        uiHidden = false;
    }
}
