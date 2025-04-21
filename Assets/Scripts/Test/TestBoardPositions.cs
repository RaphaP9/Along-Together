using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoardPositions : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementTypeSO movementTypeSO;
    [SerializeField] private Transform temporalTestObjectPrefab;
    [SerializeField] private int movementDistance;
    [SerializeField] private int obstructionJumps;

    [Header("Settings")]
    [SerializeField] private Vector2Int currentPos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Test(); //CheckAvailableMovementPositions
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            GameLogManager.Instance.Log("Test"); //Shake
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            GameLogManager.Instance.Log("Start"); //TransitionStart
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            GameLogManager.Instance.Log("End"); //TransitionEnd
        }
    }

    private void Test()
    {
        HashSet<Cell> cells = movementTypeSO.GetMovementAvailableCells(currentPos,Board.Instance,movementDistance, obstructionJumps);

        foreach (Cell cell in cells)
        {
            Instantiate(temporalTestObjectPrefab, GeneralUtilities.Vector2IntToVector3(cell.Position), Quaternion.identity);
        }
    }
}
