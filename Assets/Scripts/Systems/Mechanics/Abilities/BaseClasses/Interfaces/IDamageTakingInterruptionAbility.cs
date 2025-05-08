using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageTakingInterruptionAbility
{
    public bool IsInterruptingDamageTaking();
    public bool CanInterruptDamageTaking();
}
