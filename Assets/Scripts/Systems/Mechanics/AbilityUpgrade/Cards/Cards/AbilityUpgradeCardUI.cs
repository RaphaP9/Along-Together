using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUpgradeCardUI : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private AbilityUpgradeCardInfo abilityUpgradeCardInfo;

    public AbilityUpgradeCardInfo AbilityUpgradeCardInfo => abilityUpgradeCardInfo;

    public event EventHandler<OnAbilityUpgradeCardEventArgs> OnAbilityUpgradeCardSet;

    public class OnAbilityUpgradeCardEventArgs : EventArgs
    {
        public AbilityUpgradeCardInfo abilityUpgradeCardInfo;
    }

    public void SetAbilityUpgradeCardInfo(AbilityUpgradeCardInfo abilityUpgradeCardInfo)
    {
        this.abilityUpgradeCardInfo = abilityUpgradeCardInfo;
        OnAbilityUpgradeCardSet?.Invoke(this, new OnAbilityUpgradeCardEventArgs { abilityUpgradeCardInfo = abilityUpgradeCardInfo });
    }
}
