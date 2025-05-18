using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoundSO : MonoBehaviour
{
    [Header("Settings")]
    public int roundID;
    public RoundTier RoundTier;

    public abstract RoundType GetRoundType();
}
