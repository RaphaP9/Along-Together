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

    public Vector2 AimDirection => aimDirection;
    public float AimAngle => aimAngle;

    private void Update()
    {
        HandleAim();
    }

    private void HandleAim()
    {
        if (!CanAim()) return;

        aimDirection = CalculateAimDirection();
        aimAngle = CalculateAimAngle();

        UpdateRotation(aimAngle);

    }

    protected abstract Vector2 CalculateAimDirection();
    protected abstract float CalculateAimAngle();

    protected virtual bool CanAim()
    {
        if (!entityHealth.IsAlive()) return false;

        return true;
    }

    private void UpdateRotation(float aimAngle)
    {
        transform.rotation = Quaternion.Euler(0, 0, aimAngle);
    }

    public bool IsAimingRight()
    {
        if (aimDirection.x >= 0) return true;
        return false;
    }
}
