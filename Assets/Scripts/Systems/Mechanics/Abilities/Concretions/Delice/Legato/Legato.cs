using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legato : ActiveAbility, IDodger, IFacingInterruption, IMovementInterruption
{
    [Header("Specific Settings")]
    [SerializeField] private LayerMask pushLayerMask;

    private LegatoSO LegatoSO => AbilitySO as LegatoSO;

    public event EventHandler OnLegatoStarting;
    public event EventHandler OnLegatoStart;
    public event EventHandler OnLegatoEnding;
    public event EventHandler OnLegatoCompleted;

    public static event EventHandler OnAnyLegatoStarting;
    public static event EventHandler OnAnyLegatoStart;
    public static event EventHandler OnAnyLegatoEnding;
    public static event EventHandler OnAnyLegatoCompleted;

    public float LegatoTimer { get; private set; }
    public float Duration => GetDuration();

    private bool isCurrentlyActive = false;
    private bool isStarting = false;
    private bool isEnding = false;

    #region Interface Methods
    public bool IsDodging() => isCurrentlyActive;
    /////////////////////////////////////////////////////////////////////
    public bool IsInterruptingFacing() => isStarting || isEnding;
    public bool OverrideFacingDirection() => true;
    public Vector2 GetFacingDirection() => new Vector2(0f, -1f); //FacingDown
    /////////////////////////////////////////////////////////////////////
    public bool IsInterruptingMovement() => false; // isStarting || isEnding;
    public bool StopMovementOnInterruption() => true;
    ////////////////////////////////////////////////////////////////////
    public override bool IsInterruptingAttack() => isCurrentlyActive;
    /////////////////////////////////////////////////////////////////////
    public override bool IsInterruptingAbility() => isCurrentlyActive;
    #endregion

    #region Logic Methods
    protected override void HandleFixedUpdateLogic() { }
    protected override void HandleUpdateLogic() { }
    #endregion

    protected override void OnAbilityCastMethod()
    {
        base.OnAbilityCastMethod();
        HandleLegatoTrigger();
    }

    private void HandleLegatoTrigger()
    {
        if (isCurrentlyActive)
        {
            ResetTimer();
            return;
        }

        StartCoroutine(LegatoCoroutine());
    }

    private IEnumerator LegatoCoroutine()
    {
        isCurrentlyActive = true;
        ResetTimer();

        OnAnyLegatoStarting?.Invoke(this, EventArgs.Empty);
        OnLegatoStarting?.Invoke(this, EventArgs.Empty);

        isStarting = true;

        yield return new WaitForSeconds(LegatoSO.flyStartDuration);

        isStarting = false;

        OnAnyLegatoStart?.Invoke(this, EventArgs.Empty);
        OnLegatoStart?.Invoke(this, EventArgs.Empty);

        while (LegatoTimer < GetDuration())
        {
            LegatoTimer += Time.deltaTime;
            yield return null;
        }

        OnAnyLegatoEnding?.Invoke(this, EventArgs.Empty);
        OnLegatoEnding?.Invoke(this, EventArgs.Empty);

        isEnding = true;

        yield return new WaitForSeconds(LegatoSO.flyEndDuration);

        isEnding = false;
        isCurrentlyActive = false;

        MechanicsUtilities.PushEntitiesFromPoint(GeneralUtilities.TransformPositionVector2(transform), LegatoSO.pushData, pushLayerMask);

        OnAnyLegatoCompleted?.Invoke(this, EventArgs.Empty);
        OnLegatoCompleted?.Invoke(this, EventArgs.Empty);
    }

    private float GetDuration() //Maybe duration is not exactly FlyDuration (Longer duration due to Ability Level,etc)
    {
        return LegatoSO.flyDuration;
    }

    private void ResetTimer() => LegatoTimer = 0;
}
