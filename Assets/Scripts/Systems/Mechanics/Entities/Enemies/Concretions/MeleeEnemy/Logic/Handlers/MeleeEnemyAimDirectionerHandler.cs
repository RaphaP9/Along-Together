using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAimDirectionerHandler : EnemyAimDirectionerHandler
{
    [Header("Melee Enemy Components")]
    [SerializeField] private MeleeEnemyAttack meleeEnemyAttack;

    private void OnEnable()
    {
        meleeEnemyAttack.OnEnemyAttackCompleted += MeleeEnemyAttack_OnEnemyAttackCompleted;
    }

    private void OnDisable()
    {
        meleeEnemyAttack.OnEnemyAttackCompleted -= MeleeEnemyAttack_OnEnemyAttackCompleted;
    }

    protected override bool CanAim()
    {
        if (!base.CanAim()) return false;
        if (meleeEnemyAttack.OnAttackExecution()) return false; //Can't aim while attacking

        return true;
    }

    #region Subscriptions
    private void MeleeEnemyAttack_OnEnemyAttackCompleted(object sender, EnemyAttack.OnEnemyAttackCompletedEventArgs e)
    {
        UpdateAim(); //UpdateAim if attack finished
    }
    #endregion
}
