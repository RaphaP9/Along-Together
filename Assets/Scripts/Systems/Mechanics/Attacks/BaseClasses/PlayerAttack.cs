using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    [Header("Attack Components")]
    [SerializeField] protected CharacterIdentifier characterIdentifier;
    [SerializeField] protected PlayerAimDirectionerHandler aimDirectionerHandler;

    [Header("Attack Settings")]
    [SerializeField] protected AttackTriggerType attackTriggerType;
    [SerializeField] protected LayerMask attackLayermask;
    [Space]
    [SerializeField] protected List<Transform> attackInterruptionAbilitiesTransforms;

    [Header("Attack Runtime Filled")]
    [SerializeField] protected int attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackCritChance;
    [SerializeField] protected float attackCritDamageMultiplier;

    [Header("Debug")]
    [SerializeField] protected bool debug;

    public AttackTriggerType AttackTriggerType_ => attackTriggerType;
    public enum AttackTriggerType {Automatic, SemiAutomatic}

    protected float attackTimer = 0f;
    private List<IAttackInterruptionAbility> attackInterruptionAbilities;

    public event EventHandler<OnPlayerAttackEventArgs> OnPlayerAttack;
    public static event EventHandler<OnPlayerAttackEventArgs> OnAnyPlayerAttack;

    public class OnPlayerAttackEventArgs : EventArgs
    {
        public PlayerAttack playerAttack;
        public bool isCrit;

        public int attackDamage;
        public float attackSpeed;
        public float attackCritChance;
        public float attackCritDamageMultiplier;
    }

    protected virtual void OnEnable()
    {
        AttackDamageStatResolver.OnAttackDamageResolverUpdated += AttackDamageStatResolver_OnAttackDamageResolverUpdated;
        AttackSpeedStatResolver.OnAttackSpeedResolverUpdated += AttackSpeedStatResolver_OnAttackSpeedResolverUpdated;
        AttackCritChanceStatResolver.OnAttackCritChanceResolverUpdated += AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated;
        AttackCritDamageMultiplierStatResolver.OnAttackCritDamageMultiplierResolverUpdated += AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        AttackDamageStatResolver.OnAttackDamageResolverUpdated -= AttackDamageStatResolver_OnAttackDamageResolverUpdated;
        AttackSpeedStatResolver.OnAttackSpeedResolverUpdated -= AttackSpeedStatResolver_OnAttackSpeedResolverUpdated;
        AttackCritChanceStatResolver.OnAttackCritChanceResolverUpdated -= AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated;
        AttackCritDamageMultiplierStatResolver.OnAttackCritDamageMultiplierResolverUpdated -= AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated;
    }

    private void Awake()
    {
        GetAttackInterruptionAbilitiesInterfaces();
    }

    private void GetAttackInterruptionAbilitiesInterfaces()
    {
        attackInterruptionAbilities = GeneralUtilities.TryGetGenericsFromTransforms<IAttackInterruptionAbility>(attackInterruptionAbilitiesTransforms);
    }

    protected virtual void Start()
    {
        Initialize();
        ResetAttackTimer();
    }

    protected virtual void Initialize()
    {
        attackDamage = CalculateAttackDamage();
        attackSpeed = CalculateAttackSpeed();
        attackCritChance = CalculateAttackCritChance();
        attackCritDamageMultiplier = CalculateAttackCritDamageMultiplier();
    }

    protected virtual void Update()
    {
        HandleAttack();
        HandleAttackCooldown();
    }

    private void HandleAttack()
    {
        if (!GetAttackInput()) return;
        if (!CanAttack()) return;

        Attack();
        MaxTimer();
    }

    private void HandleAttackCooldown()
    {
        if (attackTimer < 0) return;

        attackTimer -= Time.deltaTime;
    }

    protected abstract void Attack();

    protected virtual void OnPlayerAttackMethod(bool isCrit, int attackDamage)
    {
        OnPlayerAttack?.Invoke(this, new OnPlayerAttackEventArgs { playerAttack = this, isCrit = isCrit, attackDamage = attackDamage, attackSpeed = attackSpeed, attackCritChance = attackCritChance, attackCritDamageMultiplier = attackCritDamageMultiplier });
        OnAnyPlayerAttack?.Invoke(this, new OnPlayerAttackEventArgs { playerAttack = this, isCrit = isCrit, attackDamage = attackDamage, attackSpeed = attackSpeed, attackCritChance = attackCritChance, attackCritDamageMultiplier = attackCritDamageMultiplier });
    }


    protected virtual bool CanAttack()
    {
        if (AttackOnCooldown()) return false;

        foreach (IAttackInterruptionAbility attackInterruptionAbility in attackInterruptionAbilities)
        {
            if (attackInterruptionAbility.IsInterruptingAttack() && attackInterruptionAbility.CanInterruptAttack()) return false;
        }

        return true;
    }

    private bool AttackOnCooldown() => attackTimer > 0f;
    private void ResetAttackTimer() => attackTimer = 0f;
    private void MaxTimer() => attackTimer = 1f / attackSpeed;

    #region Stat Calculations
    private int CalculateAttackDamage() => AttackDamageStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseAttackDamage);
    private float CalculateAttackSpeed() => AttackSpeedStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackSpeed);
    private float CalculateAttackCritChance() => AttackCritChanceStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackCritChance);
    private float CalculateAttackCritDamageMultiplier() => AttackCritDamageMultiplierStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackCritDamageMultiplier);

    private void RecalculateAttackDamage()
    {
        attackDamage = CalculateAttackDamage();
    }

    private void RecalculateAttackSpeed()
    {
        attackSpeed = CalculateAttackSpeed();
    }

    private void RecalculateAttackCritChance()
    {
        attackCritChance = CalculateAttackCritChance();
    }

    private void RecalculateAttackCritDamageMultiplier()
    {
        attackCritDamageMultiplier = CalculateAttackCritDamageMultiplier();
    }
    #endregion

    #region AttackTriggerType-Input Assignation
    protected bool GetSemiAutomaticInputAttack() => AttackInput.Instance.GetAttackDown();
    protected bool GetAutomaticInputAttack() => AttackInput.Instance.GetAttackHold();

    protected bool GetAttackInput()
    {
        switch (attackTriggerType)
        {
            case AttackTriggerType.SemiAutomatic:
            default:
                return GetSemiAutomaticInputAttack();
            case AttackTriggerType.Automatic:
                return GetAutomaticInputAttack();
        }
    }
    #endregion

    #region Subscriptions
    private void AttackDamageStatResolver_OnAttackDamageResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateAttackDamage();
    }
    private void AttackSpeedStatResolver_OnAttackSpeedResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateAttackSpeed();
    }
    private void AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateAttackCritChance();
    }
    private void AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateAttackCritDamageMultiplier();
    }
    #endregion
}
