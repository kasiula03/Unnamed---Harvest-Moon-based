using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory", order = 1)]
public class Inventory : ScriptableObject
{
    public int width = 8;
    public int height = 4;
    public int currentSelectedItem = 0;
    public int tempSelectedItem = -1;
    [Range(23, 31)]
    public int currentUsingItem = 23;

    public List<InventoryItem> items = new List<InventoryItem>();
    public GameObject[] objects;
    public GameObject[] meshObjects;

    public UnityEvent inventoryChanged;
    public UnityEvent selectedItemChanged;
    public UnityEvent activeItemChaned;
    public UnityEvent selectItemToMove;

    public void InstanceInventory()
    {
        int startIndex = width * (height - 1);
        objects = new GameObject[items.Count];
        meshObjects = new GameObject[items.Count];

        GenerateObjects(startIndex);
        if (objects[startIndex])
        {
            objects[startIndex].SetActive(true);
        }
        if (meshObjects[startIndex])
        {
            meshObjects[startIndex].SetActive(true);
        }
        currentUsingItem = startIndex;
    }

    private void GenerateObjects(int startIndex)
    {
        for (int i = startIndex; i < items.Count; i++)
        {
            if (items[i].item)
            {
                CreateMeshObject(i);
                CreateObjectInMainInventory(i);
            }
        }
    }

    private void CreateObjectInMainInventory(int i)
    {
        objects[i] = Instantiate(items[i].item.realObject, GameObject.FindGameObjectWithTag("MainInventory").transform);
        foreach (Transform child in objects[i].transform)
        {
            Destroy(child.gameObject);
        }
        if (objects[i])
        {
            objects[i].SetActive(false);
        }
    }

    private void CreateMeshObject(int i)
    {
        Equipment equipment = GameObject.FindGameObjectWithTag("Player").GetComponent<Equipment>();
        meshObjects[i] = equipment.AddEquipment(items[i].item);
        if (meshObjects[i])
        {
            meshObjects[i].SetActive(false);
        }
    }

    public void Add(Item item)
    {
        int index = SameTypeItemIndex(item);
        if (index > -1)
        {
            InventoryItem tempItem = items[index];
            tempItem.amount++;
            items[index] = tempItem;
        }
        else
        {
            AddItemInFreeSpot(item);
        }
        inventoryChanged.Invoke();

    }

    private void AddItemInFreeSpot(Item item)
    {
        int index = FirstFreeSpace();
        if (index > -1)
        {
            InventoryItem inventoryItem = items[index];
            inventoryItem.item = item;
            inventoryItem.amount = 1;
            items[index] = inventoryItem;
            if (index >= width * (height - 1))
            {
                CreateObjectInMainInventory(index);
                CreateMeshObject(index);
            }
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }

    private int SameTypeItemIndex(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Item inventoryItem = items[i].item;
            if (inventoryItem)
            {
                if (inventoryItem.icon.Equals(item.icon) && inventoryItem.realObject.Equals(item.realObject))
                {
                    return i;
                }
            }
        }

        return -1;
    }

    private int FirstFreeSpace()
    {
        int index = items.Count - 1;
        while (items[index].item)
        {
            index--;
            if (index < 0)
            {
                return -1;
            }
        }
        return index;
    }

    public void Remove(int index)
    {
        items[index].item = null;
        items[index].amount = 0;
        Destroy(objects[currentSelectedItem]);
        inventoryChanged.Invoke();
    }

    public bool PullOutSelectedObject()
    {
        bool hasItem = SelectedSlotHasItem();
        if (!hasItem)
        {
            return false;
        }

        PickableObject pickable = Instantiate(PullSelectedItem().realObject).GetComponent<PickableObject>();
        FindObjectOfType<HandUsage>().currentPicking = pickable;
        pickable.InteractMyself();
        return true;
    }

    public void MoveSelectVertical(int direction)
    {
        int shift = direction;
        int proposal = currentSelectedItem + shift;

        UpdateSelectedItem(proposal);
        selectedItemChanged.Invoke();
    }

    public void MoveSelectHorizontal(int direction)
    {
        int shift = width * direction;
        int proposal = currentSelectedItem + shift;

        UpdateSelectedItem(proposal);
        selectedItemChanged.Invoke();
    }

    public void SetCurrentUsingItem(int number)
    {
        int index = 23 + number;
        if (objects[currentUsingItem])
        {
            objects[currentUsingItem].SetActive(false);
        }
        if (meshObjects[currentUsingItem])
        {
            meshObjects[currentUsingItem].SetActive(false);
        }

        if (objects[index])
        {
            if (objects[index].GetComponent<PickableObject>().isTool)
            {
                objects[index].SetActive(true);
                if (meshObjects[index])
                {
                    meshObjects[index].SetActive(true);
                }

            }

        }
        currentUsingItem = index;
        activeItemChaned.Invoke();
    }

    public void MoveItemInInventory()
    {
        if (tempSelectedItem == -1)
        {
            tempSelectedItem = currentSelectedItem;
            selectItemToMove.Invoke();
        }
        else
        {
            int toUpdate = currentSelectedItem;
            Swap<InventoryItem>(items, tempSelectedItem, toUpdate);
            Swap<GameObject>(objects, tempSelectedItem, toUpdate);
            tempSelectedItem = -1;
            inventoryChanged.Invoke();
        }
    }

    private Item PullSelectedItem()
    {
        Item item = items[currentSelectedItem].item;
        Remove(currentSelectedItem);
        return item;
    }

    private GameObject PullSelectedItemObject()
    {
        GameObject inventoryObject = objects[currentSelectedItem];
        inventoryObject.SetActive(true);
        Remove(currentSelectedItem);
        return inventoryObject;
    }

    private bool SelectedSlotHasItem()
    {
        return items[currentSelectedItem].item;
    }

    private void UpdateSelectedItem(int proposal)
    {
        if (proposal >= 0 && proposal < width * height)
        {
            currentSelectedItem = proposal;
        }
    }

    private void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    [Serializable]
    public class InventoryItem
    {
        public Item item;
        public int amount;
    }
}
