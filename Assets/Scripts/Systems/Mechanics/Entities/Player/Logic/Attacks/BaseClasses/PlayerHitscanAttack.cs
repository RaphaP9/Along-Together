using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHitscanAttack : PlayerAttack
{
    [Header("Player Hitscan Attack Components")]
    [SerializeField] protected Transform firePoint;

    [Header("Player Hitscan Attack Settings")]
    [SerializeField] protected LayerMask hitscanImpactLayerMask;
    [Space]
    [SerializeField, Range(10f, 100f)] protected float hitscanDistance;
    [SerializeField, Range(0f, 15f)] protected float hitscanDispersionAngle;

    public event EventHandler<OnPlayerHitscanRayShotEventArgs> OnPlayerHitscanRayShot;
    public static event EventHandler<OnPlayerHitscanRayShotEventArgs> OnAnyPlayerHitscanRayShot;

    public class OnPlayerHitscanRayShotEventArgs : EventArgs
    {
        public Vector2 originPoint;
        public Vector2 hitPoint;
    }

    protected override void Attack()
    {
        bool isCrit = MechanicsUtilities.EvaluateCritAttack(specificEntityStatsResolver.AttackCritChance);
        int damage = isCrit ? MechanicsUtilities.CalculateCritDamage(specificEntityStatsResolver.AttackDamage, specificEntityStatsResolver.AttackCritDamageMultiplier) : specificEntityStatsResolver.AttackDamage;

        Vector2 direction = aimDirectionerHandler.AimDirection;
        Vector2 processedDirection = MechanicsUtilities.DeviateShootDirection(direction, hitscanDispersionAngle);

        Vector2 position = firePoint.position;

        ShootHitscanRay(position, processedDirection, hitscanDistance, GeneralUtilities.CombineLayerMasks(new List<LayerMask> { attackLayermask, hitscanImpactLayerMask}), damage, isCrit);

        OnEntityAttackMethod(isCrit, damage);
    }

    protected virtual void ShootHitscanRay(Vector2 originPosition , Vector2 direction, float distance, LayerMask layerMask, int damage, bool isCrit)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(originPosition, direction, distance, layerMask);
        DamageData damageData = new DamageData { damage = damage, isCrit = isCrit, damageSource = characterIdentifier.CharacterSO, canBeDodged = true, canBeImmuned = true};

        Vector2 hitPoint = Vector2.zero;

        foreach(RaycastHit2D hit in hits)
        {
            if(GeneralUtilities.CheckGameObjectInLayerMask(hit.transform.gameObject, attackLayermask))
            {
                if (MechanicsUtilities.DealDamageToTransform(hit.transform, damageData))
                {
                    hitPoint = hit.point;
                    break;
                }

                continue;
            }

            if (GeneralUtilities.CheckGameObjectInLayerMask(hit.transform.gameObject, hitscanImpactLayerMask))
            {
                hitPoint = hit.point;
                break;
            }
        }

        OnPlayerHitscanRayShotMethod(originPosition, hitPoint);
    }

    protected void OnPlayerHitscanRayShotMethod(Vector2 originPosition, Vector2 hitPoint)
    {
        OnPlayerHitscanRayShot?.Invoke(this, new OnPlayerHitscanRayShotEventArgs { originPoint = originPosition, hitPoint = hitPoint });
        OnAnyPlayerHitscanRayShot?.Invoke(this, new OnPlayerHitscanRayShotEventArgs { originPoint = originPosition, hitPoint = hitPoint });
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(firePoint.position, firePoint.position + GeneralUtilities.Vector2ToVector3(aimDirectionerHandler.AimDirection) * hitscanDistance);
    }
}
