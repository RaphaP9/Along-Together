using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KnightMovementSO", menuName = "ScriptableObjects/Movement/KnightMovement")]
public class KnightMovementSO : MovementTypeSO
{
    public override HashSet<Cell> GetMovementAvailableCells(Vector2Int currentPosition, Board board)
    {
        HashSet<Cell> movementAvailableCells = new HashSet<Cell>();

        HashSet<Vector2Int> relativePositions = new HashSet<Vector2Int>();
        relativePositions.Add(BoardUtilities.KnightA);
        relativePositions.Add(BoardUtilities.KnightB);
        relativePositions.Add(BoardUtilities.KnightD);
        relativePositions.Add(BoardUtilities.KnightE);
        relativePositions.Add(BoardUtilities.KnightF);
        relativePositions.Add(BoardUtilities.KnightG);
        relativePositions.Add(BoardUtilities.KnightH);

        movementAvailableCells = BoardUtilities.GetAvailableMovementCellsByRelativePositions(currentPosition, board, relativePositions);
        return movementAvailableCells;
    }

    public override MovementType GetMovementType() => MovementType.Knight;

    public override bool JumpObstructions() => true; //Not really needed
}