using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public static class MappingUtilities 
{
    #region Object Classification Consts 
    private const string COMMON_OBJECT_TEXT = "Objeto Común";
    private const string UNCOMMON_OBJECT_TEXT = "Objeto Poco Común";
    private const string RARE_OBJECT_TEXT = "Objeto Raro";
    private const string EPIC_OBJECT_TEXT = "Objeto Épico";
    private const string LEGENDARY_OBJECT_TEXT = "Objeto Legendario";

    private const string COMMON_TREAT_TEXT = "Treat Común";
    private const string UNCOMMON_TREAT_TEXT = "Treat Poco Común";
    private const string RARE_TREAT_TEXT = "Treat Rara";
    private const string EPIC_TREAT_TEXT = "Treat Épica";
    private const string LEGENDARY_TREAT_TEXT = "Treat Legendaria";
    #endregion

    #region Numeric Stat Consts
    private const string MAX_HEALTH_STAT = "Vida Máxima";
    private const string MAX_SHIELD_STAT = "Escudo Máximo";
    private const string HEALTH_REGEN_STAT = "Regen. de Vida";
    private const string SHIELD_REGEN_STAT = "Regen. de Escudo";
    private const string ARMOR_STAT = "Armadura";
    private const string DODGE_CHANCE_STAT = "Evasión";
    private const string ATTACK_DAMAGE_STAT = "Daño de Ataque";
    private const string ATTACK_SPEED_STAT = "Vel. de Ataque";
    private const string ATTACK_CRIT_CHANCE_STAT = "Prob. de Crítico";
    private const string ATTACK_CRIT_DAMAGE_MULTIPLIER_STAT = "Daño Crítico";
    private const string COOLDOWN_REDUCTION_STAT = "Red. de Enfriamiento";
    private const string LIFESTEAL_STAT = "Robo de Vida";
    private const string MOVEMENT_SPEED_STAT = "Vel. de Movimiento";
    private const string GOLD_STAT = "Riqueza";
    #endregion

    private const string PERCENTAGE_CHARACTER = "%";
    private const string PLUS_CHARACTER = "+";

    #region Stats

    public static NumericStatEvaluationWay GetStatEvaluationWay(NumericStatType numericStatType)
    {
        switch (numericStatType)
        {
            case NumericStatType.MaxHealth:
            case NumericStatType.MaxShield:
            case NumericStatType.Armor:
            case NumericStatType.HealthRegen:
            case NumericStatType.ShieldRegen:
            case NumericStatType.AttackDamage:
            case NumericStatType.MovementSpeed:
            case NumericStatType.AttackSpeed:
            case NumericStatType.AttackCritChance:
            case NumericStatType.AttackCritDamageMultiplier:
            case NumericStatType.DodgeChance:
            case NumericStatType.CooldownReduction:
            case NumericStatType.Lifesteal:
            case NumericStatType.Gold:
            default:
                return NumericStatEvaluationWay.PositiveAvobeBase; //For now, all stat values are treated as positive while avobe base value
        }
    }

    public static NumericStatState GetNumericStatState(NumericStatType numericStatType, float currentValue, float baseValue)
    {
        if (currentValue > baseValue)
        {
            if (GetStatEvaluationWay(numericStatType) == NumericStatEvaluationWay.PositiveAvobeBase) return NumericStatState.Positive;
            if (GetStatEvaluationWay(numericStatType) == NumericStatEvaluationWay.NegativeAvobeBase) return NumericStatState.Negative;
        }

        if (currentValue < baseValue)
        {
            if (GetStatEvaluationWay(numericStatType) == NumericStatEvaluationWay.PositiveAvobeBase) return NumericStatState.Negative;
            if (GetStatEvaluationWay(numericStatType) == NumericStatEvaluationWay.NegativeAvobeBase) return NumericStatState.Positive;
        }

        return NumericStatState.Neutral;
    }

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

    public static string ProcessNumericStatValueToString(NumericStatType numericStatType, float value)
    {
        switch (numericStatType)
        {
            case NumericStatType.MaxHealth:
            case NumericStatType.MaxShield:
            case NumericStatType.Armor:
            case NumericStatType.HealthRegen:
            case NumericStatType.ShieldRegen:
            case NumericStatType.AttackDamage:
                return ProcessCurrentValueToSimpleInt(value);
            case NumericStatType.MovementSpeed:
            case NumericStatType.AttackSpeed:
            default:
                return ProcessCurrentValueToSimpleFloat(value, 2);
            case NumericStatType.AttackCritChance:
            case NumericStatType.AttackCritDamageMultiplier:
            case NumericStatType.DodgeChance:
            case NumericStatType.CooldownReduction:
            case NumericStatType.Lifesteal:
            case NumericStatType.Gold:
                return ProcessCurrentValueToPercentage(value, 2);
        }
    }

    public static string ProcessObjectNumericStatValueToString(NumericStatType numericStatType, NumericStatModificationType numericStatModificationType, float value)
    {
        string processedString = "";

        switch (numericStatModificationType)
        {
            case NumericStatModificationType.Value:
            default:
                processedString = ProcessObjectValueNumericStatValueToString(numericStatType, value);
                break;
            case NumericStatModificationType.Percentage:
                processedString = ProcessObjectPercentageNumericStatValueToString(numericStatType, value);
                break;
            case NumericStatModificationType.Replacement:
                processedString = ProcessObjectReplacementNumericStatValueToString(numericStatType, value);
                break;
        }

        if (value > 0f) processedString = PLUS_CHARACTER + processedString; //Add plus character to values over 0

        return processedString;
    }

    public static string ProcessObjectValueNumericStatValueToString(NumericStatType numericStatType, float value)
    {
        switch (numericStatType)
        {
            case NumericStatType.MaxHealth:
            case NumericStatType.MaxShield:
            case NumericStatType.Armor:
            case NumericStatType.HealthRegen:
            case NumericStatType.ShieldRegen:
            case NumericStatType.AttackDamage:
                return ProcessCurrentValueToSimpleInt(value);
            case NumericStatType.MovementSpeed:
            case NumericStatType.AttackSpeed:
            default:
                return ProcessCurrentValueToSimpleFloat(value, 2);
            case NumericStatType.AttackCritChance:
            case NumericStatType.AttackCritDamageMultiplier:
            case NumericStatType.DodgeChance:
            case NumericStatType.CooldownReduction:
            case NumericStatType.Lifesteal:
            case NumericStatType.Gold:
                return ProcessCurrentValueToPercentage(value, 2);
        }
    }

    public static string ProcessObjectPercentageNumericStatValueToString(NumericStatType numericStatType, float value)
    {
        switch (numericStatType)
        {
            case NumericStatType.MaxHealth:
            case NumericStatType.MaxShield:
            case NumericStatType.Armor:
            case NumericStatType.HealthRegen:
            case NumericStatType.ShieldRegen:
            case NumericStatType.AttackDamage:
            case NumericStatType.MovementSpeed:
            case NumericStatType.AttackSpeed:
            case NumericStatType.AttackCritChance:
            case NumericStatType.AttackCritDamageMultiplier:
            case NumericStatType.DodgeChance:
            case NumericStatType.CooldownReduction:
            case NumericStatType.Lifesteal:
            case NumericStatType.Gold:
            default:
                return ProcessCurrentValueToPercentage(value, 2);
        }
    }

    public static string ProcessObjectReplacementNumericStatValueToString(NumericStatType numericStatType, float value)
    {
        switch (numericStatType)
        {
            case NumericStatType.MaxHealth:
            case NumericStatType.MaxShield:
            case NumericStatType.Armor:
            case NumericStatType.HealthRegen:
            case NumericStatType.ShieldRegen:
            case NumericStatType.AttackDamage:
                return ProcessCurrentValueToSimpleInt(value);
            case NumericStatType.MovementSpeed:
            case NumericStatType.AttackSpeed:
            default:
                return ProcessCurrentValueToSimpleFloat(value, 2);
            case NumericStatType.AttackCritChance:
            case NumericStatType.AttackCritDamageMultiplier:
            case NumericStatType.DodgeChance:
            case NumericStatType.CooldownReduction:
            case NumericStatType.Lifesteal:
            case NumericStatType.Gold:
                return ProcessCurrentValueToPercentage(value, 2);
        }
    }
    #endregion


    public static string MapInventoryObjectRarityType(InventoryObjectSO inventoryObjectSO)
    {
        switch (inventoryObjectSO.GetInventoryObjectType())
        {
            case InventoryObjectType.Object:
            default:
                switch (inventoryObjectSO.objectRarity)
                {
                    case Rarity.Common:
                    default:
                        return COMMON_OBJECT_TEXT;
                    case Rarity.Uncommon:
                        return UNCOMMON_OBJECT_TEXT;
                    case Rarity.Rare:
                        return RARE_OBJECT_TEXT;
                    case Rarity.Epic:
                        return EPIC_OBJECT_TEXT;
                    case Rarity.Legendary:
                        return LEGENDARY_OBJECT_TEXT;
                }

            case InventoryObjectType.Treat:
                switch (inventoryObjectSO.objectRarity)
                {
                    case Rarity.Common:
                    default:
                        return COMMON_TREAT_TEXT;
                    case Rarity.Uncommon:
                        return UNCOMMON_TREAT_TEXT;
                    case Rarity.Rare:
                        return RARE_TREAT_TEXT;
                    case Rarity.Epic:
                        return EPIC_TREAT_TEXT;
                    case Rarity.Legendary:
                        return LEGENDARY_TREAT_TEXT;
                }
        }
    }
    public static string MapNumericStatType(NumericStatType numericStatType)
    {
        switch (numericStatType)
        {
            case NumericStatType.MaxHealth:
            default:
                return MAX_HEALTH_STAT;
            case NumericStatType.MaxShield:
                return MAX_SHIELD_STAT;
            case NumericStatType.Armor:
                return ARMOR_STAT;
            case NumericStatType.HealthRegen:
                return HEALTH_REGEN_STAT;
            case NumericStatType.ShieldRegen:
                return SHIELD_REGEN_STAT;
            case NumericStatType.MovementSpeed:
                return MOVEMENT_SPEED_STAT;
            case NumericStatType.AttackDamage:
                return ATTACK_DAMAGE_STAT;
            case NumericStatType.AttackSpeed:
                return ATTACK_SPEED_STAT;
            case NumericStatType.AttackCritChance:
                return ATTACK_CRIT_CHANCE_STAT;
            case NumericStatType.AttackCritDamageMultiplier:
                return ATTACK_CRIT_DAMAGE_MULTIPLIER_STAT;
            case NumericStatType.DodgeChance:
                return DODGE_CHANCE_STAT;
            case NumericStatType.CooldownReduction:
                return COOLDOWN_REDUCTION_STAT;
            case NumericStatType.Lifesteal:
                return LIFESTEAL_STAT;
            case NumericStatType.Gold:
                return GOLD_STAT;
        }
    }
}
