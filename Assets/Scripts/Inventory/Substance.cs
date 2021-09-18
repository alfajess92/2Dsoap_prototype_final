using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Substance", menuName = "Inventory System/Substance")]
public class Substance : Item
{
    public void Awake()
    {
        type = ItemType.Substance;
        process = ProcessType.none;
    }
}
