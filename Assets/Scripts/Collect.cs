using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : IFocusable
{
    public GameObject collectionWindow;
    public Transform itemTextRoot;
    public GameObject itemText;
    public Inventory playerInv;

    public float timeToSearch = 60;

    bool windowOpen;
    float lastY = -5;

    public void collect()
    {
        List<ItemStack> collectedItems = new List<ItemStack>();
        foreach (var i in BiomeManager.currentBiome.droppingItems)
        {
            float rnd = Random.value;
            foreach (var c in i.chances)
            {
                if (c.chance >= rnd)
                {
                    if (c.amount > 0)
                        collectedItems.Add(new ItemStack() { itemID = i.itemId, itemAmount = c.amount });
                    break;
                }
                rnd -= c.chance;
            }
        }
        playerInv.AddItems(collectedItems);
        collectionWindow.SetActive(true);
        foreach (var s in collectedItems)
        {
            GameObject clone = Instantiate<GameObject>(itemText, itemTextRoot);
            clone.GetComponent<UnityEngine.UI.Text>().text = ItemHolder.GetItem(s.itemID).name + " x" + s.itemAmount;
            Vector3 lp = clone.transform.localPosition;
            lp.y = lastY;
            clone.transform.localPosition = lp;
            lastY -= 20;
        }
        WorldTime.WrapTime(timeToSearch);
        windowOpen = true;
    }

    public void CloseWindow()
    {
        if (windowOpen)
        {
            Transform[] childs = itemTextRoot.gameObject.GetComponentsInChildren<Transform>();
            foreach (var c in childs)
            {
                if(c != itemTextRoot)
                Destroy(c.gameObject);
            }
            collectionWindow.SetActive(false);
            windowOpen = false;
            lastY = -5;
        }
    }

    public override void OnGainFocus()
    {
        collect();
    }

    public override void OnLoseFocus()
    {
        CloseWindow();
    }
}

[System.Serializable]
public class ItemStack
{
    public int itemID;
    public int itemAmount;
}
