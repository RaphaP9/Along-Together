using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemyAimDirectionerHandler : EnemyAimDirectionerHandler
{
    [Header("Melee Enemy Components")]
    [SerializeField] private KamikazeEnemyExplosion kamikazeEnemyExplosion;

    protected override bool CanAim()
    {
        if (!base.CanAim()) return false;
        if (kamikazeEnemyExplosion.OnExplosionExecution()) return false; //Can't aim while exploding

        return true;
    }
}
