using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Intermediate", menuName = "Inventory System/Intermediate")]
public class Intermediate : Item

{
    public void Awake()
    {
        type = ItemType.Intermediate;
        process = ProcessType.none;
    }
}
