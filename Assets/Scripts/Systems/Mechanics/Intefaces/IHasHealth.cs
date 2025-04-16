using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IHasHealth 
{
    public bool CanTakeDamage();
    public bool CanHeal();
    public bool CanRestoreShield();

    public bool TakeDamage(DamageData damageData); //True when attack landed/ projectile impacted, false otherwise (Is not alive or has dodged)
    public void Heal(HealData healData);
    public void RestoreShield(ShieldData shieldData);

    public void Excecute(IDamageSource damageSource);
    public void HealCompletely(IHealSource healSource);
    public void RestoreShieldCompletely(IShieldSource shieldSource);

    public bool HasShield();
    public bool IsAlive();
    public bool IsFullHealth();
    public bool IsFullShield();
}


