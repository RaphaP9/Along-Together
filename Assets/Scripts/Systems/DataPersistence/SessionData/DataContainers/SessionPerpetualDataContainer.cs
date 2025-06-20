using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionPerpetualDataContainer : MonoBehaviour
{
    public static SessionPerpetualDataContainer Instance { get; private set; }

    [Header("Data")]
    [SerializeField] private PerpetualData perpetualData;

    public PerpetualData PerpetualData => perpetualData;

    #region Initialization
    private void Awake() //Remember this Awake Happens before all JSON awakes, initialization of container happens before JSON Load
    {
        SetSingleton();
        InitializeDataContainer();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void InitializeDataContainer()
    {
        perpetualData = new PerpetualData();
        perpetualData.Initialize();
    }
    #endregion

    public void SetPerpetualData(PerpetualData perpetualData) => this.perpetualData = perpetualData;
    
    public void ResetPerpetualData()
    {
        InitializeDataContainer();
    }

    public bool HasUnlockedCharacters()
    {
        if (perpetualData.unlockedCharacterIDs == GeneralGameSettings.Instance.GetStartingUnlockedCharacterIDs()) return false;
        return true;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void SetUnlockedCharacterIDs(List<int> unlockedCharacterIDs) => perpetualData.unlockedCharacterIDs = unlockedCharacterIDs;
    public void SetNumericStats(List<DataModeledNumericStat> dataModeledNumericStats) => perpetualData.numericStats = dataModeledNumericStats;
    public void SetAssetStats(List<DataModeledAssetStat> dataModeledAssetStats) => perpetualData.assetStats = dataModeledAssetStats;
}
