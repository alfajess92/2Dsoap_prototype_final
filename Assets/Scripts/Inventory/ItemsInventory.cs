using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

//This script is attached to each prefab of item_lab (curing or heater)
public class ItemsInventory : MonoBehaviour
{
    //Create an empty list of item
    public List<ItemsList> Container = new List<ItemsList>();
    
    //public IEnumerable<ItemsList> Container = new List<ItemsList>();
    //public int? totalAmount;

    public void AddItemTo(Item _item, int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }
        if (!hasItem)
        {
            Container.Add(new ItemsList(_item, _amount));

        }


    }


    //Show in the inspector
    [Serializable]
    public class ItemsList
    {
        public Item item;
        public int amount;
        //public int itemId;

        public ItemsList(Item _item, int _amount)
        {
            item = _item;
            amount = _amount;
            //itemId = _itemId;

        }

        public void AddAmount(int value)
        {
            amount += value;

        }

    }
}