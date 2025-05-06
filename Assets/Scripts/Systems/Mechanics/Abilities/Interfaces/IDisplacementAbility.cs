using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDisplacementAbility : IPassiveAbility
{
    public bool IsDisplacing();
    public bool InterruptMovement();
}
