using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyAimDirectionerHandler : EnemyAimDirectionerHandler
{
    [Header("Projectile Enemy Components")]
    [SerializeField] private ProjectileEnemyAttack projectileEnemyAttack;

    private void OnEnable()
    {
        projectileEnemyAttack.OnEnemyAttackCompleted += ProjectileEnemyAttack_OnEnemyAttackCompleted;
    }

    private void OnDisable()
    {
        projectileEnemyAttack.OnEnemyAttackCompleted -= ProjectileEnemyAttack_OnEnemyAttackCompleted;
    }

    protected override bool CanAim()
    {
        if (!base.CanAim()) return false;
        if (projectileEnemyAttack.OnAttackExecution()) return false; //Can't aim while attacking

        return true;
    }

    #region Subscriptions
    private void ProjectileEnemyAttack_OnEnemyAttackCompleted(object sender, EnemyAttack.OnEnemyAttackCompletedEventArgs e)
    {
        UpdateAim(); //UpdateAim if attack finished
    }
    #endregion
}
