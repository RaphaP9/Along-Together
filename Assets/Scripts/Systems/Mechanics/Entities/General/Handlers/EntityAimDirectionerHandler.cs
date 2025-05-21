using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityAimDirectionerHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected EntityHealth entityHealth;

    [Header("RuntimeFilled")]
    [SerializeField] protected Vector2 aimDirection;
    [SerializeField] protected float aimAngle;
    [Space]
    [SerializeField] private Vector2 refferencedAimDirection;
    [SerializeField] protected float refferencedAimAngle;

    public Vector2 AimDirection => aimDirection;
    public float AimAngle => aimAngle;

    public virtual void HandleAim() //Called By the corresponding entity StateHandler: PlayerStateHandler, MeleeEnemyStateHandler, etc
    {
        if (!CanAim()) return;
        UpdateAim();
    }

    protected virtual bool CanAim()
    {
        if (!entityHealth.IsAlive()) return false;

        return true;
    }

    protected void UpdateAim()
    {
        aimDirection = CalculateAimDirection();
        aimAngle = CalculateAimAngle();

        UpdateRotation(aimAngle);
    }

    protected abstract Vector2 CalculateAimDirection();
    protected abstract float CalculateAimAngle();

    private void UpdateRotation(float aimAngle) => transform.rotation = Quaternion.Euler(0, 0, aimAngle);
    public bool IsAimingRight() => aimDirection.x >= 0;

}
