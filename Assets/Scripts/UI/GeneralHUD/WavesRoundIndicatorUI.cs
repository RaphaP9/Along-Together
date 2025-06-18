using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WavesRoundIndicatorUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI wavesRoundIndicatorText;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        WavesRoundHandler.OnWavesRoundStart += WavesRoundHandler_OnWavesRoundStart;
        WavesRoundHandler.OnWavesRoundCompleted += WavesRoundHandler_OnWavesRoundCompleted;

        WavesRoundHandler.OnWavesRoundWaveStart += WavesRoundHandler_OnWavesRoundWaveStart;
        WavesRoundHandler.OnWavesRoundWaveCompleted += WavesRoundHandler_OnWavesRoundWaveCompleted;
    }

    private void OnDisable()
    {
        WavesRoundHandler.OnWavesRoundStart -= WavesRoundHandler_OnWavesRoundStart;
        WavesRoundHandler.OnWavesRoundCompleted -= WavesRoundHandler_OnWavesRoundCompleted;

        WavesRoundHandler.OnWavesRoundWaveStart -= WavesRoundHandler_OnWavesRoundWaveStart;
        WavesRoundHandler.OnWavesRoundWaveCompleted -= WavesRoundHandler_OnWavesRoundWaveCompleted;
    }

    public void ShowUI()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }

    public void HideUI()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }

    private void SetWavesRoundIndicatorText(int currentWave, int totalWaves) => wavesRoundIndicatorText.text = $"{currentWave}/{totalWaves}";


    private void WavesRoundHandler_OnWavesRoundStart(object sender, WavesRoundHandler.OnWavesRoundEventArgs e)
    {
        ShowUI();
    }

    private void WavesRoundHandler_OnWavesRoundCompleted(object sender, WavesRoundHandler.OnWavesRoundEventArgs e)
    {
        HideUI();
    }

    private void WavesRoundHandler_OnWavesRoundWaveStart(object sender, WavesRoundHandler.OnWavesRoundWaveEventArgs e)
    {
        SetWavesRoundIndicatorText(e.currentWave, e.totalWaves);
    }

    private void WavesRoundHandler_OnWavesRoundWaveCompleted(object sender, WavesRoundHandler.OnWavesRoundWaveEventArgs e)
    {
        //
    }

}
