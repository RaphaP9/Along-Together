using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbility : Ability, IPassiveAbility
{
    private PassiveAbilitySO PassiveAbilitySO => AbilitySO as PassiveAbilitySO;
}
