using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunNumericStatModifierManager : NumericStatModifierManager
{
    //RunNumericStatModifierManager will register/unregister stats from objects added to/removed from inventory
    public static RunNumericStatModifierManager Instance { get; private set; }

    protected virtual void OnEnable()
    {
        ObjectsInventoryManager.OnObjectAddedToInventory += ObjectsInventoryManager_OnObjectAddedToInventory;
        ObjectsInventoryManager.OnObjectRemovedFromInventory += ObjectsInventoryManager_OnObjectRemovedFromInventory;
    }

    protected virtual void OnDisable()
    {
        ObjectsInventoryManager.OnObjectAddedToInventory -= ObjectsInventoryManager_OnObjectAddedToInventory;
        ObjectsInventoryManager.OnObjectRemovedFromInventory -= ObjectsInventoryManager_OnObjectRemovedFromInventory;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one RunNumericStatModifierManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region Subscriptions
    private void ObjectsInventoryManager_OnObjectAddedToInventory(object sender, ObjectsInventoryManager.OnObjectEventArgs e)
    {
        AddStatModifiers(e.objectIdentified.assignedGUID, e.objectIdentified.objectSO);
    }

    private void ObjectsInventoryManager_OnObjectRemovedFromInventory(object sender, ObjectsInventoryManager.OnObjectEventArgs e)
    {
        RemoveStatModifiersByGUID(e.objectIdentified.assignedGUID);
    }
    #endregion
}
