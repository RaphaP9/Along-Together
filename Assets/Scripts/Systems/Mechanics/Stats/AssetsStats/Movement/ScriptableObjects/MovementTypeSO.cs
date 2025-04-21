using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementTypeSO : AssetStatSO
{
    public abstract MovementType GetMovementType();

    public abstract HashSet<Cell> GetMovementAvailableCells(Vector2Int currentPosition, Board board, int movementDistance, int obstructionJumps);

    public override AssetStatType GetAssetStatType() => AssetStatType.MovementType;
}
