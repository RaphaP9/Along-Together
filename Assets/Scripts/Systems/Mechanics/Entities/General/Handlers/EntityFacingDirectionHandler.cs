using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFacingDirectionHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private EntityAimDirectionerHandler entityAimDirectioner;
    [SerializeField] private EntityHealth entityHealth;

    [Header("Components")]
    [SerializeField] private List<Component> facingInterruptionComponents;

    [Header("Settings")]
    [SerializeField] private FacingType facingType;
    [SerializeField] private Vector2Int startingFacingDirection;
    [SerializeField, Range(0.5f, 10f)] private float minimumRigidbodyVelocity;

    [Header("Runtime Filled")]
    [SerializeField] private Vector2 currentRawFacingDirection;
    [SerializeField] private Vector2Int currentFacingDirection;
    [Space]
    [SerializeField] private bool isOverridingFacingDirection;
    [SerializeField] private Vector2 overridenDirection;

    public Vector2Int CurrentFacingDirection => currentFacingDirection;
    public Vector2 CurrentRawFacingDirection => currentRawFacingDirection;
    public bool IsOverridingFacingDirection => isOverridingFacingDirection;

    private List<IFacingInterruption> facingInterruptions;

    private enum FacingType { Rigidbody, Aim }
    public Vector2 OverridenDirection => overridenDirection;

    private void Awake()
    {
        GetFacingInterruptionInterfaces();
    }

    private void Start()
    {
        RecalculateFacingDirections(startingFacingDirection);
    }

    public void HandleFacing() //Called By the corresponding entity StateHandler: PlayerStateHandler, MeleeEnemyStateHandler, etc
    {
        HandleDirectionOverride();
        HandleFacingDirection();
    }

    private void GetFacingInterruptionInterfaces() => facingInterruptions = GeneralUtilities.TryGetGenericsFromComponents<IFacingInterruption>(facingInterruptionComponents);


    #region FacingDirectionOverride

    private void HandleDirectionOverride()
    {
        if (!CanFace()) return;

        foreach (IFacingInterruption facingInterruptionAbility in facingInterruptions)
        {
            if (facingInterruptionAbility.IsInterruptingFacing())
            {
                isOverridingFacingDirection = true;
                overridenDirection = facingInterruptionAbility.GetFacingDirection();

                RecalculateFacingDirections(overridenDirection);
                return;
            }
        }

        isOverridingFacingDirection = false;
        overridenDirection = Vector2.zero;
    }

    #endregion

    #region Facing Direction Logic
    private void HandleFacingDirection()
    {
        if (!CanFace()) return;
        if (isOverridingFacingDirection) return;

        switch (facingType)
        {
            case FacingType.Rigidbody:
            default:
                HandleFacingDirectionByRigidbody();
                break;
            case FacingType.Aim:
                HandleFacingDirectionByAim();
                break;

        }
    }

    private void HandleFacingDirectionByRigidbody()
    {
        if (_rigidbody2D.velocity.magnitude < minimumRigidbodyVelocity) return;

        Vector2 direction = _rigidbody2D.velocity.normalized;

        if (currentRawFacingDirection != direction)
        {
            RecalculateFacingDirections(direction);
        }
    }

    private void HandleFacingDirectionByAim()
    {
        Vector2 direction = entityAimDirectioner.AimDirection;

        if (currentRawFacingDirection != direction)
        {
            RecalculateFacingDirections(direction);
        }
    }

    private void RecalculateFacingDirections(Vector2 direction)
    {
        SetCurrentRawFacingDirection(direction);
        RecalculateCurrentFacingDirection();
    }

    private void RecalculateCurrentFacingDirection() => currentFacingDirection = GeneralUtilities.ClampVector2To8Direction(currentRawFacingDirection);
    private void SetCurrentRawFacingDirection(Vector2 rawFacingDirection) => currentRawFacingDirection = rawFacingDirection;
    private void SetCurrentFacingDirection(Vector2Int facingDirection) => currentFacingDirection = facingDirection;

    public bool IsFacingRight() => currentRawFacingDirection.x >= 0;

    #endregion

    protected virtual bool CanFace()
    {
        if (!entityHealth.IsAlive()) return false;

        return true;
    }
}
