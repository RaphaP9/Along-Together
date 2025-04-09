using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class BoardUtilities
{
    public static readonly Vector2Int Up = new(0,1);
    public static readonly Vector2Int Down = new(0,-1);
    public static readonly Vector2Int Left = new(-1,0);
    public static readonly Vector2Int Right = new(1,0);

    public static readonly Vector2Int UpRightDiagonal = new(1, 1);
    public static readonly Vector2Int DownRightDiagonal = new(1, -1);
    public static readonly Vector2Int UpLeftDiagonal = new(-1, 1);
    public static readonly Vector2Int DownLeftDiagonal = new(-1, -1);

    public static HashSet<Cell> GetAvailableMovementCellsByDirections(Vector2Int currentPosition, Board board, HashSet<Vector2Int> directions, int cellDistance, bool obstructDirection)
    {
        HashSet<Cell> movementAvailableCells = new HashSet<Cell>();

        foreach (Vector2Int direction in directions)
        {
            HashSet<Cell> cells = GetAvailableMovementCellsByDirection(currentPosition, board, direction, cellDistance, obstructDirection);
            movementAvailableCells.AddRange(cells);
        }

        return movementAvailableCells;
    }

    public static HashSet<Cell> GetAvailableMovementCellsByDirection(Vector2Int currentPosition, Board board, Vector2Int direction, int cellDistance, bool jumpObstructions)
    {
        HashSet<Cell> movementAvailableCells = new HashSet<Cell>();

        for (int i = 1; i <= cellDistance; i++)
        {
            Vector2Int posiblePosition = currentPosition + direction * i;

            if (!board.ExistCellsWithSpecificCoordinate(posiblePosition))
            {
                if (!jumpObstructions) return movementAvailableCells; //Any other possible further cell is discarded
                else continue;
            }

            Cell posibleCell = board.GetCellWithSpecificCoordinate(posiblePosition);

            if (!posibleCell.CanBeOccupied())
            {
                if (!jumpObstructions) return movementAvailableCells;
                else continue;
            }

            if (!posibleCell.CanBeStepped())
            {
                if (!jumpObstructions) return movementAvailableCells;
                else continue;
            }

            movementAvailableCells.Add(posibleCell);
        }

        return movementAvailableCells;
    }
}
