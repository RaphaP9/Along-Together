using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteTreatHandler : TreatHandler
{
    public static ExecuteTreatHandler Instance { get; private set; }

    private ExecuteTreatConfigSO ExecuteTreatConfigSO => treatConfigSO as ExecuteTreatConfigSO;

    private PlayerHealth playerHealth;

    private void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
    }

    private void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Abstract Methods
    protected override void OnTreatActivatedByInventoryObjectsMethod() { Debug.Log("Enabled By InvObjects."); }
    protected override void OnTreatDeactivatedByInventoryObjectsMethod() { Debug.Log("Disabled By InvObjects."); }
    protected override void OnTreatEnablementByConditionMethod() { Debug.Log("Enabled By Condition."); }
    protected override void OnTreatDisablementByConditionMethod() { Debug.Log("Disabled By Condition."); }
    protected override bool EnablementCondition() => playerHealth.CurrentHealth <= 5;
    #endregion


}
