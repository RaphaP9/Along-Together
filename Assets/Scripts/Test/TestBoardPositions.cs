using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoardPositions : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementTypeSO movementTypeSO;
    [SerializeField] private Transform temporalTestObjectPrefab;

    [Header("Settings")]
    [SerializeField] private Vector2Int currentPos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Test();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            GameLogManager.Instance.Log("Test");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            GameLogManager.Instance.Log("Start");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            GameLogManager.Instance.Log("End");
        }
    }

    private void Test()
    {
        HashSet<Cell> cells = movementTypeSO.GetMovementAvailableCells(currentPos,Board.Instance);

        foreach (Cell cell in cells)
        {
            Instantiate(temporalTestObjectPrefab, GeneralUtilities.Vector2IntToVector3(cell.Position), Quaternion.identity);
        }
    }
}
