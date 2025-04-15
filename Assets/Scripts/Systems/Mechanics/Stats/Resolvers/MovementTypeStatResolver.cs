using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTypeStatResolver : StatResolver
{
    public static MovementTypeStatResolver Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<MovementTypeStatModificationManager> movementTypeStatModificationManagers;

    public List<MovementTypeStatModificationManager> MovementTypeStatModificationManagers => movementTypeStatModificationManagers;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MovementTypeStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public List<MovementTypeSO> ResolveMovementTypeStat(CharacterSO characterSO)
    {
        List<MovementTypeSO> resolvedMovementTypes = new List<MovementTypeSO>();

        foreach (MovementTypeStatModificationManager statManager in movementTypeStatModificationManagers)
        {
            if (statManager.ReplacementStatModifiers.Count > 0)
            {
                float rawValue = statManager.ReplacementStatModifiers[^1].value; //Return the first 
                return Mathf.CeilToInt(rawValue);
            }
        }
    }
}