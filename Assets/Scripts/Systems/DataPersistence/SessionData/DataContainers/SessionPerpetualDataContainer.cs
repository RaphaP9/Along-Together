using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SessionPerpetualDataContainer : MonoBehaviour
{
    public static SessionPerpetualDataContainer Instance { get; private set; }

    [Header("Data")]
    [SerializeField] private PerpetualData perpetualData;

    public PerpetualData PerpetualData => perpetualData;

    private void OnEnable()
    {
        WinManager.OnRunCompleted += WinManager_OnRunCompleted;
    }

    private void OnDisable()
    {
        WinManager.OnRunCompleted -= WinManager_OnRunCompleted;
    }

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

    private void AddUnlockedCharacterIDs(List<int> characterIDs)
    {
        perpetualData.unlockedCharacterIDs.AddRange(characterIDs);
        perpetualData.unlockedCharacterIDs = perpetualData.unlockedCharacterIDs.Distinct().ToList();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void SetHasCompletedTutorial(bool hasCompletedTutorial) => perpetualData.hasCompletedTutorial = hasCompletedTutorial;
    public void SetUnlockedCharacterIDs(List<int> unlockedCharacterIDs) => perpetualData.unlockedCharacterIDs = unlockedCharacterIDs;

    #region Subscriptions

    private void WinManager_OnRunCompleted(object sender, WinManager.OnRunCompletedEventArgs e)
    {
        AddUnlockedCharacterIDs(GeneralGameSettings.Instance.GetRunCompletedUnlockedCharacterIDsByCharacterSO(e.characterSO));
    }
    #endregion
}
