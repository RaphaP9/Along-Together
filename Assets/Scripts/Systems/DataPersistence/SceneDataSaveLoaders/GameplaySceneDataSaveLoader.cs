using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySceneDataSaveLoader : MonoBehaviour
{
    public static GameplaySceneDataSaveLoader Instance {  get; private set; }

    [Header("Settings")]
    [SerializeField] private bool loadDataOnAwake;
    [SerializeField] private bool saveDataOnQuit;

    private void Awake()
    {
        SetSingleton();
        if (loadDataOnAwake) GeneralDataSaveLoader.Instance.CompleteDataLoad();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        if(saveDataOnQuit) GeneralDataSaveLoader.Instance.CompleteDataSave();
    }
}
