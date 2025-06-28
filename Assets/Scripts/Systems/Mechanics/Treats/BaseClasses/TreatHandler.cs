using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TreatHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected TreatConfigSO treatConfigSO;

    [Header("Runtime Filled")]
    [SerializeField] protected bool isCurrentlyActiveByInventoryObjects;
    [SerializeField] protected bool isMeetingCondition;

    protected List<InventoryObjectSO> InventoryObjectsToActivate => treatConfigSO.activatorInventoryObjects;

    protected bool previouslyActiveByInventoryObjects = false;
    protected bool previouslyMeetingCondition = false;

    public static event EventHandler<OnTreatConfigEventArgs> OnTreatActivatedByInventoryObjects;
    public static event EventHandler<OnTreatConfigEventArgs> OnTreatDeactivatedByInventoryObjects;
    public static event EventHandler<OnTreatConfigEventArgs> OnTreatEnablementByCondition;
    public static event EventHandler<OnTreatConfigEventArgs> OnTreatDisablementByCondition;

    public class OnTreatConfigEventArgs : EventArgs
    {
        public TreatConfigSO treatConfigSO;
    }

    private void Awake()
    {
        SetSingleton();
    }

    protected virtual void Update()
    {
        HandleTreatActiveByInventoryObjects();
        HandleTreatEnablementByConditio();
    }

    #region Abstract/Virtual Methods
    protected abstract void SetSingleton();

    protected virtual void OnTreatActivatedByInventoryObjectsMethod() //Will trigger as soon as activeByInventoryObjects
    {
        OnTreatActivatedByInventoryObjects?.Invoke(this, new OnTreatConfigEventArgs { treatConfigSO = treatConfigSO });
    }

    protected virtual void OnTreatDeactivatedByInventoryObjectsMethod()//Will trigger as soon as deactiveByInventoryObjects
    {
        OnTreatDeactivatedByInventoryObjects?.Invoke(this, new OnTreatConfigEventArgs { treatConfigSO = treatConfigSO});
    }

    protected virtual void OnTreatEnablementByConditionMethod() //Will be triggered as soon as meeting condition and  was activeByInventoryObject/ as soon as active by activeByInventoryObjects and was meeting condition
    {
        OnTreatEnablementByCondition?.Invoke(this, new OnTreatConfigEventArgs { treatConfigSO = treatConfigSO });
    }

    protected virtual void OnTreatDisablementByConditionMethod() //Will be triggered as soon as not meeting condition and was activeByInventoryObject / as soon as deactiveByInventoryObject and was meeting condition
    {
        OnTreatDisablementByCondition?.Invoke(this, new OnTreatConfigEventArgs { treatConfigSO = treatConfigSO });
    }

    protected virtual bool EnablementCondition() => true; //As default the EnablementCondition will always be met, override in inheritances otherwise
    #endregion

    #region Logic Of Activation By InvObjects / Enablement By Condition
    private bool IsActiveByInventoryObjects()
    {
        foreach(InventoryObjectSO inventoryObjectSO in treatConfigSO.activatorInventoryObjects)
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

    private void HandleTreatActiveByInventoryObjects()
    {
        bool currentlyActiveByInventoryObjects = IsActiveByInventoryObjects();

        if(!previouslyActiveByInventoryObjects && currentlyActiveByInventoryObjects)
        {
            OnTreatActivatedByInventoryObjectsMethod();
            if (isMeetingCondition) OnTreatEnablementByConditionMethod();
        }
        else if(previouslyActiveByInventoryObjects && !currentlyActiveByInventoryObjects)
        {
            if (isMeetingCondition) OnTreatDisablementByConditionMethod();
            OnTreatDeactivatedByInventoryObjectsMethod();
        }

        isCurrentlyActiveByInventoryObjects = currentlyActiveByInventoryObjects;
        previouslyActiveByInventoryObjects = isCurrentlyActiveByInventoryObjects;       
    }

    private void HandleTreatEnablementByConditio()
    {
        bool meetingCondition = EnablementCondition();

        if (!previouslyMeetingCondition && meetingCondition)
        {
            if(isCurrentlyActiveByInventoryObjects) OnTreatEnablementByConditionMethod();
        }
        else if (previouslyMeetingCondition && !meetingCondition)
        {
            if (isCurrentlyActiveByInventoryObjects) OnTreatDisablementByConditionMethod();
        }

        isMeetingCondition = meetingCondition;
        previouslyMeetingCondition = isMeetingCondition;
    }
    #endregion
}
