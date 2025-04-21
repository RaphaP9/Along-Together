using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ImmovableMovementSO", menuName = "ScriptableObjects/Movement/ImmovableMovement")]
public class ImmovableMovementSO : MovementTypeSO
{
    public override HashSet<Cell> GetMovementAvailableCells(Vector2Int currentPosition, Board board, int movementDistance, int obstructionJumps)
    {
        HashSet<Cell> movementAvailableCells = new HashSet<Cell>(); //Empty List

        return movementAvailableCells;
    }

    public override MovementType GetMovementType() => MovementType.Immovable;
}
