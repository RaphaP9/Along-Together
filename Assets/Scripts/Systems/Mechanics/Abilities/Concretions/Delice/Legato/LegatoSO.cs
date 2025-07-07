using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LegatoSO", menuName = "ScriptableObjects/Abilities/Delice/Legato")]
public class LegatoSO : ActiveAbilitySO, IDamageSource
{
    [Header("Specific Settings")]
    [Range(0f, 1f)] public float flyStartDuration;
    [Range(1f, 5f)] public float flyDuration;
    [Range(0f, 1f)] public float flyEndDuration;
    [Space]
    [ColorUsage(true, true)] public Color damageColor;
    [Space]
    [Range(1f, 8f)] public float actionRadius;
    [Range(5, 10)] public int landDamage;
    [Range(1f, 100f)] public float pushForce;

    #region Damage Source Methods
    public DamageSourceClassification GetDamageSourceClassification() => DamageSourceClassification.Character;
    public Color GetDamageSourceColor() => damageColor;
    public string GetDamageSourceDescription() => description;
    public string GetDamageSourceName() => abilityName;
    public Sprite GetDamageSourceSprite() => sprite;
    #endregion
}
