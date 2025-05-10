using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : PlayerAttack
{
    protected override void Attack()
    {
        Debug.Log("Attacking!");
    }
}
