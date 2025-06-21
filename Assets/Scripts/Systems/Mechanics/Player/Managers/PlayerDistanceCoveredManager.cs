using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDistanceCoveredManager : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private float playerDistanceCovered;
    [SerializeField] private PlayerMovement playerMovement;

    public float PlayerDistanceCovered => playerDistanceCovered;

    private void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
    }

    private void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
    }

    private void Update()
    {
        HandleDistanceCoveredUpdate();
    }

    private void HandleDistanceCoveredUpdate()
    {
        if (playerMovement == null) return;

        playerDistanceCovered = playerMovement.DistanceCovered;
    }

    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        playerMovement = e.playerTransform.GetComponentInChildren<PlayerMovement>();
    }
}
