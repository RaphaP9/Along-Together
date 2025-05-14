using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmorStatResolver : EntityArmorStatResolver
{
    private CharacterSO CharacterSO => entitySO as CharacterSO;

    protected virtual void OnEnable()
    {
        ArmorStatResolver.OnArmorResolverUpdated += ArmorStatResolver_OnArmorResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        ArmorStatResolver.OnArmorResolverUpdated -= ArmorStatResolver_OnArmorResolverUpdated;
    }

    protected override int CalculateStat()
    {
        return ArmorStatResolver.Instance.ResolveStatInt(CharacterSO.baseArmor);
    }

    private void ArmorStatResolver_OnArmorResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}
