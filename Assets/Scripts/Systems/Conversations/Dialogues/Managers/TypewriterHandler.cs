using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypewriterHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI sentenceText;

    [Header("Typewriter Settings")]
    [SerializeField, Range(0f, 2f)] private float timeToStartTypewriting;
    [SerializeField, Range(1, 100)] private int charactersPerSecond;
    [SerializeField, Range(0f, 3f)] private float interpuntuationDelay;
    [Space]
    [SerializeField] private bool speedUpSkip;
    [SerializeField, Range(2, 50)] private int skipSpeedMultiplier;

    [Header("Runtime Filled")]
    [SerializeField] private string typewriterText;

    private int currentVisibleCharacterIndex;

    private float regularDelay;
    private float skipDelay;

    public bool isSkipping;
    public bool isCompleted;

    private bool delayInterrupted;
    private bool delayCompleted;
    private bool skippedThisFrame;

    private const char PERIOD_CHARACTER = '.';

    private Coroutine typewriterCoroutine;
    private Coroutine delayCoroutine;

    private bool shouldSkip = false;
    private bool finishedTyping = false;

    private bool SkipInput => ConversationsInput.Instance.GetSkipDown();

    #region Events
    public static event EventHandler<OnTypewriterSentenceEventArgs> OnTypewriterSentenceStart;
    public static event EventHandler<OnTypewriterSentenceEventArgs> OnTypewriterSentenceTypingBegin;
    public static event EventHandler<OnTypewriterSentenceEventArgs> OnTypewriterSentenceTypingSkip;
    public static event EventHandler<OnTypewriterSentenceEventArgs> OnTypewriterSentenceTypingComplete;
    public static event EventHandler<OnTypewriterSentenceEventArgs> OnTypewriterSentenceEnd;

    public static event EventHandler<OnTypewriterSentenceEventArgs> OnTypewriterCompleted;
    #endregion

    #region Classes
    public class OnTypewriterSentenceEventArgs : EventArgs
    {
        public string typewriterText;
    }
    #endregion

    private void OnEnable()
    {
        DialogueManager.OnSentenceBegin += DialogueManager_OnSentenceBegin;

        DialogueManager.OnGeneralDialogueBegin += DialogueManager_OnGeneralDialogueBegin;
        DialogueManager.OnGeneralDialogueConcluded += DialogueManager_OnGeneralDialogueConcluded;
        DialogueManager.OnMidSentences += DialogueManager_OnMidSentences;
    }

    private void OnDisable()
    {
        DialogueManager.OnSentenceBegin -= DialogueManager_OnSentenceBegin;

        DialogueManager.OnGeneralDialogueBegin -= DialogueManager_OnGeneralDialogueBegin;
        DialogueManager.OnGeneralDialogueConcluded -= DialogueManager_OnGeneralDialogueConcluded;
        DialogueManager.OnMidSentences -= DialogueManager_OnMidSentences;
    }

    private void Start()
    {
        ResetFlags();
        InitializeVariables();
    }

    private void Update()
    {
        HandleSkip();
    }

    private void InitializeVariables()
    {
        regularDelay = 1f / charactersPerSecond;
        skipDelay = regularDelay / skipSpeedMultiplier;
    }

    private void ResetFlags()
    {
        isSkipping = false;
        isCompleted = false;
        delayInterrupted = false;
        delayCompleted = false;
        skippedThisFrame = false;
    }

    private void HandleSkip()
    {
        if (!SkipInput) return;
        shouldSkip = true;
    }

    #region Logic
    private void StartTypewriting(string text)
    {
        if (typewriterCoroutine != null) StopCoroutine(typewriterCoroutine);

        sentenceText.text = text;

        AssignTypewritterCoroutineRefference(text); //Includes Coroutine Start
    }

    private IEnumerator TypewriterCoroutine(string textToTypewrite)
    {
        UpdateTypewriterText(textToTypewrite);

        ResetMaxVisibleCharacters();
        ResetCurrentVisibleCharacterIndex();

        sentenceText.ForceMeshUpdate();

        TMP_TextInfo textInfo = sentenceText.textInfo;
        int lastCharacterIndex = textInfo.characterCount - 1;

        #region TimeToStartTypewritingLogic Logic
        delayCompleted = false;
        AssignDelayCoroutineRefference(timeToStartTypewriting);
        yield return new WaitUntil(() => shouldSkip || delayCompleted);
        ResetDelayCoroutineRefference();
        delayCompleted = false;
        #endregion

        while (currentVisibleCharacterIndex < textInfo.characterCount)
        {
            if (shouldSkip && !speedUpSkip) break;

            sentenceText.maxVisibleCharacters++;
            sentenceText.ForceMeshUpdate();

            char character = textInfo.characterInfo[currentVisibleCharacterIndex].character;

            float delayBetweenCharacters = EvaluateInterpuntuationCharacter(character) ? interpuntuationDelay : regularDelay;
            delayBetweenCharacters = (shouldSkip && speedUpSkip)? skipDelay : delayBetweenCharacters;

            #region DelayBetweenCharacters Logic
            delayCompleted = false;
            AssignDelayCoroutineRefference(delayBetweenCharacters);
            yield return new WaitUntil(() => shouldSkip || delayCompleted);
            ResetDelayCoroutineRefference();
            delayCompleted = false;
            #endregion

            currentVisibleCharacterIndex++;
        }

        shouldSkip = false;

        sentenceText.maxVisibleCharacters = textInfo.characterCount;
        sentenceText.ForceMeshUpdate();

        OnTypewriterSentenceTypingComplete?.Invoke(this, new OnTypewriterSentenceEventArgs { typewriterText = typewriterText });
    }

    private IEnumerator DelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        delayCompleted = true;
    }

    #endregion

    #region Evaluators
    private bool EvaluateInterpuntuationCharacter(char character)
    {
        if (character == PERIOD_CHARACTER) return true;

        return false;
    }
    #endregion

    #region Setters
    private void ResetCurrentVisibleCharacterIndex() => currentVisibleCharacterIndex = 0;
    private void ResetMaxVisibleCharacters() => sentenceText.maxVisibleCharacters = 0;

    private void AssignTypewritterCoroutineRefference(string textToTypewrite) => typewriterCoroutine = StartCoroutine(TypewriterCoroutine(textToTypewrite));
    private void ResetTypewritterCoroutineRefference() => typewriterCoroutine = null;

    private void AssignDelayCoroutineRefference(float delay) => delayCoroutine = StartCoroutine(DelayCoroutine(delay));
    private void ResetDelayCoroutineRefference() => delayCoroutine = null;

    private void UpdateTypewriterText(string text)
    {
        typewriterText = text;
        sentenceText.text = text;
    }

    private void ClearTypewriterText()
    {
        typewriterText = "";
        sentenceText.text = "";
    }

    private void CompleteTypewriterReset()
    {
        ResetTypewritterCoroutineRefference();
        ResetDelayCoroutineRefference();

        ResetCurrentVisibleCharacterIndex();
        ResetMaxVisibleCharacters();

        ClearTypewriterText();
    }
    #endregion

    #region Subscriptions
    private void DialogueManager_OnSentenceBegin(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        StartTypewriting(e.dialogueSentence.sentenceText);  
    }

    private void DialogueManager_OnGeneralDialogueBegin(object sender, EventArgs e)
    {
        CompleteTypewriterReset();
    }

    private void DialogueManager_OnGeneralDialogueConcluded(object sender, EventArgs e)
    {
        CompleteTypewriterReset();
    }

    private void DialogueManager_OnMidSentences(object sender, EventArgs e)
    {
        CompleteTypewriterReset();
    }
    #endregion
}

