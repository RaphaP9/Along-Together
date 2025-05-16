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
    [SerializeField] private Vector2Int currentFacingDirection;
    [Space]
    [SerializeField] private bool isOverridingFacingDirection;
    [SerializeField] private Vector2 overridenDirection;

    public Vector2Int CurrentFacingDirection => currentFacingDirection;
    private List<IFacingInterruption> facingInterruptions;

    private enum FacingType { Rigidbody, Aim }
    public bool IsOverridingFacingDirection => isOverridingFacingDirection;
    public Vector2 OverridenDirection => overridenDirection;

    private void Awake()
    {
        GetFacingInterruptionInterfaces();
    }

    private void Start()
    {
        SetCurrentFacingDirection(startingFacingDirection);
    }

    private void Update()
    {
        HandleDirectionOverride();
        HandleFacingDirection();
    }

    private void GetFacingInterruptionInterfaces() => facingInterruptions = GeneralUtilities.TryGetGenericsFromComponents<IFacingInterruption>(facingInterruptionComponents);


    #region FacingDirectionOverride

    private void HandleDirectionOverride()
    {
        if (!entityHealth.IsAlive()) return;

        foreach (IFacingInterruption facingInterruptionAbility in facingInterruptions)
        {
            if (facingInterruptionAbility.IsInterruptingFacing())
            {
                isOverridingFacingDirection = true;
                overridenDirection = facingInterruptionAbility.GetFacingDirection();

                Vector2Int direction = GeneralUtilities.ClampVector2To8Direction(overridenDirection);
                SetCurrentFacingDirection(direction);
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
        if (!entityHealth.IsAlive()) return;
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

        Vector2Int direction = GeneralUtilities.ClampVector2To8Direction(_rigidbody2D.velocity);

        if (currentFacingDirection != direction)
        {
            SetCurrentFacingDirection(direction);
        }
    }

    private void HandleFacingDirectionByAim()
    {
        Vector2Int direction = GeneralUtilities.ClampVector2To8Direction(entityAimDirectioner.AimDirection);

        if (currentFacingDirection != direction)
        {
            SetCurrentFacingDirection(direction);
        }
    }

    private void SetCurrentFacingDirection(Vector2Int facingDirection) => currentFacingDirection = facingDirection;

    public bool IsFacingRight()
    {
        switch (facingType)
        {
            case FacingType.Rigidbody:
            default:
                return _rigidbody2D.velocity.x >= 0f;
            case FacingType.Aim:
                return entityAimDirectioner.AimDirection.x >= 0;
        }
    }
    #endregion
}
