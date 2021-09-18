using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsIngredientCondition : DropCondition
{
    public override bool Check(DraggableComponent draggable)
    {
        //only if the draggable object has an Ingredient script attached
        return draggable.GetComponent<Ingredient>() != null;
    }
}
