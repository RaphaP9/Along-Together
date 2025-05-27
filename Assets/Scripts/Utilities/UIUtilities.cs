using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public static class UIUtilities
{
    #region Object Classification Consts 
    private const string COMMON_OBJECT_TEXT = "Treat Común";
    private const string UNCOMMON_OBJECT_TEXT = "Treat Poco Común";
    private const string RARE_OBJECT_TEXT = "Treat Raro";
    private const string EPIC_OBJECT_TEXT = "Treat Épico";
    private const string LEGENDARY_OBJECT_TEXT = "Treat Legendario";

    private const string COMMON_TREAT_TEXT = "Arma Común";
    private const string UNCOMMON_TREAT_TEXT = "Arma Poco Común";
    private const string RARE_TREAT_TEXT = "Arma Rara";
    private const string EPIC_TREAT_TEXT = "Arma Épica";
    private const string LEGENDARY_TREAT_TEXT = "Arma Legendaria";
    #endregion

    #region Numeric Stat Description Text
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

    public static void SetCanvasGroupAlpha(CanvasGroup canvasGroup, float alpha) => canvasGroup.alpha = alpha;
    public static void SetImageFillRatio(Image image, float fillRatio) => image.fillAmount = fillRatio;
    public static void SetImageColor(Image image, Color color) => image.color = color;
    public static void SetImagesColor(List<Image> images, Color color)
    {
        foreach (Image image in images)
        {
            SetImageColor(image, color);
        }
    }

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
