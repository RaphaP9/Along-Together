using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : EntityMovement
{
    [Header("Components")]
    [SerializeField] private PlayerRelativeHandler playerRelativeHandler;

    public void MoveAwayFromPlayerDirection() => MoveTowardsDirection(-playerRelativeHandler.DirectionToPlayer);
    public void MoveTowardsPlayerDirection() => MoveTowardsDirection(playerRelativeHandler.DirectionToPlayer);
    public void MoveTowardsPlayerPosition() => MoveTowardsPosition(playerRelativeHandler.PlayerPosition);
    public void StopOnCurrentPosition() => Stop();

    protected void Stop()
    {
        _rigidbody2D.velocity = Vector2.zero;
    }

    protected void MoveTowardsDirection(Vector2 direction)
    {
        Vector2 normalizedDirection = direction.normalized;
        _rigidbody2D.velocity = normalizedDirection * GetMovementSpeedValue();
    }

    protected void MoveTowardsPosition(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - GeneralUtilities.TransformPositionVector2(transform);
        direction.Normalize();
        _rigidbody2D.velocity = direction * GetMovementSpeedValue();
    }
}
