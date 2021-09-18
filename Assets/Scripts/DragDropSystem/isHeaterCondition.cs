using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsHeaterCondition : DropCondition
{
    public override bool Check(DraggableComponent draggable)
    {
        //only if the draggable object has an Heater script attached
        return draggable.GetComponent<Heater>() != null;
    }
}
