using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAimDirectionerHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerRelativeHandler playerRelativeHandler;
    [SerializeField] private EnemyHealth enemyHealth;

    [Header("RuntimeFilled")]
    [SerializeField] private Vector2 aimDirection;
    [SerializeField] private float aimAngle;

    private void Update()
    {
        HandleAim();
    }

    private void HandleAim()
    {
        if (!CanAim()) return;

        aimDirection = playerRelativeHandler.DirectionToPlayer;
        aimAngle = playerRelativeHandler.AngleToPlayer;

        UpdateRotation(aimAngle);

    }

    private bool CanAim()
    {
        if (!enemyHealth.IsAlive()) return false;

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
