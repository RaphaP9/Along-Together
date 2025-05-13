using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFacingInterruptionAbility 
{
    public bool IsInterruptingFacing();
    public Vector2 GetFacingDirection();
}
