using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TreatHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<InventoryObjectSO> inventoryObjectsToActivate;

    public List<InventoryObjectSO> InventoryObjectsToActivate => inventoryObjectsToActivate;

    private void Awake()
    {
        SetSingleton();
    }

    protected abstract void SetSingleton();

    protected virtual bool TreatEnabled()
    {
        foreach(InventoryObjectSO inventoryObjectSO in inventoryObjectsToActivate)
        {
            switch (inventoryObjectSO.GetInventoryObjectType())
            {
                case InventoryObjectType.Object:
                    if (ObjectsInventoryManager.Instance.HasObjectSOInInventory(inventoryObjectSO as ObjectSO)) return true;
                    break;
                case InventoryObjectType.Treat:
                    if (TreatsInventoryManager.Instance.HasTreatSOInInventory(inventoryObjectSO as TreatSO)) return true;
                    break;
            }
        }

        return false;
    }
}
