using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TreatHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TreatSO treatSO;

    public TreatSO TreatSO => treatSO;

    private void Awake()
    {
        SetSingleton();
    }

    protected abstract void SetSingleton();

    protected virtual bool TreatEnabled() => TreatsInventoryManager.Instance.HasTreatSOInInventory(treatSO);
}
