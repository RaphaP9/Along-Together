using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PerpetualData : DataModel
{
    public bool hasCompletedTutorial;
    public List<int> unlockedCharacterIDs;

    public PerpetualData()
    {
        hasCompletedTutorial = false;
        unlockedCharacterIDs = new List<int>();
    }

    public override void Initialize()
    {
        if (GeneralGameSettings.Instance == null)
        {
            Debug.Log("GeneralGameSettings Instance is null. Can not Initialize DataModel.");
            return;
        }

        unlockedCharacterIDs = GeneralGameSettings.Instance.GetStartingUnlockedCharacterIDs();
    }
}
