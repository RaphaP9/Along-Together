using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MechanicsUtilities
{
    public const float PERSPECTIVE_SCALE_X = 1f;
    public const float PERSPECTIVE_SCALE_Y = 1f;

    private const int ARMOR_THRESHOLD_50_PERCENT = 10;
    private const int EXECUTE_DAMAGE = 999;

    #region Perspective
    public static Vector2 ScaleVector2ToPerspective(Vector2 baseVector)
    {
        Vector2 scaledVector = new Vector2(baseVector.x * PERSPECTIVE_SCALE_X, baseVector.y * PERSPECTIVE_SCALE_Y);
        return scaledVector;
    }
    #endregion

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
