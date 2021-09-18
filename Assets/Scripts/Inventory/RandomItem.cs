using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Random", menuName = "Inventory System/RandomItem")]

public class RandomItem : Item
{

    public void Awake()
    {
        type = ItemType.Random;
        process = ProcessType.none;
    }


}
