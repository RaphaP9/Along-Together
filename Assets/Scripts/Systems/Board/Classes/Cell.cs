using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform cellOccupantHolder;

    [Header("Runtime Filled")]
    [SerializeField] private Vector2Int position;
    [SerializeField] private CellOccupant cellOccupant;

    public Transform CellOccupantHolder => cellOccupantHolder;
    public Vector2Int Position => position;
    public CellOccupant CellOccupant => cellOccupant;

    private void OnEnable()
    {
        CellOccupant.OnCellInitialized += CellOccupant_OnCellInitialized;
        CellOccupant.OnCellSet += CellOccupant_OnCellSet;
        CellOccupant.OnCellCleared += CellOccupant_OnCellCleared;
    }

    private void OnDisable()
    {
        CellOccupant.OnCellInitialized -= CellOccupant_OnCellInitialized;
        CellOccupant.OnCellSet -= CellOccupant_OnCellSet;
        CellOccupant.OnCellCleared -= CellOccupant_OnCellCleared;
    }

    private void Awake()
    {
        SetPositionDueToWorldPosition();
    }

    private void SetPositionDueToWorldPosition()
    {
        Vector2Int intPosition = BoardUtilities.GetIntPositionDueToWorldPosition(transform);
        position = intPosition;
    }

    private void SetOccuppant(CellOccupant cellOccupant) => this.cellOccupant = cellOccupant;
    private void ClearOccupant() => cellOccupant = null;

    public virtual bool CanBeOccupied() => cellOccupant == null;
    public virtual bool CanBeStepped()
    {
        return cellOccupant == null;
    }

    #region CellOccupantHandler Subscriptions
    private void CellOccupant_OnCellInitialized(object sender, CellOccupant.OnCellEventArgs e)
    {
        if (e.newCell != this) return;

        SetOccuppant(e.cellOccupant);
    }

    private void CellOccupant_OnCellSet(object sender, CellOccupant.OnCellEventArgs e)
    {
        if(e.previousCell == this)
        {
            ClearOccupant();
        }

        if(e.newCell == this)
        {
            SetOccuppant(e.cellOccupant);
        }
    }

    private void CellOccupant_OnCellCleared(object sender, CellOccupant.OnCellEventArgs e)
    {
        if (e.previousCell != this) return;

        ClearOccupant();
    }
    #endregion
}
