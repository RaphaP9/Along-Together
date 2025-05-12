using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileAttack : PlayerAttack
{
    [Header("Player Projectile Attack Components")]
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform projectilePrefab;

    [Header("Player Projectile Attack Settings")]
    [SerializeField] protected ProjectileDamageType projectileDamageType;
    [SerializeField, Range(0f,3f)] protected float projectileAreaRadius;
    [Space]
    [SerializeField] protected LayerMask projectileImpactLayerMask;
    [Space]
    [SerializeField, Range(0.1f,50f)] protected float projectileSpeed;
    [SerializeField, Range(0.5f,15f)] protected float projectileLifespan;
    [Space]
    [SerializeField, Range(0f, 15f)] protected float projectileDispersionAngle;

    protected override void Attack()
    {
        bool isCrit = MechanicsUtilities.EvaluateCritAttack(attackCritChance);
        int damage = isCrit ? MechanicsUtilities.CalculateCritDamage(attackDamage, attackCritDamageMultiplier) : attackDamage;

        Vector2 shootDirection = attackDirectionerHandler.AimDirection;
        Vector2 position = firePoint.position;

        InstantiateProjectile(characterIdentifier.CharacterSO, projectilePrefab, position, shootDirection, damage, isCrit, projectileSpeed, projectileLifespan, projectileDamageType, projectileAreaRadius, attackLayermask, projectileImpactLayerMask);

        OnPlayerAttackMethod(isCrit, damage);
    }

    protected void InstantiateProjectile(IDamageSourceSO damageSource, Transform projectilePrefab, Vector2 position, Vector2 shootDirection, int damage, bool isCrit, float speed, float lifespan, ProjectileDamageType projectileDamageType , float areaRadius, LayerMask targetLayerMask, LayerMask impactLayerMask)
    {
        Vector3 vector3Position = GeneralUtilities.Vector2ToVector3(position);
        Transform instantiatedProjectile = Instantiate(projectilePrefab, vector3Position, Quaternion.identity);

        ProjectileHandler projectileHandler = instantiatedProjectile.GetComponent<ProjectileHandler>();

        if (projectileHandler == null)
        {
            if (debug) Debug.Log("Instantiated projectile does not contain a ProjectileHandler component. Projectile set will be ignored.");
            return;
        }

        Vector2 processedShootDirection = MechanicsUtilities.DeviateShootDirection(shootDirection, projectileDispersionAngle);

        projectileHandler.SetProjectile(damageSource, processedShootDirection, damage, isCrit, speed, lifespan, projectileDamageType, areaRadius, targetLayerMask, impactLayerMask);
    }
}
