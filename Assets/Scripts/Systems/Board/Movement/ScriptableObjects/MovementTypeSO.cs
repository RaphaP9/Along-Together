using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementTypeSO : ScriptableObject
{
    public abstract MovementType GetMovementType();

    public abstract HashSet<Cell> GetMovementAvailableCells(Vector2Int currentPosition, Board board);

    public abstract bool JumpObstructions(); //Jump or not Jump, move similar to Knight in chess, where you can jump other pieces on the way
}
