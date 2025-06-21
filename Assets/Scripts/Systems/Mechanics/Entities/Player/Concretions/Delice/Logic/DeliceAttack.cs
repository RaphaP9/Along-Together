using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliceAttack : PlayerProjectileAttack
{
    [Header("Staccato Settings")]
    [SerializeField] private Staccato staccato;

    public event EventHandler OnDeliceRegularAttack;
    public static event EventHandler OnAnyDeliceRegularAttack;

    public event EventHandler OnDeliceBurstAttack;
    public static event EventHandler OnAnyDeliceBurstAttack;

    protected override void Attack()
    {
        if (staccato.IsCurrentlyActive)
        {
            StaccatoAttack();
        }
        else
        {
            base.Attack();
            OnDeliceRegularAttack?.Invoke(this, EventArgs.Empty);
            OnAnyDeliceRegularAttack?.Invoke(this, EventArgs.Empty);
        }
    }

    private void StaccatoAttack()
    {
        StartCoroutine(StaccatoAttackCoroutine());
    }

    private IEnumerator StaccatoAttackCoroutine()
    {
        for(int i=0; i < 3; i++)
        {
            ShootProjectile(projectilePrefab);
            OnDeliceBurstAttack?.Invoke(this, EventArgs.Empty);
            OnAnyDeliceBurstAttack?.Invoke(this, EventArgs.Empty);

            yield return new WaitForSeconds(staccato.GetBurstInterval());
        }
    }
}
