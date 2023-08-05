using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singlton<Inventory>
{
    public List<Item> items = new List<Item>();

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 20;

    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= space)
            {
                Debug.Log("Net Mesta");
                return false;
            }

            //Item copyItem = Instantiate(item);

            //for (int i = 0; i < items.Count; i++)
            //{
            //    if (items[i].name == copyItem.name)
            //    {
            //        items[i].itemAmount++;
            //        return true;
            //    }
            //}
            items.Add(item);

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
        return true;
    }
    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}