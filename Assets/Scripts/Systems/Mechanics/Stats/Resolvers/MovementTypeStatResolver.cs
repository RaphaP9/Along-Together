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
                resolvedMovementTypes.Add(statManager.ReplacementStatModifiers[^1].asset as MovementTypeSO); //Return the first 
                return resolvedMovementTypes;
            }
        }

        foreach (MovementTypeSO movementType in characterSO.movementTypes)
        {
            resolvedMovementTypes.Add(movementType);
        }

        foreach(MovementTypeStatModificationManager statManager in movementTypeStatModificationManagers)
        {
            foreach (AssetStatModifier statModifier in statManager.UnionStatModifiers)
            {
                resolvedMovementTypes.Add(statModifier.asset as MovementTypeSO);
            }
        }

        return resolvedMovementTypes;
    }
}