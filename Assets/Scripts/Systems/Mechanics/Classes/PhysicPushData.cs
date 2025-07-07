using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhysicPushData 
{
    [Range(1f, 100f)] public float pushForce;
    [Range(1f, 8f)] public float actionRadius;

    public PhysicPushData(float pushForce, float actionRadius)
    {
        this.pushForce = pushForce;
        this.actionRadius = actionRadius;
    }
}
