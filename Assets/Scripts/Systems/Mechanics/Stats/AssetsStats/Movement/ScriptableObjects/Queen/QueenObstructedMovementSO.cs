using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QueenObstructedMovementSO", menuName = "ScriptableObjects/Movement/Queen/QueenObstructedMovement", order = 1)]
public class QueenObstructedMovementSO : MovementTypeSO
{
    public override HashSet<Cell> GetMovementAvailableCells(Vector2Int currentPosition, Board board)
    {
        HashSet<Cell> movementAvailableCells = new HashSet<Cell>();

        HashSet<Vector2Int> directions = new HashSet<Vector2Int>();
        directions.Add(BoardUtilities.Up);
        directions.Add(BoardUtilities.Down);
        directions.Add(BoardUtilities.Left);
        directions.Add(BoardUtilities.Right);
        directions.Add(BoardUtilities.UpRightDiagonal);
        directions.Add(BoardUtilities.DownRightDiagonal);
        directions.Add(BoardUtilities.UpLeftDiagonal);
        directions.Add(BoardUtilities.DownLeftDiagonal);

        movementAvailableCells = BoardUtilities.GetAvailableMovementCellsByDirectionsUnlimited(currentPosition, board, directions, JumpObstructions());
        return movementAvailableCells;
    }

    public override MovementType GetMovementType() => MovementType.QueenObstructed;

    public override bool JumpObstructions() => false;
}