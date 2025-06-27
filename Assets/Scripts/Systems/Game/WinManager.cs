using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(2f, 5f)] private float timeToEndAfterWin;
    [SerializeField] private string winScene;
    [SerializeField] private TransitionType winTransitionType;
    [Space]
    [SerializeField] private bool wipeRunDataOnWin;

    public static event EventHandler<OnRunCompletedEventArgs> OnRunCompleted;
    public static event EventHandler OnTriggerDataSaveOnRunCompleted;

    public class OnRunCompletedEventArgs : EventArgs
    {
        public CharacterSO characterSO;
    }

    private void OnEnable()
    {
        GameManager.OnGameWon += GameManager_OnGameWon;
    }

    private void OnDisable()
    {
        GameManager.OnGameWon -= GameManager_OnGameWon;
    }

    private void WinGame()
    {
        OnRunCompleted?.Invoke(this, new OnRunCompletedEventArgs { characterSO = PlayerCharacterManager.Instance.CharacterSO });
        OnTriggerDataSaveOnRunCompleted?.Invoke(this, EventArgs.Empty);

        if (wipeRunDataOnWin)
        {
            DataUtilities.WipeRunData();
            SessionRunDataContainer.Instance.ResetRunData();
        }

        StartCoroutine(WinGameCoroutine());
    }

    private IEnumerator WinGameCoroutine()
    {
        yield return new WaitForSeconds(timeToEndAfterWin);
        ScenesManager.Instance.TransitionLoadTargetScene(winScene, winTransitionType);
    }

    private void GameManager_OnGameWon(object sender, System.EventArgs e)
    {
        WinGame();
    }
}

