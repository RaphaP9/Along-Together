using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    //We can afford making this class a Singleton because each game only does one thing
    public static Board Instance {  get; private set; }

    [Header("Lists - Runtime Filled")]
    [SerializeField] private List<Cell> cells;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<Cell> Cells => cells;

    private void Awake()
    {
        SetSingleton();
        FillCellsList();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one Board instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void FillCellsList()
    {
        cells = FindObjectsOfType<Cell>().ToList();
    }

    public List<Cell> GetCellsWithYCoordinate(int yCoordinate)
    {
        List<Cell> foundCells = new List<Cell>();

        foreach(Cell cell in cells)
        {
            if (cell.Position.y == yCoordinate) foundCells.Add(cell);
        }

        if(foundCells.Count <= 0)
        {
            if (debug) Debug.Log($"No cells found with Y Coordinate:{yCoordinate}");
        }

        return foundCells;
    }

    public List<Cell> GetCellsWithXCoordinate(int xCoordinate)
    {
        List<Cell> foundCells = new List<Cell>();

        foreach (Cell cell in cells)
        {
            if (cell.Position.x == xCoordinate) foundCells.Add(cell);
        }

        if (foundCells.Count <= 0)
        {
            if (debug) Debug.Log($"No cells found with X Coordinate:{xCoordinate}");
        }

        return foundCells;
    }

    public Cell GetCellWithSpecificCoordinate(Vector2Int specificCoordinate)
    {
        foreach (Cell cell in cells)
        {
            if (cell.Position == specificCoordinate) return cell;
        }

        if(debug) Debug.Log($"No cells found with Coordinates: {specificCoordinate}");
        return null;
    }

    public bool ExistCellsWithYCoordinate(int yCoordinate)
    {
        foreach (Cell cell in cells)
        {
            if (cell.Position.y == yCoordinate) return true;
        }

        return false;
    }

    public bool ExistCellsWithXCoordinate(int xCoordinate)
    {
        foreach (Cell cell in cells)
        {
            if (cell.Position.x == xCoordinate) return true;
        }

        return false;
    }

    public bool ExistCellsWithSpecificCoordinate(Vector2Int specificCoordinate)
    {
        foreach (Cell cell in cells)
        {
            if (cell.Position == specificCoordinate) return cell;
        }

        return false;
    }
}
