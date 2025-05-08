using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivePassiveAbility : Ability, IActiveAbility, IPassiveAbility
{
    public bool AbilityCastInput()
    {
        throw new System.NotImplementedException();
    }

    public float CalculateAbilityCooldown()
    {
        throw new System.NotImplementedException();
    }
}
