using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldRegenStatResolver : EntityIntStatResolver
{
    private CharacterSO CharacterSO => entitySO as CharacterSO;

    protected virtual void OnEnable()
    {
        ShieldRegenStatResolver.OnShieldRegenResolverUpdated += ShieldRegenStatResolver_OnShieldRegenResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        ShieldRegenStatResolver.OnShieldRegenResolverUpdated -= ShieldRegenStatResolver_OnShieldRegenResolverUpdated;
    }

    protected override int CalculateStat()
    {
        return ShieldRegenStatResolver.Instance.ResolveStatInt(CharacterSO.baseShieldRegen);
    }

    private void ShieldRegenStatResolver_OnShieldRegenResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}
