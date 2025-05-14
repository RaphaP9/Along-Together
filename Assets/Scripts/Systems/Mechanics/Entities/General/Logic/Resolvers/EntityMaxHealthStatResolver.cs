using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMaxHealthStatResolver : EntityIntStatResolver
{
    protected override int CalculateStat() => entitySO.baseHealth;
}
