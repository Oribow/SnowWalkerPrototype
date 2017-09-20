using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour {

    public Item[] itemssss;

    public static Item[] items;

    void Awake()
    {
        items = itemssss;
    }

    public static Item GetItem(int id)
    {
        foreach (var i in items)
        {
            if (i.id == id)
                return i;
        }
        throw new System.ArgumentException();
    }
}
