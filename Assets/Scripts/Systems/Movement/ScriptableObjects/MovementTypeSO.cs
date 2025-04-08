using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementTypeSO : ScriptableObject
{
    public abstract MovementType GetMovementType();

    public abstract List<Cell> GetMovementAvailableCells(Vector2Int currentPosition, Board board);
}
