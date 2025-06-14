using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legato : ActiveAbility, IDodger, IAttackInterruption
{
    [Header("Specific Settings")]
    [SerializeField] private LayerMask pushLayerMask;

    private LegatoSO LegatoSO => AbilitySO as LegatoSO;

    public event EventHandler OnLegatoStart;
    public event EventHandler OnLegatoCompleted;

    public static event EventHandler OnAnyLegatoStart;
    public static event EventHandler OnAnyLegatoCompleted;

    public float LegatoTimer { get; private set; }
    public float Duration => GetDuration();

    private bool isCurrentlyActive;


    #region Interface Methods
    public bool IsDodging() => isCurrentlyActive;
    public bool IsInterruptingAttack() => isCurrentlyActive;
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

        OnAnyLegatoStart?.Invoke(this, EventArgs.Empty);
        OnLegatoStart?.Invoke(this, EventArgs.Empty);

        Debug.Log("LegatoStart");

        ResetTimer();

        while (LegatoTimer < GetDuration())
        {
            LegatoTimer += Time.deltaTime;
            yield return null;
        }

        isCurrentlyActive = false;

        MechanicsUtilities.PushEntitiesFromPoint(GeneralUtilities.TransformPositionVector2(transform), LegatoSO.pushData, pushLayerMask);

        OnAnyLegatoCompleted?.Invoke(this, EventArgs.Empty);
        OnLegatoCompleted?.Invoke(this, EventArgs.Empty);

        Debug.Log("LegatoEnd");
    }

    private float GetDuration()
    {
        return LegatoSO.flyDuration;
    }

    private void ResetTimer() => LegatoTimer = 0;
}
