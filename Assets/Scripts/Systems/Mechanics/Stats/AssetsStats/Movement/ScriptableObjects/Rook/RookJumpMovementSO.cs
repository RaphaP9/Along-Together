using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RookJumpMovementSO", menuName = "ScriptableObjects/Movement/Rook/RookJumpMovement", order = 2)]
public class RookJumpMovementSO : MovementTypeSO
{
    public override HashSet<Cell> GetMovementAvailableCells(Vector2Int currentPosition, Board board)
    {
        HashSet<Cell> movementAvailableCells = new HashSet<Cell>();

        HashSet<Vector2Int> directions = new HashSet<Vector2Int>();
        directions.Add(BoardUtilities.Up);
        directions.Add(BoardUtilities.Down);
        directions.Add(BoardUtilities.Left);
        directions.Add(BoardUtilities.Right);

        movementAvailableCells = BoardUtilities.GetAvailableMovementCellsByDirectionsUnlimited(currentPosition, board, directions, JumpObstructions());
        return movementAvailableCells;
    }

    public override MovementType GetMovementType() => MovementType.RookJump;

    public override bool JumpObstructions() => true;
}