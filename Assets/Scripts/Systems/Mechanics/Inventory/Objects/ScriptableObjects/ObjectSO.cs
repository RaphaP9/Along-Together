using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectSO", menuName = "ScriptableObjects/Inventory/Object")]
public class ObjectSO : InventoryObjectSO, IHasEmbeddedNumericStats
{
    [Header("Numeric Embedded Stats")]
    public List<NumericEmbeddedStat> numericEmbeddedStats;

    public override InventoryObjectType GetInventoryObjectType() => InventoryObjectType.Object;

    public List<NumericEmbeddedStat> GetNumericEmbeddedStats() => numericEmbeddedStats;
}
