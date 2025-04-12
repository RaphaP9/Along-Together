using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MechanicsUtilities
{
    private const int ARMOR_THRESHOLD_50_PERCENT = 10;
    private const int EXECUTE_DAMAGE = 999;

    #region DamageTakenProcessing
    public static bool EvaluateDodgeChance(float dodgeChance)
    {
        float randomNumber = Random.Range(0f, 1f);

        if (dodgeChance >= randomNumber) return true;
        return false;
    }

    public static int MitigateDamageByArmor(int baseDamage, int armor)
    {
        if(armor < 0) armor = 0;

        float rawResultingDamage = (float) baseDamage / (1 + armor/ARMOR_THRESHOLD_50_PERCENT);
        int roundedResultingDamage = Mathf.CeilToInt(rawResultingDamage);

        return roundedResultingDamage;
    }
    #endregion

    #region Const GetMethods
    public static int GetArmor50PercentThreshold() => ARMOR_THRESHOLD_50_PERCENT;
    public static int GetExecuteDamage() => EXECUTE_DAMAGE;
    #endregion
}
