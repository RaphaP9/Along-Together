using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDisplacementAbility
{
    public bool IsDisplacing();
    public bool CanInterruptMovement();
}
