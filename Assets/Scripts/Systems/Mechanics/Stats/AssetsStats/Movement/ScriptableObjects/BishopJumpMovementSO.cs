using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BishopJumpMovementSO", menuName = "ScriptableObjects/Movement/Bishop/BishopJumpMovement", order = 2)]
public class BishopJumpMovementSO : MovementTypeSO
{
    public override HashSet<Cell> GetMovementAvailableCells(Vector2Int currentPosition, Board board, int movementDistance, int obstructionJumps)
    {
        HashSet<Cell> movementAvailableCells = new HashSet<Cell>();

        HashSet<Vector2Int> directions = new HashSet<Vector2Int>();
        directions.Add(BoardUtilities.UpRightDiagonal);
        directions.Add(BoardUtilities.DownRightDiagonal);
        directions.Add(BoardUtilities.UpLeftDiagonal);
        directions.Add(BoardUtilities.DownLeftDiagonal);

        movementAvailableCells = BoardUtilities.GetAvailableMovementCellsByDirections(currentPosition, board, directions, movementDistance, obstructionJumps);
        return movementAvailableCells;
    }

    public override MovementType GetMovementType() => MovementType.Bishop;
}