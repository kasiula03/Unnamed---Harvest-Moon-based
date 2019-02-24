using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{

    public GameObject avatar;

    public GameObject wornLegs;
    public GameObject wornChest;
    public GameObject wornHair;
    public GameObject wornBeard;
    public GameObject wornMustache;
    public GameObject wornHandRight;
    public GameObject wornChestArmor;

    private Stitcher stitcher;

    public void Awake()
    {
        stitcher = new Stitcher();
    }

    public GameObject AddEquipment(Item equipmentToAdd)
    {
        GameObject addedObject = null;
        switch (equipmentToAdd.itemType)
        {
            case Item.ItemType.HandRight:
                addedObject = AddEquipmentHelper(wornHandRight, equipmentToAdd);
                wornHandRight = addedObject;
                break;
            default:
                break;
        }
        return addedObject;
    }

    public GameObject AddEquipmentHelper(GameObject wornItem, Item itemToAddToWornItem)
    {
        wornItem = Wear(itemToAddToWornItem.realObject, wornItem);
        wornItem.name = itemToAddToWornItem.realObject.name;
        return wornItem;
    }

    private GameObject Wear(GameObject clothing, GameObject wornClothing)
    {
        if (clothing == null)
            return null;
        clothing = (GameObject)GameObject.Instantiate(clothing);
        wornClothing = stitcher.Stitch(clothing, avatar);
        GameObject.Destroy(clothing);
        return wornClothing;
    }
}
