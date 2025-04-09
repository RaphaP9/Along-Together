using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Triple4DirectionObstructedMovementSO", menuName = "ScriptableObjects/Movement/Directional/4Direction/Obstructed/Triple4DirectionObstructedMovement", order = 2)]
public class Triple4DirectionObstructedMovement : MovementTypeSO
{
    public override HashSet<Cell> GetMovementAvailableCells(Vector2Int currentPosition, Board board)
    {
        HashSet<Cell> movementAvailableCells = new HashSet<Cell>();

        HashSet<Vector2Int> directions = new HashSet<Vector2Int>();
        directions.Add(BoardUtilities.Up);
        directions.Add(BoardUtilities.Down);
        directions.Add(BoardUtilities.Left);
        directions.Add(BoardUtilities.Right);

        movementAvailableCells = BoardUtilities.GetAvailableMovementCellsByDirections(currentPosition, board, directions, 3, JumpObstructions());
        return movementAvailableCells;
    }

    public override MovementType GetMovementType() => MovementType.Triple4DirectionsObstructed;

    public override bool JumpObstructions() => false;
}
