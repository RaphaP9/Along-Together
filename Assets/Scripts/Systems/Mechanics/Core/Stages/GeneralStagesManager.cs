using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStagesManager : MonoBehaviour
{
    public static GeneralStagesManager Instance {  get; private set; }

    [Header("Lists")]
    [SerializeField] private List<Stage> stages;

    public List<Stage> Stages => stages;

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
            Debug.LogWarning("There is more than one GeneralStagesManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public int GetStagesCount() => stages.Count;
}
