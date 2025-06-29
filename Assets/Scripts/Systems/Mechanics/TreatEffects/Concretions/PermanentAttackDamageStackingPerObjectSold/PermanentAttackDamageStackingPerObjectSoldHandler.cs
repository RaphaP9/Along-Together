using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentAttackDamageStackingPerObjectSoldHandler : PermanentStackingTreatEffectHandler
{
    public static PermanentAttackDamageStackingPerObjectSoldHandler Instance { get; private set; }

    private PermanentAttackDamageStackingPerObjectSoldSO PermanentAttackDamageStackingPerObjectSoldSO => treatEffectSO as PermanentAttackDamageStackingPerObjectSoldSO;

    protected void OnEnable()
    {
        EnemyHealth.OnAnyEnemyDeath += EnemyHealth_OnAnyEnemyDeath;
    }

    protected void OnDisable()
    {
        EnemyHealth.OnAnyEnemyDeath -= EnemyHealth_OnAnyEnemyDeath;
    }

    protected override string GetRefferencialGUID() => PermanentAttackDamageStackingPerObjectSoldSO.refferencialGUID;
    protected override NumericEmbeddedStat GetRefferencialNumericEmbeddedStatPerStack() => PermanentAttackDamageStackingPerObjectSoldSO.statPerStack;

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

    protected override void AddStacks(int quantity)
    {
        base.AddStacks(quantity);
        AddProportionalStatForStacks(PermanentAttackDamageStackingPerObjectSoldSO.statPerStack);
    }

    #region Subscriptions
    private void EnemyHealth_OnAnyEnemyDeath(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return;
        if (!isStacking) return;
        if (e.damageSource.GetDamageSourceClassification() != DamageSourceClassification.Character) return;
        AddStacks(1);
    }
    #endregion
}
