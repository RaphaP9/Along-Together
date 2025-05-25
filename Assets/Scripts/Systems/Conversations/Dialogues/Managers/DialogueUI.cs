using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI sentenceText;
    [Space]
    [SerializeField] private Image speakerImage;
    [SerializeField] private TextMeshProUGUI speakerNameText;

    [Header("Positions & Transforms")]
    [SerializeField] private RectTransform speakerGroupTransform;
    [SerializeField] private RectTransform sentenceTextTransform;
    [Space]
    [SerializeField] private RectTransform leftSpeakerGroupPosition;
    [SerializeField] private RectTransform leftSentenceTextPosition;
    [Space]
    [SerializeField] private RectTransform rightSpeakerGroupPosition;
    [SerializeField] private RectTransform rightSentenceTextPosition;

    [Header("Runtime Filled")]
    [SerializeField] private DialogueSentence currentDialogueSentence;

    private const string HIDDEN_ANIMATION_NAME = "Hidden";
    private const string Idle_ANIMATION_NAME = "Idle";

    private const string DIALOGUE_TRANSITION_IN_ANIMATION_NAME = "DialogueTransitionIn";
    private const string DIALOGUE_TRANSITION_OUT_ANIMATION_NAME = "DialogueTransitionOut";

    private const string SENTENCE_TRANSITION_IN_ANIMATION_NAME = "SentenceTransitionIn";
    private const string SENTENCE_TRANSITION_OUT_ANIMATION_NAME = "SentenceTransitionOut";

    #region Events
    public static event EventHandler<OnDialogueSentenceEventArgs> OnDialogueTransitionInStart;
    public static event EventHandler<OnDialogueSentenceEventArgs> OnDialogueTransitionInEnd;

    public static event EventHandler<OnDialogueSentenceEventArgs> OnSentenceTransitionInStart;
    public static event EventHandler<OnDialogueSentenceEventArgs> OnSentenceTransitionInEnd;

    public static event EventHandler<OnDialogueSentenceEventArgs> OnSentenceTransitionOutStart;
    public static event EventHandler<OnDialogueSentenceEventArgs> OnSentenceTransitionOutEnd;

    public static event EventHandler<OnDialogueSentenceEventArgs> OnDialogueTransitionOutStart;
    public static event EventHandler<OnDialogueSentenceEventArgs> OnDialogueTransitionOutEnd;
    #endregion

    public class OnDialogueSentenceEventArgs : EventArgs
    {

    }

    #region Animation Event Methods
    private void TriggerDialogueTransitionInEnd() => OnDialogueTransitionInEnd?.Invoke(this, new OnDialogueSentenceEventArgs { });
    private void TriggerDialogueTransitionOutEnd() => OnDialogueTransitionOutEnd?.Invoke(this, new OnDialogueSentenceEventArgs { });
    private void TriggerSentenceTransitionInEnd() => OnSentenceTransitionInEnd?.Invoke(this, new OnDialogueSentenceEventArgs { });
    private void TriggerSentenceTransitionOutEnd() => OnSentenceTransitionOutEnd?.Invoke(this, new OnDialogueSentenceEventArgs { });
    #endregion

    #region Set 
    private void SetSentenceUI(DialogueSentence dialogueSentence)
    {
        SetCurrentDialogueSentence(dialogueSentence);

        sentenceText.text = dialogueSentence.sentenceText;
        speakerImage.sprite = dialogueSentence.dialogueSpeakerSO.speakerImage;
        speakerNameText.text = dialogueSentence.dialogueSpeakerSO.speakerName;
        speakerNameText.color = dialogueSentence.dialogueSpeakerSO.nameColor;

        if (dialogueSentence.speakerOnRight) SetRightSpeakerUIPosition();
        else SetLeftSpeakerUIPosition();
    }

    private void SetLeftSpeakerUIPosition()
    {
        speakerGroupTransform.position = leftSpeakerGroupPosition.position;
        sentenceTextTransform.position = leftSentenceTextPosition.position;
    }

    private void SetRightSpeakerUIPosition()
    {
        speakerGroupTransform.position = rightSpeakerGroupPosition.position;
        sentenceTextTransform.position = rightSentenceTextPosition.position;
    }

    private void SetCurrentDialogueSentence(DialogueSentence dialogueSentence) => currentDialogueSentence = dialogueSentence;
    #endregion
}
