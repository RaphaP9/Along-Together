using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ImmovableMovementSO", menuName = "ScriptableObjects/Movement/ImmovableMovement")]
public class ImmovableMovementSO : MovementTypeSO
{
    public override HashSet<Cell> GetMovementAvailableCells(Vector2Int currentPosition, Board board)
    {
        HashSet<Cell> movementAvailableCells = new HashSet<Cell>(); //Empty List

        return movementAvailableCells;
    }

    public override MovementType GetMovementType() => MovementType.Immovable;

    public override bool JumpObstructions() => false; //Not really needed
}
