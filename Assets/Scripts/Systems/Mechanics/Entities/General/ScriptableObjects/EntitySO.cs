using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntitySO : ScriptableObject
{
    [Header("Entity Identifiers")]
    public int id;
    public string entityName;
    [TextArea(3, 10)] public string description;
    public Sprite sprite;

    [Header("Entity Stats")]
    [Range(0, 20)] public int healthPoints;
    [Range(0, 20)] public int shieldPoints;
    [Range(0, 20)] public int armorPoints;
    [Space]
    [Range(0, 1)] public float dodgeChance;

    [Header("Movement Types")]
    public List<MovementTypeSO> movementTypes;
}
