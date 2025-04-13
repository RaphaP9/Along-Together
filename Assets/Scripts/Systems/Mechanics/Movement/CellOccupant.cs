using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellOccupant : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private Cell currentCell;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public static event EventHandler<OnCellEventArgs> OnCellInitialized;
    public static event EventHandler<OnCellEventArgs> OnCellSet;
    public static event EventHandler<OnCellEventArgs> OnCellCleared;

    public Cell CurrentCell => currentCell;

    public class OnCellEventArgs : EventArgs
    {
        public CellOccupant cellOccupant;

        public Cell previousCell;
        public Cell newCell;
    }

    private void Start()
    {
        ClearCell();
        InitializeCellByPosition();
        SnapTransformToCellOccupantHolder();
    }

    private void InitializeCellByPosition()
    {
        Vector2Int intPosition = BoardUtilities.GetIntPositionDueToWorldPosition(transform);
        Cell cell = Board.Instance.GetCellWithSpecificCoordinate(intPosition);

        if(cell == null)
        {
            if (debug) Debug.Log($"Could not find cell with position {intPosition}. Will not initialize cell.");
            return;
        }

        if (!cell.CanBeOccupied())
        {
            if(debug) Debug.Log($"Cell with position {cell.Position} is already occupied. Will not initialze cell.");
            return;
        }

        SetCell(cell);
        OnCellInitialized?.Invoke(this, new OnCellEventArgs { cellOccupant = this, previousCell = null, newCell = cell});
    }

    private void ClearCell()
    {
        if(currentCell == null)
        {
            if (debug) Debug.Log($"Current cell is already null. Cell cleareance will be ignored.");
            return;
        }

        Cell previousCell = currentCell;

        currentCell = null;
        OnCellCleared?.Invoke(this, new OnCellEventArgs {cellOccupant = this, previousCell = previousCell, newCell = null });
    }

    public void SetCell(Cell cell)
    {
        if(cell == null)
        {
            if (debug) Debug.Log($"Cell is null. Will not initialize cell.");
            return;
        }

        if (!cell.CanBeOccupied())
        {
            if (debug) Debug.Log($"Cell with position {cell.Position} is already occupied. Will not set cell.");
            return;
        }

        Cell previousCell = currentCell;
        currentCell = cell;

        OnCellSet?.Invoke(this, new OnCellEventArgs { cellOccupant = this, previousCell = previousCell, newCell = currentCell });
    }

    private void SnapTransformToCellOccupantHolder()
    {
        if (currentCell == null) return;

        transform.position = currentCell.CellOccupantHolder.position;
    }
}
