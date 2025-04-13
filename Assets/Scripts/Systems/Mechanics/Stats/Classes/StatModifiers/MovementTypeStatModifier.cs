using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTypeStatModifier : AssetStatModifier
{
    public MovementTypeSO movementTypeSO;

    public override StatType GetStatType() => StatType.MovementType;
}

