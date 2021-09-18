using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTEST : MonoBehaviour
{
    public Inventory inventory;

    public void OnTriggerEnter(Collider other)
    {
        //create ItemObject script because Item class is abstract
        var item = other.GetComponent<ItemObject>();
        if (item)
        {
            //check instead item.item
            inventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }

    //To remove the items from the inventory
    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}
