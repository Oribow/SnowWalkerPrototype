using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        Weapon,
        Item,
        Placeable,
        Consumable
    }

    public string name;
    public int id;
    public ItemType itemType;
    public GameObject placableObject;

    public bool IsUsable()
    {
        return itemType != ItemType.Item;
    }

    public string GetUseButtonText()
    {
        switch (itemType)
        {
            case ItemType.Consumable:
                return "Consume";
            case ItemType.Placeable:
                return "Place";
            case ItemType.Weapon:
                return "Equip";
        }
        return "";
    }

    public void Use(Inventory inv)
    {
        switch (itemType)
        {
            case ItemType.Consumable:
                return;
            case ItemType.Placeable:
                GameObject.Instantiate(placableObject, new Vector3(inv.transform.position.x + 5, 2f, 0), Quaternion.identity);
                inv.RemoveItem(new ItemStack() { itemAmount = 1, itemID = id });
                return;
            case ItemType.Weapon:
                inv.EquipWeapon(this);
                return;
        }
        throw new System.ArgumentException();
    }

}
