using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TreatSO : InventoryObjectSO
{
    public override InventoryObjectType GetInventoryObjectType() => InventoryObjectType.Treat;
}
