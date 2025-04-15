using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CellOccupant cellOccupant;

    [Header("States")]
    [SerializeField] private CellMovementState cellMovementState;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private enum CellMovementState {NotMoving, StartingMovement, Moving, EndingMovement }

    public static event EventHandler<OnCellEventArgs> OnStartingMoveToCell;
    public static event EventHandler<OnCellEventArgs> OnStartMoveToCell;
    public static event EventHandler<OnCellEventArgs> OnEndingMoveToCell;
    public static event EventHandler<OnCellEventArgs> OnEndMoveToCell;

    private const float TIME_TO_START_MOVEMENT = 1f;
    private const float TIME_TO_END_MOVEMENT = 1f;
    private const float SMOOTH_MOVEMENT_FACTOR = 10f;
    private const float END_MOVEMENT_DISTANCE_THRESHOLD = 0.01f;

    public class OnCellEventArgs : EventArgs
    {
        public CellMovement cellMovement;

        public Cell previousCell;
        public Cell newCell;
    }

    private void Start()
    {
        SetCellMovementState(CellMovementState.NotMoving);
    }

    public void MoveToCell(Cell cell)
    {
        if (cell == null)
        {
            if (debug) Debug.Log("Can not move to null cell. Movement will be ignored");
            return;
        }

        if (cell == cellOccupant.CurrentCell)
        {
            if (debug) Debug.Log("Can not move to current cell. Movement will be ignored");
            return;
        }

        cellOccupant.SetCell(cell);

        StopAllCoroutines();
        StartCoroutine(MoveToCellCoroutine(cell));
    }

    private IEnumerator MoveToCellCoroutine(Cell cell)
    {
        Cell originCell = cellOccupant.CurrentCell;

        SetCellMovementState(CellMovementState.StartingMovement);
        OnStartingMoveToCell?.Invoke(this, new OnCellEventArgs {cellMovement = this, previousCell = originCell, newCell = cell });
        
        yield return new WaitForSeconds(TIME_TO_START_MOVEMENT);

        SetCellMovementState(CellMovementState.Moving);
        OnStartMoveToCell?.Invoke(this, new OnCellEventArgs {cellMovement = this, previousCell = originCell, newCell = cell });

        float distanceToCell = CalculateDistanceToCell(cell);

        while(distanceToCell > END_MOVEMENT_DISTANCE_THRESHOLD)
        {
            transform.position = Vector3.Lerp(transform.position, cell.CellOccupantHolder.position, SMOOTH_MOVEMENT_FACTOR * Time.deltaTime);
            distanceToCell = CalculateDistanceToCell(cell);

            yield return null;
        }

        SetPositionToCellOccupantPosition(cell);

        SetCellMovementState(CellMovementState.EndingMovement);
        OnEndingMoveToCell?.Invoke(this, new OnCellEventArgs { cellMovement = this, previousCell = originCell, newCell = cell });

        yield return new WaitForSeconds(TIME_TO_END_MOVEMENT);

        SetCellMovementState(CellMovementState.NotMoving);
        OnEndMoveToCell?.Invoke(this, new OnCellEventArgs { cellMovement = this, previousCell = originCell, newCell = cell });
    }

    private float CalculateDistanceToCell(Cell cell) => Vector3.Distance(transform.position, cell.CellOccupantHolder.position);
    private void SetPositionToCellOccupantPosition(Cell cell) => transform.position = cell.CellOccupantHolder.position;

    private void SetCellMovementState(CellMovementState state) => cellMovementState = state;
}
