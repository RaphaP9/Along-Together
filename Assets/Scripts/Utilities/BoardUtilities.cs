using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class BoardUtilities
{
    #region Directional Vector2Ints
    public static readonly Vector2Int Up = new(0,1);
    public static readonly Vector2Int Down = new(0,-1);
    public static readonly Vector2Int Left = new(-1,0);
    public static readonly Vector2Int Right = new(1,0);

    public static readonly Vector2Int UpRightDiagonal = new(1, 1);
    public static readonly Vector2Int DownRightDiagonal = new(1, -1);
    public static readonly Vector2Int UpLeftDiagonal = new(-1, 1);
    public static readonly Vector2Int DownLeftDiagonal = new(-1, -1);
    #endregion

    #region Knight RelativePositions Vector2Ints
    public static readonly Vector2Int KnightA = new(1, 2);
    public static readonly Vector2Int KnightB = new(1, -2);
    public static readonly Vector2Int KnightC = new(-1, 2);
    public static readonly Vector2Int KnightD = new(-1, -2);
    public static readonly Vector2Int KnightE = new(2, 1);
    public static readonly Vector2Int KnightF = new(2, -1);
    public static readonly Vector2Int KnightG = new(-2, 1);
    public static readonly Vector2Int KnightH = new(-2, -1);
    #endregion

    private const bool DEBUG = true;

    #region Positions
    public static Vector2Int GetIntPositionDueToWorldPosition(Transform transform)
    {
        Vector2 rawPosition = GeneralUtilities.SupressZComponent(transform.position);
        Vector2Int intPosition = GeneralUtilities.Vector2ToVector2Int(rawPosition);
        return intPosition;
    }

    #endregion

    #region Cells
    public static Cell GetFurthestCellInDirection(Vector2Int currentPosition, Board board, Vector2Int direction)
    {
        if(direction == Vector2.zero)
        {
            if (DEBUG) Debug.Log("Direction is Vector2Int Zero. Returning null");
            return null;
        }

        Cell furthestCell = null;

        foreach(Cell cell in board.Cells)
        {
            Vector2Int relativePosition = cell.Position - currentPosition;

            if (!GeneralUtilities.CheckVectorIntsAreSameDirection(relativePosition, direction)) continue;

            if(furthestCell == null)
            {
                furthestCell = cell;
                continue;
            }

            if(cell.Position.magnitude > furthestCell.Position.magnitude)
            {
                furthestCell = cell;
            }
        }

        return furthestCell;
    }

    public static HashSet<Cell> ResolveAvailableCells(Vector2Int currentPosition, Board board, List<MovementTypeSO> movementTypes)
    {
        HashSet<Cell> availableCells = new HashSet<Cell>();

        foreach(MovementTypeSO movementTypeSO in movementTypes)
        {
            HashSet<Cell> availableMovementTypeCells = movementTypeSO.GetMovementAvailableCells(currentPosition, board);
            availableCells.AddRange(availableMovementTypeCells);
        }

        return availableCells;
    }
    #endregion

    #region AvailableMovementCells
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

        if (direction == Vector2Int.zero) return movementAvailableCells;

        int numberOfObstructions = 0;

        for (int i = 1; i <= cellDistance; i++)
        {
            Vector2Int posiblePosition = currentPosition + direction * i;

            if (!board.ExistCellsWithSpecificCoordinate(posiblePosition))
            {
                if (!jumpObstructions) return movementAvailableCells; //Any other possible further cell is discarded
                else
                {
                    numberOfObstructions++;
                    continue;
                }
            }

            Cell posibleCell = board.GetCellWithSpecificCoordinate(posiblePosition);

            if (!posibleCell.CanBeOccupied())
            {
                if (!jumpObstructions) return movementAvailableCells;
                else
                {
                    numberOfObstructions++;
                    continue;
                }
            }

            if (!posibleCell.CanBeStepped())
            {
                if (!jumpObstructions) return movementAvailableCells;
                else
                {
                    numberOfObstructions++;
                    continue;
                }
            }

            movementAvailableCells.Add(posibleCell);
        }

        return movementAvailableCells;
    }

    public static HashSet<Cell> GetAvailableMovementCellsByDirectionsUnlimited(Vector2Int currentPosition, Board board, HashSet<Vector2Int> directions, bool obstructDirection)
    {
        HashSet<Cell> movementAvailableCells = new HashSet<Cell>();

        foreach (Vector2Int direction in directions)
        {
            HashSet<Cell> cells = GetAvailableMovementCellsByDirectionUnlimited(currentPosition, board, direction, obstructDirection);
            movementAvailableCells.AddRange(cells);
        }

        return movementAvailableCells;
    }

    public static HashSet<Cell> GetAvailableMovementCellsByDirectionUnlimited(Vector2Int currentPosition, Board board, Vector2Int direction, bool jumpObstructions)
    {
        HashSet<Cell> movementAvailableCells = new HashSet<Cell>();

        if (direction == Vector2Int.zero) return movementAvailableCells;

        Cell furthestCellInDirection = GetFurthestCellInDirection(currentPosition, board, direction);

        if(furthestCellInDirection == null) return movementAvailableCells; //If there are no cells in that direction, we return an empty list

        int distanceToCover = 0;
        int numberOfObstructions = 0;

        bool conditionToBreak = false;

        while (!conditionToBreak)
        {
            distanceToCover++;
            Vector2Int posiblePosition = currentPosition + direction * distanceToCover;

            if (!board.ExistCellsWithSpecificCoordinate(posiblePosition)) //If a cell does not exist there, it is considered an obstruction
            {
                if (!jumpObstructions) //It can not jump obstructions, the condition to break is covered and the while loop concludes
                {
                    conditionToBreak = true;
                }
                else //Otherwise, keep count on the number of obstructions
                {
                    numberOfObstructions++;
                }

                continue; //Go to next while loop
            }

            Cell possibleCell = board.GetCellWithSpecificCoordinate(posiblePosition);

            if (possibleCell == furthestCellInDirection) //We have reached the furthest cell in direction, no more cells in that direction, so we meet the condition to break and this will be the last loop
            {
                conditionToBreak = true;
            }

            if (!possibleCell.CanBeOccupied()) //Same logic with Occupation
            {
                if (!jumpObstructions) 
                {
                    conditionToBreak = true;
                }
                else 
                {
                    numberOfObstructions++;
                }

                continue;
            }

            if (!possibleCell.CanBeStepped()) //Same logic with Stepped
            {
                if (!jumpObstructions)
                {
                    conditionToBreak = true;
                }
                else
                {
                    numberOfObstructions++;
                }

                continue;
            }

            movementAvailableCells.Add(possibleCell);
        }

        return movementAvailableCells;
    }

    public static HashSet<Cell> GetAvailableMovementCellsByRelativePositions(Vector2Int currentPosition, Board board, HashSet<Vector2Int> relativePositions)
    {
        HashSet<Cell> movementAvailableCells = new HashSet<Cell>();

        foreach (Vector2Int relativePosition in relativePositions)
        {
            Cell cell = GetAvailableMovementCellByRelativePosition(currentPosition, board, relativePosition);

            if (cell == null) continue;

            movementAvailableCells.Add(cell);
        }

        return movementAvailableCells;
    }

    public static Cell GetAvailableMovementCellByRelativePosition(Vector2Int currentPosition, Board board, Vector2Int relativePosition)
    {
        Vector2Int seekedPosition = currentPosition + relativePosition;

        if (!board.ExistCellsWithSpecificCoordinate(seekedPosition)) return null;
        
        Cell seekedCell = board.GetCellWithSpecificCoordinate(seekedPosition);

        if(!seekedCell.CanBeOccupied()) return null;
        if(!seekedCell.CanBeStepped()) return null;

        return seekedCell;
    }
    #endregion
}
