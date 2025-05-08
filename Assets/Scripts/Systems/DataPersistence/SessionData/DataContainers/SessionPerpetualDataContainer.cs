using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionPerpetualDataContainer : MonoBehaviour
{
    public static SessionPerpetualDataContainer Instance { get; private set; }

    [Header("Data")]
    [SerializeField] private PerpetualData perpetualData = new();

    public PerpetualData PerpetualData => perpetualData;

    #region Singleton Initialization
    private void Awake()
    {
        SetSingleton();
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
    #endregion

    public void SetPerpetualData(PerpetualData perpetualData) => this.perpetualData = perpetualData;
    public void ClearPerpetualData() => perpetualData = new();

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void SetNumericStats(List<DataModeledNumericStat> dataModeledNumericStats) => perpetualData.numericStats = dataModeledNumericStats;
    public void SetAssetStats(List<DataModeledAssetStat> dataModeledAssetStats) => perpetualData.assetStats = dataModeledAssetStats;
}
