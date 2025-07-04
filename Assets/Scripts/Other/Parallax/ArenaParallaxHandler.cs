using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaParallaxHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform arenaCenterRefference;

    [Header("Settings")]
    [SerializeField] private Vector2 displacementMultipliers;

    private const float DISTANCE_THRESHOLD_TO_UPDATE = 50f;

    private void Update()
    {
        HandleParallax();
    }

    private void HandleParallax()
    {
        if (PlayerTransformRegister.Instance == null) return;
        if (PlayerTransformRegister.Instance.PlayerTransform == null) return;

        Vector2 playerOffsetFromCenter = GeneralUtilities.SupressZComponent(PlayerTransformRegister.Instance.PlayerTransform.position - arenaCenterRefference.position);

        if (playerOffsetFromCenter.magnitude > DISTANCE_THRESHOLD_TO_UPDATE) return;
       
        transform.localPosition = new Vector3(playerOffsetFromCenter.x * displacementMultipliers.x, playerOffsetFromCenter.y * displacementMultipliers.y, transform.position.z);
    }
}
