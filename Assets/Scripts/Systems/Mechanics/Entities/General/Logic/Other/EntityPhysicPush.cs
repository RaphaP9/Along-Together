using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPhysicPush : MonoBehaviour, IDisplacement
{
    [Header("Comonents")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private EntityHealth entityHealth;
    [Space]
    [SerializeField] protected List<Component> pushImmunerComponents;

    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float pushResistanceFactor;

    [Header("Runtime Filled")]
    [SerializeField] private PhysicPushData currentPushData;
    [Space]
    [SerializeField] private Vector2 currentPushDirection;

    private const float END_PUSH_THRESHOLD = 0.1f;
    public bool IsPushing { get; private set; }

    protected List<IPushImmuner> pushImmuners;

    private void OnEnable()
    {
        entityHealth.OnEntityDeath += EntityHealth_OnEntityDeath;
    }

    private void OnDisable()
    {
        entityHealth.OnEntityDeath -= EntityHealth_OnEntityDeath;
    }

    protected virtual void Awake()
    {
        GetPushImmunerInterfaces();
    }

    private void GetPushImmunerInterfaces() => pushImmuners = GeneralUtilities.TryGetGenericsFromComponents<IPushImmuner>(pushImmunerComponents);

    #region Push From Point
    public void PushEnemyFromPoint(Vector2 point, PhysicPushData pushData)
    {
        if (!CanPush()) return;

        StopAllCoroutines();
        StartCoroutine(PushEnemyFromPointCoroutine(point, pushData));
    }

    private IEnumerator PushEnemyFromPointCoroutine(Vector2 point, PhysicPushData pushData)
    {
        IsPushing = true;
        SetCurrentPushData(pushData);

        yield return null; //Let other scripts update their IDisplacement stuff

        currentPushDirection = CalculatePushDirection(point);

        Vector2 pushVector = currentPushDirection * pushData.pushForce;
        _rigidbody2D.velocity = pushVector;

        while(_rigidbody2D.velocity.magnitude > END_PUSH_THRESHOLD)
        {
            _rigidbody2D.AddForce(-pushVector* pushResistanceFactor);
            yield return null;
        }

        StopRigidbody();
        ClearCurrentPushData();

        IsPushing = false;
    }
    #endregion

    #region Push From Direction

    public void PushEnemyFromDirection(Vector2 direction, PhysicPushData pushData)
    {
        if (!CanPush()) return;

        StopAllCoroutines();
        StartCoroutine(PushEnemyFromDirectionCoroutine(direction, pushData));
    }

    private IEnumerator PushEnemyFromDirectionCoroutine(Vector2 direction, PhysicPushData pushData)
    {
        IsPushing = true;
        SetCurrentPushData(pushData);

        yield return null; //Let other scripts update their IDisplacement stuff (to stop movement, etc)

        Vector2 pushVector = direction.normalized * pushData.pushForce;
        _rigidbody2D.velocity = pushVector;

        while (_rigidbody2D.velocity.magnitude <= END_PUSH_THRESHOLD)
        {
            _rigidbody2D.AddForce(-pushVector * pushResistanceFactor);
            yield return null;
        }

        StopRigidbody();
        ClearCurrentPushData();

        IsPushing = false;
    }

    #endregion

    private void SuddenEndPush()
    {
        StopAllCoroutines();
        StopRigidbody();

        IsPushing = false;
    }

    private void StopRigidbody() => _rigidbody2D.velocity = Vector2.zero;

    private Vector2 CalculatePushDirection(Vector2 pushOrigin)
    {
        Vector2 pushDirection = GeneralUtilities.TransformPositionVector2(transform) - pushOrigin;
        pushDirection.Normalize();
        return pushDirection;
    }

    public bool IsDisplacing() => IsPushing;

    private bool CanPush()
    {
        if (!entityHealth.IsAlive()) return false;

        foreach (IPushImmuner pushImmuner in pushImmuners)
        {
            if (pushImmuner.IsPushImmuning()) return false;
        }

        return true;
    }

    private void SetCurrentPushData(PhysicPushData pushData) => currentPushData = pushData;
    private void ClearCurrentPushData() => currentPushData = null;

    private void EntityHealth_OnEntityDeath(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        SuddenEndPush();
    }
}
