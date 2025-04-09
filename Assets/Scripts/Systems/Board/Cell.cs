using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform cellCenter;

    [Header("Runtime Filled")]
    [SerializeField] private Vector2Int position;
    [SerializeField] private Transform occupant;

    public Transform CellCenter => cellCenter;
    public Vector2Int Position => position;

    private void Awake()
    {
        SetPositionDueToWorldPosition();
    }

    private void SetPositionDueToWorldPosition()
    {
        Vector2 rawPosition = GeneralUtilities.SupressZComponent(transform.position);
        Vector2Int intPosition = GeneralUtilities.Vector2ToVector2Int(rawPosition);

        position = intPosition;
    }

    private void SetOccuppant(Transform occupant) => this.occupant = occupant;
    private void ClearOccupant() => occupant = null;

    public virtual bool CanBeOccupied() => occupant != null;
    public virtual bool CanBeStepped()
    {
        return occupant == null;
    }
}
