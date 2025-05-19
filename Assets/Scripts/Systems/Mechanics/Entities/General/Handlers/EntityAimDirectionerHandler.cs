using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityAimDirectionerHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected EntityHealth entityHealth;
    [Space]
    [SerializeField] protected Transform refferenceAimPoint;

    [Header("RuntimeFilled")]
    [SerializeField] protected Vector2 aimDirection;
    [SerializeField] protected float aimAngle;
    [Space]
    [SerializeField] private Vector2 refferencedAimDirection;
    [SerializeField] protected float refferencedAimAngle;

    public Vector2 AimDirection => aimDirection;
    public float AimAngle => aimAngle;

    public Vector2 RefferencedlAimDirection => refferencedAimDirection;
    public float RefferencedlAimAngle => refferencedAimAngle;

    protected virtual void Update()
    {
        HandleAim();
    }

    protected virtual void HandleAim()
    {
        if (!CanAim()) return;
        UpdateAim();
        UpdateRefferencedAim();
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

    protected void UpdateRefferencedAim()
    {
        if (refferenceAimPoint == null) return;

        refferencedAimDirection = CalculateRefferencedAimDirection();
        refferencedAimAngle = CalculateRefferencedAimAngle();
    }

    protected abstract Vector2 CalculateAimDirection();
    protected abstract float CalculateAimAngle();

    protected abstract Vector2 CalculateRefferencedAimDirection();
    protected abstract float CalculateRefferencedAimAngle();

    private void UpdateRotation(float aimAngle) => transform.rotation = Quaternion.Euler(0, 0, aimAngle);
    public bool IsAimingRight() => aimDirection.x >= 0;

}
