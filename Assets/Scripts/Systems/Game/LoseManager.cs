using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoseManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(2f, 5f)] private float timeToEndAfterLose;
    [SerializeField] private string loseScene;
    [SerializeField] private TransitionType loseTransitionType;
    [Space]
    [SerializeField] private bool wipeRunDataOnLose;

    public static event EventHandler<OnRunLostEventArgs> OnRunLost;
    public static event EventHandler OnTriggerDataSaveOnRunLost;

    public class OnRunLostEventArgs : EventArgs
    {
        public CharacterSO characterSO;
    }

    private void OnEnable()
    {
        GameManager.OnGameLost += GameManager_OnGameLost;
    }

    private void OnDisable()
    {
        GameManager.OnGameLost -= GameManager_OnGameLost;
    }

    private void LoseGame()
    {
        OnRunLost?.Invoke(this, new OnRunLostEventArgs { characterSO = PlayerCharacterManager.Instance.CharacterSO });
        OnTriggerDataSaveOnRunLost?.Invoke(this, EventArgs.Empty);

        if (wipeRunDataOnLose)
        {
            DataUtilities.WipeRunData();
            SessionRunDataContainer.Instance.ResetRunData();
        }

        StartCoroutine(LoseGameCoroutine());
    }

    private IEnumerator LoseGameCoroutine()
    {
        yield return new WaitForSeconds(timeToEndAfterLose);
        ScenesManager.Instance.TransitionLoadTargetScene(loseScene, loseTransitionType);
    }

    private void GameManager_OnGameLost(object sender, System.EventArgs e)
    {
        LoseGame();
    }

}
