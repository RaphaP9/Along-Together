using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRotationHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectileHandler projectileHandler;
    [SerializeField] private Transform transformToRotate;

    private void OnEnable()
    {
        projectileHandler.OnProjectileSet += ProjectileHandler_OnProjectileSet;
    }

    private void OnDisable()
    {
        projectileHandler.OnProjectileSet -= ProjectileHandler_OnProjectileSet;
    }

    private void RotateTowardsDirection(Vector2 direction)
    {
        GeneralUtilities.RotateTransformTowardsVector2(transformToRotate, direction);
    }

    private void ProjectileHandler_OnProjectileSet(object sender, ProjectileHandler.OnProjectileEventArgs e)
    {
        RotateTowardsDirection(e.direction);
    }
}
