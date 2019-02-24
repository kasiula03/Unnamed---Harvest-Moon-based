using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System;

public class InputController : AutoMoveController
{

    private bool calendarOpened = false;
    private bool inventoryOpened = false;
    private bool inventoryItemAction = false;
    protected bool actionExecuting = false;

    private const String calendarButton = "CalendarAction";
    private const String inventoryButton = "InventoryAction";
    private const String inventoryItemButton = "InventoryItemAction";

    public UnityEvent openCalendarEvent;
    public UnityEvent hideCalendarEvent;
    public UnityEvent openInventoryEvent;
    public UnityEvent hideInventoryEvent;

    public PickableObject pickingObject;

    public Inventory inventory;

    protected void OnStart()
    {
        inventory.InstanceInventory();
        hideCalendarEvent.Invoke();
        hideInventoryEvent.Invoke();
    }

    protected void KeyboardActions()
    {
        for (int i = 1; i < 9; ++i)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                inventory.SetCurrentUsingItem(i);
            }
        }

        if(Input.mouseScrollDelta.y != 0)
        {
            int toolNumber = inventory.currentUsingItem - 23 - (int)Input.mouseScrollDelta.y;
            if (toolNumber > 0 && toolNumber < 9)
            {
                inventory.SetCurrentUsingItem(toolNumber);
            }
        }

        if (Input.GetButtonUp(calendarButton))
        {
            if (calendarOpened)
            {
                hideCalendarEvent.Invoke();
                calendarOpened = false;
            }
            else
            {
                openCalendarEvent.Invoke();
                calendarOpened = true;
            }
        }

        if (Input.GetButtonUp(inventoryButton))
        {
            if (inventoryOpened)
            {
                hideInventoryEvent.Invoke();
                inventoryOpened = false;
            }
            else
            {
                openInventoryEvent.Invoke();
                inventoryOpened = true;
            }
        }

        if (inventoryOpened)
        {
            HandleInventoryActions();
        }


        if (Input.GetButtonUp(inventoryItemButton))
        {
            if (inventoryItemAction || pickingObject)
            {
                HidePickingItemToInventory();
                inventoryItemAction = false;
            }
            else
            {
                inventoryItemAction = PullOutSelectedItem();
            }
        }
    }

    protected bool PlayerCanMove()
    {
        return !(calendarOpened || inventoryOpened || actionExecuting);
    }

    private void HandleInventoryActions()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetButtonDown("Horizontal") && input.x != 0)
        {
            inventory.MoveSelectVertical((int)input.x);
        }
        if (Input.GetButtonDown("Vertical") && input.y != 0)
        {
            inventory.MoveSelectHorizontal(-(int)input.y);
        }

        if (Input.GetButtonDown("ToolAction"))
        {
            inventory.MoveItemInInventory();
        }

    }

    private bool PullOutSelectedItem()
    {
        return inventory.PullOutSelectedObject();
    }

    private void HidePickingItemToInventory()
    {
        if (pickingObject)
        {
            inventory.Add(pickingObject.item);
            pickingObject.HideInInventory();
            pickingObject = null;
        }
    }
}
