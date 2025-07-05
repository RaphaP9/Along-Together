using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : EntityMovement
{
    [Header("Components")]
    [SerializeField] private PlayerRelativeHandler playerRelativeHandler;

    [Header("Enemy Avoidance")]
    [SerializeField] private LayerMask avoidanceLayerMask;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField, Range (0f,2f)] private float avoidanceDetectionRadius;
    [SerializeField, Range (0f,1f)] private float avoidanceWeight;

    private const int MAX_AVOIDANCE_COUNT = 5;
    private const float AVOID_THRESHOLD_DISTANCE = 0.25f;

    public override void Stop()
    {
        _rigidbody2D.velocity = Vector2.zero;
    }

    protected void MoveTowardsDirection(Vector2 direction)
    {
        if (!CanApplyMovement()) return;

        Vector2 normalizedDirection = direction.normalized;
        _rigidbody2D.velocity = normalizedDirection * GetMovementSpeedValue();
    }

    protected void MoveTowardsPosition(Vector2 targetPosition)
    {
        if (!CanApplyMovement()) return;

        Vector2 direction = targetPosition - GeneralUtilities.TransformPositionVector2(transform);
        direction.Normalize();
        _rigidbody2D.velocity = direction * GetMovementSpeedValue();
    }

    public void MoveAwayFromPlayerDirection() => MoveTowardsDirection(-playerRelativeHandler.DirectionToPlayer);

    public void MoveTowardsPlayerDirection()
    {
        if (!CanApplyMovement()) return;

        Vector2 avoidanceDirection = CalculateAvoidanceVector(avoidanceDetectionRadius, avoidanceLayerMask, avoidanceWeight);
        Vector2 finalDirection = (playerRelativeHandler.DirectionToPlayer + avoidanceDirection).normalized;

        _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, finalDirection * GetMovementSpeedValue(), Time.deltaTime * smoothVelocityFactor);
    }

    public void MoveTowardsPlayerPosition() => MoveTowardsPosition(playerRelativeHandler.PlayerPosition);
    public void StopOnCurrentPosition() => Stop();

    private Vector2 CalculateAvoidanceVector(float radius, LayerMask avoidanceLayerMask, float weight)
    {
        Collider2D[] results = new Collider2D[MAX_AVOIDANCE_COUNT];
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, radius, results, avoidanceLayerMask);

        Vector2 separation = Vector2.zero;
        int validCount = 0;

        for (int i = 0; i < count; i++)
        {
            Collider2D col = results[i];
            if (col == null || col == _collider2D) continue;

            Vector2 directionFromAvoidee = GeneralUtilities.SupressZComponent(transform.position - col.transform.position);
            float distance = directionFromAvoidee.magnitude;

            if (distance > AVOID_THRESHOLD_DISTANCE)
            {
                separation += directionFromAvoidee.normalized / distance;
                validCount++;
            }
        }

        if (validCount > 0) separation = separation.normalized * weight;
        return separation;
    }
}
