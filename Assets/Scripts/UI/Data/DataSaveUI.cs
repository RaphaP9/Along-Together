using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaveUI : MonoBehaviour
{
    [Header("UIComponents")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField,Range(1f,4f)] private float minimumShowingInidicatorTime;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string SHOWING_ANIMATION = "Showing";

    private bool dataSaveCompleted = false;

    private void OnEnable()
    {
        GeneralDataSaveLoader.OnDataSaveStart += GeneralDataSaveLoader_OnDataSaveStart;
        GeneralDataSaveLoader.OnDataSaveComplete += GeneralDataSaveLoader_OnDataSaveComplete;
    }

    private void OnDisable()
    {
        GeneralDataSaveLoader.OnDataSaveStart -= GeneralDataSaveLoader_OnDataSaveStart;
        GeneralDataSaveLoader.OnDataSaveComplete -= GeneralDataSaveLoader_OnDataSaveComplete;
    }

    public void ShowIndicator()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }

    public void HideIndicator()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }

    private IEnumerator DataSaveIndicatorCoroutine()
    {
        ShowIndicator();

        yield return new WaitUntil(() => dataSaveCompleted);
        dataSaveCompleted = false;

        yield return new WaitForSeconds(minimumShowingInidicatorTime);

        HideIndicator();
    }

    #region Subscriptions
    private void GeneralDataSaveLoader_OnDataSaveStart(object sender, System.EventArgs e)
    {
        StartCoroutine(DataSaveIndicatorCoroutine());
    }

    private void GeneralDataSaveLoader_OnDataSaveComplete(object sender, System.EventArgs e)
    {
        dataSaveCompleted = true;
    }
    #endregion
}
