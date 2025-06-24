using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GoldMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [Header("FreeFall Settings")]
    [SerializeField] private LayerMask terrainLayerMask;
    [SerializeField, Range(2f,10f)] private float minImpulse;
    [SerializeField, Range(2f,10f)] private float maxImpulse;
    [SerializeField, Range(-360f, 360)] private float minVelAngle;
    [SerializeField, Range(-360f, 360)] private float maxVelAngle;
    [Space]
    [SerializeField, Range(0.2f, 2.5f)] private float minTimeToStop;
    [SerializeField, Range(0.2f, 2.5f)] private float maxTimeToStop;

    [Header("PlayerDetection Settings")]
    [SerializeField, Range(0.5f, 7f)] private float basePlayerDetectionRange;
    [SerializeField, Range(1f, 100f)] private float moveTowardsPlayerSmoothFactor;

    [Header("Collection By Round End Settings")]
    [SerializeField, Range(1f, 100f)] private float moveTowardsPlayerRoundEndSmoothFactor;

    [Header("Runtiime Filled")]
    [SerializeField] private float chosenAngle;
    [SerializeField] private Vector2 chosenDirection;

    private bool playerDetected = false;
    private bool collectionByRoundEnd = false;

    public static event EventHandler OnAnyPlayerDetected;
    public event EventHandler OnPlayerDetected;

    public static event EventHandler OnAnyCollectionByRoundEnd;
    public event EventHandler OnCollectionByRoundEnd;

    private void OnEnable()
    {
        GameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void Start()
    {
        ChooseRandomDirection();
        ThrowInChosenDirection();

        StartCoroutine(StopInTimeCoroutine());
    }

    private void Update()
    {
        HandlePlayerDetection();
        HandleMoveTowardsPlayer();
        HandleMoveTowardsPlayerRoundEnd();
    }

    private void ThrowInChosenDirection()
    {
        float impulse = UnityEngine.Random.Range(minImpulse, maxImpulse);


        _rigidbody2D.AddForce(impulse * chosenDirection, ForceMode2D.Impulse);
    }

    private void ChooseRandomDirection()
    {
        chosenAngle = UnityEngine.Random.Range(minVelAngle, maxVelAngle); 
        chosenDirection = GeneralUtilities.GetAngleDegreesVector2(chosenAngle);
    }

    private IEnumerator StopInTimeCoroutine()
    {
        float stopTime = UnityEngine.Random.Range(minTimeToStop, maxTimeToStop);

        yield return new WaitForSeconds(stopTime);

        StopMovement();
    }

    private void StopMovement()
    {
        _rigidbody2D.gravityScale = 0f;
        _rigidbody2D.velocity = Vector2.zero;
    }

    private void HandlePlayerDetection()
    {
        if (playerDetected) return;
        if (PlayerTransformRegister.Instance == null) return;
        if(PlayerTransformRegister.Instance.PlayerTransform == null) return;

        float detectionRange = basePlayerDetectionRange;

        if(Vector2.Distance(transform.position, PlayerTransformRegister.Instance.PlayerTransform.position)< basePlayerDetectionRange)
        {
            playerDetected = true;
            OnAnyPlayerDetected?.Invoke(this, EventArgs.Empty);
            OnPlayerDetected?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HandleMoveTowardsPlayer()
    {
        if (collectionByRoundEnd) return;
        if (!playerDetected) return;
        if (PlayerTransformRegister.Instance == null) return;
        if (PlayerTransformRegister.Instance.PlayerTransform == null) return;

        transform.position = Vector3.Lerp(transform.position, PlayerTransformRegister.Instance.PlayerTransform.position, moveTowardsPlayerSmoothFactor*Time.deltaTime);
    }

    private void HandleMoveTowardsPlayerRoundEnd()
    {
        if (!collectionByRoundEnd) return;
        if (PlayerTransformRegister.Instance == null) return;
        if (PlayerTransformRegister.Instance.PlayerTransform == null) return;

        transform.position = Vector3.Lerp(transform.position, PlayerTransformRegister.Instance.PlayerTransform.position, moveTowardsPlayerRoundEndSmoothFactor * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GeneralUtilities.CheckGameObjectInLayerMask(collision.gameObject, terrainLayerMask)) return;

        StopMovement();
    }

    private void GameManager_OnStateChanged(object sender, GameManager.OnStateChangeEventArgs e)
    {
        if (e.newState != GameManager.State.EndingCombat) return;

        collectionByRoundEnd = true;
        OnAnyCollectionByRoundEnd?.Invoke(this, EventArgs.Empty);
        OnCollectionByRoundEnd?.Invoke(this, EventArgs.Empty);
    }
}
