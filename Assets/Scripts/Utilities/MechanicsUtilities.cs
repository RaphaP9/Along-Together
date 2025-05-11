using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MechanicsUtilities
{
    public const float PERSPECTIVE_SCALE_X = 1f;
    public const float PERSPECTIVE_SCALE_Y = 1f;

    private const int ARMOR_THRESHOLD_50_PERCENT = 10;
    private const int EXECUTE_DAMAGE = 999;

    private const string PERCENTAGE_CHARACTER = "%";

    #region Perspective
    public static Vector2 ScaleVector2ToPerspective(Vector2 baseVector)
    {
        Vector2 scaledVector = new Vector2(baseVector.x * PERSPECTIVE_SCALE_X, baseVector.y * PERSPECTIVE_SCALE_Y);
        return scaledVector;
    }
    #endregion

    #region Const GetMethods
    public static int GetArmor50PercentThreshold() => ARMOR_THRESHOLD_50_PERCENT;
    public static int GetExecuteDamage() => EXECUTE_DAMAGE;
    #endregion

    #region Damage Evaluation & Processing

    public static bool EvaluateCritAttack(float critChance)
    {
        float randomNumber = Random.Range(0f, 1f);

        if (critChance >= randomNumber) return true;
        return false;
    }

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

    public static int CalculateCritDamage(int nonCritDamage, float attackCritDamageMultiplier)
    {
        float critDamage = nonCritDamage * attackCritDamageMultiplier;
        int roundedCritDamage = Mathf.CeilToInt(critDamage);

        return roundedCritDamage;
    }
    #endregion

    #region Damage Dealing

    public static void DealDamageInAreas(List<Vector2> positions, float areaRadius, DamageData damageData , LayerMask layermask)
    {
        List<Transform> detectedTransforms = GeneralUtilities.DetectTransformsInMultipleRanges(positions, areaRadius, layermask);
        List<IHasHealth> entityHealthsInRange = GeneralUtilities.TryGetGenericsFromTransforms<IHasHealth>(detectedTransforms);

        foreach (IHasHealth iHasHealth in entityHealthsInRange)
        {
            iHasHealth.TakeDamage(damageData);
        }
    }

    public static void DealDamageInAreas(List<Vector2> positions, float areaRadius, DamageData damageData, LayerMask layermask, List<Transform> exeptionTransforms)
    {
        List<Transform> detectedTransforms = GeneralUtilities.DetectTransformsInMultipleRanges(positions, areaRadius, layermask);

        foreach(Transform exceptionTransform in exeptionTransforms)
        {
            detectedTransforms.Remove(exceptionTransform);
        }

        List<IHasHealth> entityHealthsInRange = GeneralUtilities.TryGetGenericsFromTransforms<IHasHealth>(detectedTransforms);

        foreach (IHasHealth iHasHealth in entityHealthsInRange)
        {
            iHasHealth.TakeDamage(damageData);
        }
    }

    public static void DealDamageInArea(Vector2 position, float areaRadius, DamageData damageData, LayerMask layermask)
    {
        List<Transform> detectedTransforms = GeneralUtilities.DetectTransformsInRange(position, areaRadius, layermask);
        List<IHasHealth> entityHealthsInRange = GeneralUtilities.TryGetGenericsFromTransforms<IHasHealth>(detectedTransforms);

        foreach (IHasHealth iHasHealth in entityHealthsInRange)
        {
            iHasHealth.TakeDamage(damageData);
        }
    }

    public static void DealDamageInArea(Vector2 position, float areaRadius, DamageData damageData, LayerMask layermask, List<Transform> exceptionTransforms)
    {
        List<Transform> detectedTransforms = GeneralUtilities.DetectTransformsInRange(position, areaRadius, layermask);

        foreach (Transform exceptionTransform in exceptionTransforms)
        {
            detectedTransforms.Remove(exceptionTransform);
        }

        List<IHasHealth> entityHealthsInRange = GeneralUtilities.TryGetGenericsFromTransforms<IHasHealth>(detectedTransforms);

        foreach (IHasHealth iHasHealth in entityHealthsInRange)
        {
            iHasHealth.TakeDamage(damageData);
        }
    }

    public static bool DealDamageToTransform(Transform transform, DamageData damageData)
    {
        bool damaged = false;

        if(GeneralUtilities.TryGetGenericFromTransform<IHasHealth>(transform, out var iHasHealth))
        {
            damaged = iHasHealth.TakeDamage(damageData);
        }

        return damaged;
    }

    #endregion

    #region Projectiles
    public static Vector2 DeviateShootDirection(Vector2 shootDirection, float dispersionAngle)
    {
        float randomAngle = Random.Range(-dispersionAngle, dispersionAngle);

        Vector2 deviatedDirection = GeneralUtilities.RotateVector2ByAngleDegrees(shootDirection, randomAngle);
        deviatedDirection.Normalize();

        return deviatedDirection;
    }
    #endregion

    #region StatUIProcessing
    public static string ProcessCurrentValueToSimpleInt(float currentValue)
    {
        int intValue = Mathf.RoundToInt(currentValue);
        string stringValue = intValue.ToString();
        return stringValue;
    }

    public static string ProcessCurrentValueToSimpleFloat(float currentValue, int decimalPlaces)
    {
        float floatValue = GeneralUtilities.RoundToNDecimalPlaces(currentValue, decimalPlaces);
        string stringValue = floatValue.ToString();
        return stringValue;
    }

    public static string ProcessCurrentValueToPercentage(float currentValue, int decimalPlaces)
    {
        float floatValue = GeneralUtilities.RoundToNDecimalPlaces(currentValue, decimalPlaces);
        string stringValue = TransformToPercentage(floatValue);
        return stringValue;
    }

    public static string ProcessCurrentValueToExcessPercentage(float currentValue, int decimalPlaces)
    {
        float floatValue = GeneralUtilities.RoundToNDecimalPlaces(currentValue, decimalPlaces);
        floatValue = floatValue - 1;
        string stringValue = TransformToPercentage(floatValue);
        return stringValue;
    }

    public static string TransformToPercentage(float value)
    {
        float percentageValue = value * 100;
        string stringValue = percentageValue.ToString() + PERCENTAGE_CHARACTER;
        return stringValue;
    }
    #endregion

    #region Abilities

    public static AbilityLevel GetNextAbilityLevel(AbilityLevel previousAbilityLevel)
    {
        switch (previousAbilityLevel)
        {
            case AbilityLevel.NotLearned:
            default:
                return AbilityLevel.Level1;
            case AbilityLevel.Level1:
                return AbilityLevel.Level2;
            case AbilityLevel.Level2:
                return AbilityLevel.Level3;
            case AbilityLevel.Level3:
                return AbilityLevel.Level3;
        }
    }
    #endregion
}