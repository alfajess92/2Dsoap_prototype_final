using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Product", menuName = "Inventory System/Product")]
public class Product : Item
{
    public void Awake()
    {
        type = ItemType.Product;
        process = ProcessType.none;
    }
}
