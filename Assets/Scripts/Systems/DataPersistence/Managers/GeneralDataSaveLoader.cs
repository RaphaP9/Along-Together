using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class GeneralDataSaveLoader : MonoBehaviour
{
    public static GeneralDataSaveLoader Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private JSONPerpetualDataPersistenceManager JSONPerpetualDataPersistenceManager;
    [SerializeField] private JSONRunDataPersistenceManager JSONRunDataPersistenceManager;   

    public static event EventHandler OnDataLoadStart;
    public static event EventHandler OnDataLoadComplete;

    public static event EventHandler OnDataSaveStart;
    public static event EventHandler OnDataSaveComplete;    

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one GeneralDataSaveLoader instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }


    #region Load
    public void CompleteDataLoad()
    {
        LoadJSONData();
        LoadDataContainersData();
    }

    public async Task CompleteDataLoadAsync()
    {
        await LoadJSONDataAsync();
        LoadDataContainersData();
    }

    #region JSON Load
    public void LoadJSONData()
    {
        OnDataLoadStart?.Invoke(this, EventArgs.Empty);

        JSONPerpetualDataPersistenceManager.LoadData(); //NOTE: Order is important
        JSONRunDataPersistenceManager.LoadData();

        OnDataLoadComplete?.Invoke(this, EventArgs.Empty);
    }

    public async Task LoadJSONDataAsync()
    {
        OnDataLoadStart?.Invoke(this, EventArgs.Empty);

        await JSONPerpetualDataPersistenceManager.LoadDataAsync();
        await JSONRunDataPersistenceManager.LoadDataAsync();

        OnDataLoadComplete?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    public void LoadDataContainersData()
    {
        List<SessionDataSaveLoader> sessionDataSaveLoaders = FindObjectsOfType<SessionDataSaveLoader>().ToList();

        foreach (SessionDataSaveLoader sessionDataSaveLoader in sessionDataSaveLoaders)
        {
            sessionDataSaveLoader.InjectAllDataFromDataContainers();
        }
    }


    #endregion

    #region Save
    public void CompleteDataSave()
    {
        SaveDataContainersData();
        SaveJSONData();
    }

    public async Task CompleteDataSaveAsync()
    {
        SaveDataContainersData();
        await SaveJSONDataAsync();
    }

    #region JSON Save
    public void SaveJSONData()
    {
        OnDataSaveStart?.Invoke(this, EventArgs.Empty);

        JSONPerpetualDataPersistenceManager.SaveData();
        JSONRunDataPersistenceManager.SaveData();

        OnDataSaveComplete?.Invoke(this, EventArgs.Empty);
    }

    public async Task SaveJSONDataAsync()
    {
        OnDataSaveStart?.Invoke(this, EventArgs.Empty);

        await JSONPerpetualDataPersistenceManager.SaveDataAsync();
        await JSONRunDataPersistenceManager.SaveDataAsync();

        OnDataSaveComplete?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    public void SaveDataContainersData()
    {
        List<SessionDataSaveLoader> sessionDataSaveLoaders = FindObjectsOfType<SessionDataSaveLoader>().ToList();

        foreach (SessionDataSaveLoader sessionDataSaveLoader in sessionDataSaveLoaders)
        {
            sessionDataSaveLoader.ExtractAllDataToDataContainers();
        }
    }
    #endregion
}
