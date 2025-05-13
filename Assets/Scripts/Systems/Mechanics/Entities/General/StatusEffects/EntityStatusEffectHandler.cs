using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatusEffectHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<EntityStatusEffect> statusEffects;

    [Header("Components")]
    [SerializeField] private EntityHealth entityHealth;
    [SerializeField] private EntityMovement entityMovement;


}

public class EntityStatusEffectGroup
{
    public string originGUID;
    public EntityStatusEffect statusEffect;
    public float duration;
    public float value;
    [Space]
    public float tickTime; //OptionalFor Poison, Burn, etc
}
