using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreatSO", menuName = "ScriptableObjects/Inventory/Treat")]
public class TreatSO : InventoryObjectSO
{
    public override InventoryObjectType GetInventoryObjectType() => InventoryObjectType.Treat;
}
