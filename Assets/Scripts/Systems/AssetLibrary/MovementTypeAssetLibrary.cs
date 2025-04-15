using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTypeAssetLibrary : MonoBehaviour
{
    public static MovementTypeAssetLibrary Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<MovementTypeSO> movementTypes;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Debug.LogWarning("There is more than one MovementTypeAssetLibrary instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public MovementTypeSO GetMovementTypeSOByID(int id)
    {
        foreach (MovementTypeSO movementTypeSO in movementTypes)
        {
            if(movementTypeSO.id == id) return movementTypeSO;
        }

        if (debug) Debug.Log($"No MovementTypeSO matches the ID:{id}. Returning null");
        return null;
    }

    public MovementTypeSO GetMovementTypeSOByMovementType(MovementType movementType)
    {
        foreach (MovementTypeSO movementTypeSO in movementTypes)
        {
            if (movementTypeSO.GetMovementType() == movementType) return movementTypeSO;
        }

        if (debug) Debug.Log($"No MovementTypeSO matches the MovementType:{movementType}. Returning null");
        return null;
    }
}
