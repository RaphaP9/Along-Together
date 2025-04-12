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

    public void TakeDamage(int damage, bool isCrit, IDamageSource damageSource);
    public void Heal(int healAmount, IHealSource healSource);
    public void RestoreShield(int shieldAmount, IShieldSource healSource);

    public void InstaKill(IDamageSource damageSource);
    public void HealCompletely(IHealSource healSource);
    public void RestoreShieldCompletely(IShieldSource shieldSource);

    public bool HasShield();
    public bool IsAlive();
    public bool IsFullHealth();
    public bool IsFullShield();
}


