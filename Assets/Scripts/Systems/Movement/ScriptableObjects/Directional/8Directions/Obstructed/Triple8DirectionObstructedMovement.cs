using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Triple8DirectionObstructedMovementSO", menuName = "ScriptableObjects/Movement/Directional/8Direction/Obstructed/Triple8DirectionObstructedMovement" , order = 2)]
public class Triple8DirectionObstructedMovement : MovementTypeSO
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

        movementAvailableCells = BoardUtilities.GetAvailableMovementCellsByDirections(currentPosition, board, directions, 3, JumpObstructions());
        return movementAvailableCells;
    }

    public override MovementType GetMovementType() => MovementType.Triple8DirectionsObstructed;

    public override bool JumpObstructions() => false;
}
