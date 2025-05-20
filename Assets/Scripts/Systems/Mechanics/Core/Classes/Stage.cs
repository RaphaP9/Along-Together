using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage
{
    public List<RoundGroup> roundGroups;

    public int GetRoundsQuantityInStage() => roundGroups.Count;
}
