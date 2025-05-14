using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityArmorStatResolver : EntityIntStatResolver
{
    protected override int CalculateStat() => entitySO.baseArmor;
}

