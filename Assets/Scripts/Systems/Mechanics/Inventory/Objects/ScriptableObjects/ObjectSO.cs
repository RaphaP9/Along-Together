using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectSO", menuName = "ScriptableObjects/Inventory/Object")]
public class ObjectSO : InventoryObjectSO, IHasEmbeddedNumericStats
{
    public override InventoryObjectType GetInventoryObjectType() => InventoryObjectType.Object;
}
