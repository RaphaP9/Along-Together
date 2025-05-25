using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private DialogueSO currentDialogueSO;

    [Header("States - Runtime Filled")]
    [SerializeField] private DialogueState dialogueState;

    public DialogueState State => dialogueState;

    public enum DialogueState { NotOnDialogue, DialogueTransitionIn, DialogueTransitionOut, Idle, SentenceTransitionIn, SentenceTransitionOut }

    #region Flags
    private bool dialogueTransitionInCompleted = false;
    private bool dialogueTransitionOutCompleted = false;

    private bool sentenceTransitionInCompleted = false;
    private bool sentenceTransitionOutCompleted = false;

    private bool dialogueConcluded = false;

    private bool shouldSkipSentence = false;
    private bool shouldSkipDialogue = false;
    #endregion

    #region Events
    public static event EventHandler<OnDialogueEventArgs> OnDialogueBegin;   
    public static event EventHandler<OnDialogueEventArgs> OnDialogueEnd;

    public static event EventHandler<OnDialogueEventArgs> OnSentenceBegin;
    public static event EventHandler<OnDialogueEventArgs> OnSentenceEnd;
    #endregion

    public class OnDialogueEventArgs : EventArgs
    {
        public DialogueSO dialogueSO;
        public DialogueSentence dialogueSentence;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("There is more than one DialogueManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region Logic
    public void StartDialogue(DialogueSO dialogueSO)
    {
        if (!CanStartDialogue()) return;

        StartCoroutine(DialogueCoroutine(dialogueSO));
    }

    private IEnumerator DialogueCoroutine(DialogueSO dialogueSO)
    {


        //OnDialogueBegin?.Invoke(this)

        yield return new WaitForEndOfFrame();
    }
    #endregion

    private bool CanStartDialogue()
    {
        if (dialogueState != DialogueState.NotOnDialogue) return false;
        return true;
    }

    #region Setters

    #endregion
}
