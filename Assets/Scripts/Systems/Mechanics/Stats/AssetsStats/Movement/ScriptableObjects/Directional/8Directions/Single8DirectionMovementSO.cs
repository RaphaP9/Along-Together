using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Single8DirectionMovementSO", menuName = "ScriptableObjects/Movement/Directional/8Direction/Single8DirectionMovement")]
public class Single8DirectionMovement : MovementTypeSO
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

        movementAvailableCells = BoardUtilities.GetAvailableMovementCellsByDirections(currentPosition, board, directions, 1, JumpObstructions());
        return movementAvailableCells;
    }

    public override MovementType GetMovementType() => MovementType.Single8Directions;

    public override bool JumpObstructions() => false; //Can be true or false, cellDistance is 1 so it does not matter
}
