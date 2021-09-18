using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSlot : EquipmentSlot
{
    protected override void Awake()
    {
        base.Awake();
        DropArea.DropConditions.Add(new IsIngredientCondition());
        Debug.Log("An ingredient was added to an ingredient slot");
    }
}
