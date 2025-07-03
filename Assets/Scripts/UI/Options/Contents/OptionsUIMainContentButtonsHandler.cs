using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUIMainContentButtonsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private OptionsUIContentsHandler optionsUIContentsHandler;

    [Header("UI Components")]
    [SerializeField] private Button audioOptionsButton;
    [SerializeField] private Button graphicsOptionsButton;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        audioOptionsButton.onClick.AddListener(ShowAudioContent);
        graphicsOptionsButton.onClick.AddListener(ShowGraphicsContent);
    }

    private void ShowAudioContent() => optionsUIContentsHandler.ShowAudioOptionsContent();
    private void ShowGraphicsContent() => optionsUIContentsHandler.ShowGraphicsOptionsContent();
}
