using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject {

    public GameObject realObject;
    public Sprite icon;
    public ItemType itemType;
    
    public enum ItemType
    {
        None,
        Legs,
        Chest,
        Head,
        HandRight,
        HandLeft
    }
}
