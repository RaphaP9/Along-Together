using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : EntityMovement
{
    public static PlayerMovement Instance {  get; private set; }

    [Header("Enabler")]
    [SerializeField] private bool movementEnabled;

    [Header("Components")]
    [SerializeField] private CharacterIdentifier characterIdentifier;
    [SerializeField] private PlayerHealth playerHealth;
    [Space]
    [SerializeField] private CheckWall checkWall;
    //[SerializeField] private PlayerDash playerDash;

    #region Events

    public static event EventHandler<OnEntityStatsEventArgs> OnPlayerStatsInitialized;
    public event EventHandler<OnEntityStatsEventArgs> OnThisPlayerStatsInitialized;

    public static event EventHandler<OnEntityStatsEventArgs> OnPlayerStatsUpdated;
    public event EventHandler<OnEntityStatsEventArgs> OnThisPlayerStatsUpdated;

    //

    public static event EventHandler<OnEntityStatsEventArgs> OnPlayerMovementSpeedChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnThisPlayerMovementSpeedChanged;
    #endregion

    private Rigidbody2D _rigidbody2D;

    public Vector2 DirectionInput => MovementInput.Instance.GetMovementInputNormalized();

    public float DesiredSpeed { get; private set; }
    public float SmoothCurrentSpeed { get; private set; }

    public Vector2 SmoothDirectionInput { get; private set; }
    public Vector2 LastNonZeroInput  { get; private set; }
    public Vector2 FinalMoveValue { get; private set; }

    public Vector2 ScaledMovementVector { get; private set; }
    public bool MovementEnabled => movementEnabled;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        SetSingleton();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PlayerMovement instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    protected override float CalculateMovementSpeed() => MovementSpeedStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.movementSpeed);

    #region Logic
    private void HandleMovement()
    {
        if (!movementEnabled) return;

        CalculateDesiredSpeed();
        SmoothSpeed();

        CalculateLastNonZeroDirection();
        SmoothDirection();

        CalculateFinalMovement();
        ScaleFinalMovement();
    }

    private void CalculateDesiredSpeed()
    {
        DesiredSpeed = CanMove() ? CalculateMovementSpeed() : 0f;
    }

    private bool CanMove()
    {
        if (DirectionInput == Vector2.zero) return false;
        if (checkWall.HitWall) return false;
        if (!playerHealth.IsAlive()) return false;

        return true;
    }

    private void SmoothSpeed()
    {
        SmoothCurrentSpeed = Mathf.Lerp(SmoothCurrentSpeed, DesiredSpeed, Time.deltaTime * smoothVelocityFactor);
    }

    private void CalculateLastNonZeroDirection() => LastNonZeroInput = DirectionInput != Vector2.zero ? DirectionInput : LastNonZeroInput;
    private void SmoothDirection() => SmoothDirectionInput = Vector2.Lerp(SmoothDirectionInput, DirectionInput, Time.deltaTime * smoothDirectionFactor);

    private void CalculateFinalMovement()
    {
        Vector2 finalInput = SmoothDirectionInput * SmoothCurrentSpeed;

        Vector2 roundedFinalInput;
        roundedFinalInput.x = Mathf.Abs(finalInput.x) < 0.01f ? 0f : finalInput.x;
        roundedFinalInput.y = Mathf.Abs(finalInput.y) < 0.01f ? 0f : finalInput.y;

        FinalMoveValue = roundedFinalInput;
    }

    private void ScaleFinalMovement()
    {
        ScaledMovementVector = MechanicsUtilities.ScaleVector2ToPerspective(FinalMoveValue);
    }

    private void ApplyMovement()
    {
        //if (playerDash.IsDashing) return;

        _rigidbody2D.velocity = new Vector2(ScaledMovementVector.x, ScaledMovementVector.y);
    }
    #endregion

    #region Virtual Event Methods

    protected override void OnEntityStatsInitializedMethod()
    {
        base.OnEntityStatsInitializedMethod();

        OnThisPlayerStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { movementSpeed = CalculateMovementSpeed() });
        OnPlayerStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { movementSpeed = CalculateMovementSpeed() });
    }

    protected override void OnEntityStatsUpdatedMethod()
    {
        base.OnEntityStatsUpdatedMethod();

        OnThisPlayerStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { movementSpeed = CalculateMovementSpeed() });
        OnPlayerStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { movementSpeed = CalculateMovementSpeed() });
    }

    protected override void OnEntityMovementSpeedChangedMethod()
    {
        base.OnEntityMovementSpeedChangedMethod();

        OnThisPlayerMovementSpeedChanged?.Invoke(this, new OnEntityStatsEventArgs { movementSpeed = CalculateMovementSpeed() });
        OnPlayerMovementSpeedChanged?.Invoke(this, new OnEntityStatsEventArgs { movementSpeed = CalculateMovementSpeed() });
    }
    #endregion
}
