using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class Ritardando : ActiveAbility, IFacingInterruption
{
    [Header("Specific Settings")]
    [SerializeField] private LayerMask effectLayerMask; //For both damage & slow
    [Space]
    [SerializeField] private List<Transform> areaTransforms;
    [SerializeField, Range(0.5f, 1f)] private float areaRadius;

    private RitardandoSO RitardandoSO => AbilitySO as RitardandoSO;

    public event EventHandler OnRitardandoPerformanceStart;
    public event EventHandler OnRitardandoPerformanceEnd;

    public static event EventHandler OnAnyRitardandoPerformanceStart;
    public static event EventHandler OnAnyRitardandoPerformanceEnd;

    private bool isPerforming = false;

    #region Interface Methods
    public bool IsInterruptingFacing() => isPerforming;
    public Vector2 GetFacingDirection() => new Vector2(0f, -1f); //FacingDown
    public override bool IsInterruptingAttack() => isPerforming;
    public override bool IsInterruptingAbility() => isPerforming;
    #endregion

    #region Logic Methods
    protected override void HandleFixedUpdateLogic() { }
    protected override void HandleUpdateLogic() { }
    #endregion

    protected override void OnAbilityCastMethod()
    {
        base.OnAbilityCastMethod();
        HandleRitardandoTrigger();
    }

    private void HandleRitardandoTrigger()
    {
        StartCoroutine(RitardandoCoroutine());
    }

    private IEnumerator RitardandoCoroutine()
    {
        isPerforming = true;

        OnAnyRitardandoPerformanceStart?.Invoke(this, EventArgs.Empty);
        OnRitardandoPerformanceStart?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(RitardandoSO.performanceTime);

        DamageData damageData = new DamageData(RitardandoSO.damage, false, RitardandoSO, false, true, true, true);

        MechanicsUtilities.DealDamageInAreas(GeneralUtilities.TransformPositionVector2List(areaTransforms), areaRadius, damageData, effectLayerMask);
        MechanicsUtilities.SlowInAreas(GeneralUtilities.TransformPositionVector2List(areaTransforms), areaRadius, RitardandoSO.slowStatusEffect, effectLayerMask);

        isPerforming = false;

        OnAnyRitardandoPerformanceEnd?.Invoke(this, EventArgs.Empty);
        OnRitardandoPerformanceEnd?.Invoke(this, EventArgs.Empty);
    }
}
