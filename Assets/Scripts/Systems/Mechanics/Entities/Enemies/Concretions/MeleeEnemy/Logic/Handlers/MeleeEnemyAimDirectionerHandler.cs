using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAimDirectionerHandler : EnemyAimDirectionerHandler
{
    [Header("Melee Enemy Components")]
    [SerializeField] private MeleeEnemyAttack meleeEnemyAttack;

    private void OnEnable()
    {
        meleeEnemyAttack.OnMeleeEnemyAttackCompleted += MeleeEnemyAttack_OnMeleeEnemyAttackCompleted;
    }

    private void OnDisable()
    {
        meleeEnemyAttack.OnMeleeEnemyAttackCompleted -= MeleeEnemyAttack_OnMeleeEnemyAttackCompleted;
    }

    protected override bool CanAim()
    {
        if (!base.CanAim()) return false;
        if (meleeEnemyAttack.OnAttackExecution()) return false; //Can't aim while attacking

        return true;
    }

    #region Subscriptions
    private void MeleeEnemyAttack_OnMeleeEnemyAttackCompleted(object sender, MeleeEnemyAttack.OnEnemyAttackEventArgs e)
    {
        UpdateAim(); //UpdateAim if attack finished
    }
    #endregion
}
