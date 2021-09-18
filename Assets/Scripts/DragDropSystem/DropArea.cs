using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DropArea : MonoBehaviour
{
    public List<DropCondition> DropConditions = new List<DropCondition>();

    public event Action<DraggableComponent> OnDropHandler;

    public Item item;
    public int itemID;

    //Return true if all conditions are met for a draggable object
    public bool Accepts(DraggableComponent draggable)
    {
        return DropConditions.TrueForAll(cond => cond.Check(draggable));

    }

    //Transform this event to return the item that is dropped as a variable an read it from itemsInventory class inside the equipment
    public Item Drop(DraggableComponent draggable)
    {
        OnDropHandler?.Invoke(draggable);          
        item = draggable.GetComponentInParent<ButtonInfoLab>().GetItem();//read the item type
        //Read the ID item from the buttoninfolab?
        itemID = draggable.GetComponentInParent<ButtonInfoLab>().GetID();
        return item;
        
    }



}
