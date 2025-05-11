using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : PlayerAttack
{
    [Header("Player Melee Attack Components")]
    [SerializeField] protected List<Transform> attackPoints;

    [Header("Player Melee Attack Settings")]
    [SerializeField, Range(0.1f, 3f)] protected float attackAreaRadius;


    protected override void Attack()
    {
        bool isCrit = MechanicsUtilities.EvaluateCritAttack(attackCritChance);
        List<Vector2> positions = GeneralUtilities.TransformPositionVector2List(attackPoints);

        int damage = isCrit ? MechanicsUtilities.CalculateCritDamage(attackDamage, attackCritDamageMultiplier) : attackDamage;

        MechanicsUtilities.DealDamageInArea(positions, attackAreaRadius, damage, isCrit, attackLayermask, characterIdentifier.CharacterSO, new List<Transform> {transform});

        OnPlayerAttackMethod(isCrit, damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (Transform attackPoint in attackPoints)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackAreaRadius);
        }
    }
}
