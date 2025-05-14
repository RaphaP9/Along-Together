using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthRegenStatResolver : EntityIntStatResolver
{
    private CharacterSO CharacterSO => entitySO as CharacterSO;

    protected virtual void OnEnable()
    {
        HealthRegenStatResolver.OnHealthRegenResolverUpdated += HealthRegenStatResolver_OnHealthRegenResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        HealthRegenStatResolver.OnHealthRegenResolverUpdated -= HealthRegenStatResolver_OnHealthRegenResolverUpdated;
    }

    protected override int CalculateStat()
    {
        return HealthRegenStatResolver.Instance.ResolveStatInt(CharacterSO.baseHealthRegen);
    }

    private void HealthRegenStatResolver_OnHealthRegenResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}
