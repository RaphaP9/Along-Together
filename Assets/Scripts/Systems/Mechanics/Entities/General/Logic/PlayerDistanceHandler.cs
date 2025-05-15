using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDistanceHandler : MonoBehaviour
{
    [Header("RuntimeFilled")]
    [SerializeField] private float distanceToPlayer;
    public float DistanceToPlayer => distanceToPlayer;

    private void Update()
    {
        HandleDistanceToPlayer();
    }

    private void HandleDistanceToPlayer()
    {
        if (PlayerTransformRegister.Instance.PlayerTransform == null) return;

        distanceToPlayer = Vector3.Distance(transform.position, PlayerTransformRegister.Instance.PlayerTransform.position);
    }
}
