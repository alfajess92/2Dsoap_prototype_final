using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Useful to hold the items

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
//source: https://www.youtube.com/watch?v=_IqTeruf3-s

public class Inventory : ScriptableObject
{

    public List<InventorySlot> Container = new List<InventorySlot>();

    public void AddItem(Item _item, int _amount)
    {
        //check if the item is in the inventory        
       
        bool hasItem = false;
        for (int i=0; i< Container.Count ; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }

        //if the container is empty, this will create a new inventory

        if (!hasItem)
        {
            Container.Add(new InventorySlot(_item, _amount));
        }

    }
    
}


[System.Serializable]

public class InventorySlot
{
    public Item item;
    public int amount;
    public InventorySlot(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

}