using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaxShieldStatResolver : EntityMaxShieldStatResolver
{
    private CharacterSO CharacterSO => entitySO as CharacterSO;

    protected virtual void OnEnable()
    {
        MaxShieldStatResolver.OnMaxShieldResolverUpdated += MaxShieldStatResolver_OnMaxShieldResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        MaxShieldStatResolver.OnMaxShieldResolverUpdated -= MaxShieldStatResolver_OnMaxShieldResolverUpdated;
    }

    protected override int CalculateStat()
    {
        return MaxShieldStatResolver.Instance.ResolveStatInt(CharacterSO.baseShield);
    }

    private void MaxShieldStatResolver_OnMaxShieldResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}
