using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCuringCondition : DropCondition
{
    public override bool Check(DraggableComponent draggable)
    {
        //only if the draggable object has an curing script attached
        return draggable.GetComponent<Curing>() != null;
    }
}
