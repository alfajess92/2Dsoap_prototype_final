using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "New Default", menuName = "Inventory System/Default")]
public class Default : Item
{
    // Start is called before the first frame update

    

    public void Awake()
    {
        type = ItemType.Default;
        process = ProcessType.none;
    }

    
}
