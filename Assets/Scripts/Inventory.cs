using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : IFocusable
{

    [HideInInspector]
    public List<ItemStack> collectedItems;

    public GameObject invWindow;
    public Transform itemTextRoot;
    public GameObject itemText;
    public Transform craftTextRoot;
    public GameObject craftButton;
    public UnityEngine.UI.Text currentlyEquipped;

    [SerializeField]
    ItemStack[] preExistingItems;

    [SerializeField]
    CraftingRecept[] recepts;
    Item weapon;

    bool isWindowOpen = false;

    void Awake()
    {
        collectedItems = new List<ItemStack>(5);
        EquipWeapon(null);
        AddItems(preExistingItems);
    }

    public void ToogleInv()
    {
        if (!isWindowOpen)
            OnGainFocus();
        else
            OnLoseFocus();
    }

    public void ShowInv()
    {
        if (isWindowOpen)
            return;

    }

    public void EquipWeapon(Item item)
    {
        if (item == null)
        {
            currentlyEquipped.text = "Equipped: None";
        }
        else
        {
            currentlyEquipped.text = "Equipped: " + item.name;
            weapon = item;
        }
    }

    public void AddItems(IEnumerable<ItemStack> items)
    {
        foreach (var aStack in items)
        {
            foreach (var stack in collectedItems)
            {
                if (stack.itemID == aStack.itemID)
                {
                    stack.itemAmount += aStack.itemAmount;
                    goto Found;
                }
            }
            collectedItems.Add(new ItemStack() { itemAmount = aStack.itemAmount, itemID = aStack.itemID });
            Found:
            continue;
        }
    }

    public bool DoesEnoughItemExits(ItemStack aStack)
    {
        foreach (var stack in collectedItems)
        {
            if (stack.itemID == aStack.itemID)
            {
                if (aStack.itemAmount <= stack.itemAmount)
                    return true;
                else
                    return
                         false;
            }
        }
        return false;
    }

    public void RemoveItem(ItemStack aStack)
    {
        for (int i = 0; i < collectedItems.Count; i++)
        {
            var stack = collectedItems[i];
            if (stack.itemID == aStack.itemID)
            {
                stack.itemAmount -= aStack.itemAmount;
                if (stack.itemAmount <= 0)
                {
                    collectedItems.RemoveAt(i);
                }
                return;
            }
        }
        return;
    }

    public override void OnGainFocus()
    {
        WorldTime.Pause();
        invWindow.SetActive(true);
        float lastY = -5;
        foreach (var s in collectedItems)
        {
            GameObject clone = Instantiate<GameObject>(itemText, itemTextRoot);
            var item = ItemHolder.GetItem(s.itemID);
            var compHolder = clone.GetComponent<ItemEntry>();
            compHolder.useButton.gameObject.SetActive(item.IsUsable() && item != weapon);
            compHolder.buttonText.text = item.GetUseButtonText();
            compHolder.useButton.onClick.AddListener(() => { item.Use(this); ToogleInv(); ToogleInv(); });
            compHolder.itemName.text = item.name + " x" + s.itemAmount;
            Vector3 lp = clone.transform.localPosition;
            lp.y = lastY;
            clone.transform.localPosition = lp;
            lastY -= 20;
        }
        isWindowOpen = true;

        lastY = -5;
        foreach (var r in recepts)
        {
            if (!r.Craftable(this))
                continue;

            GameObject clone = Instantiate<GameObject>(craftButton, craftTextRoot);
            clone.GetComponentInChildren<UnityEngine.UI.Text>().text = r.ToString();
            clone.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { r.Craft(this); });
            Vector3 lp = clone.transform.localPosition;
            lp.y = lastY;
            clone.transform.localPosition = lp;
            lastY -= 30;
        }
    }

    public override void OnLoseFocus()
    {
        WorldTime.Resume();
        invWindow.SetActive(false);
        Transform[] childs = itemTextRoot.gameObject.GetComponentsInChildren<Transform>();
        foreach (var c in childs)
        {
            if (c != itemTextRoot)
                Destroy(c.gameObject);
        }

        childs = craftTextRoot.gameObject.GetComponentsInChildren<Transform>();
        foreach (var c in childs)
        {
            if (c != craftTextRoot)
                Destroy(c.gameObject);
        }
        isWindowOpen = false;
    }

    [System.Serializable]
    class CraftingRecept
    {
        [SerializeField]
        ItemStack[] requiredItems;
        [SerializeField]
        ItemStack[] outcome;

        public bool Craftable(Inventory inv)
        {
            foreach (var r in requiredItems)
            {
                if (!inv.DoesEnoughItemExits(r))
                    return false;
            }
            return true;
        }

        public void Craft(Inventory inv)
        {
            foreach (var r in requiredItems)
            {
                inv.RemoveItem(r);
            }
            inv.AddItems(outcome);
            inv.ToogleInv();
            inv.ToogleInv();
        }

        public override string ToString()
        {
            string s = "";
            foreach (var r in requiredItems)
            {
                s += r.itemAmount + "x " + ItemHolder.GetItem(r.itemID).name + ", ";
            }
            s.Remove(s.Length - 2);
            s += " --> ";
            foreach (var o in outcome)
            {
                s += o.itemAmount + "x " + ItemHolder.GetItem(o.itemID).name + ", ";
            }
            s.Remove(s.Length - 2);
            return s;
        }
    }
}
