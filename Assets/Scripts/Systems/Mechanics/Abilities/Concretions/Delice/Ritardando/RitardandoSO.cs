using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RitardandoSO", menuName = "ScriptableObjects/Abilities/Delice/Ritardando")]
public class RitardandoSO : ActiveAbilitySO, IDamageSource
{
    [Header("Specific Settings")]
    [Range(1, 10)] public int damage;
    [ColorUsage(true, true)] public Color damageColor;
    [Space]
    public SlowStatusEffect slowStatusEffect;
    [Space]
    [Range(0f, 1f)] public float performanceTime;

    #region Damage Source Methods
    public DamageSourceClassification GetDamageSourceClassification() => DamageSourceClassification.Character;
    public Color GetDamageSourceColor() => damageColor;
    public string GetDamageSourceDescription() => description;
    public string GetDamageSourceName() => abilityName;
    public Sprite GetDamageSourceSprite() => sprite;
    #endregion
}
