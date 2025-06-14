using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhysicPushData 
{
    [Range(1f, 15f)] public float pushForce;
    [Range(1f, 8f)] public float actionRadius;
}
