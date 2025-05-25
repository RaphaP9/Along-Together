using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private DialogueSO currentDialogueSO;
    [SerializeField] private DialogueSentence currentSentence;

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

    public static event EventHandler<OnDialogueEventArgs> OnSentenceIdle;
    public static event EventHandler OnNotOnDialogue;
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

    private void Start()
    {
        SetDialogueState(DialogueState.NotOnDialogue);
        ClearCurrentDialogue();
        ClearCurrentSentence();
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
        if (dialogueSO.dialogueSentences.Count <= 0) return;

        StartCoroutine(DialogueCoroutine(dialogueSO));
    }

    private IEnumerator DialogueCoroutine(DialogueSO dialogueSO)
    {
        bool suddenDialogueEnd = false;

        SetCurrentDialogue(dialogueSO);

        for(int i=0; i< dialogueSO.dialogueSentences.Count; i++)
        {
            SetCurrentSentence(dialogueSO.dialogueSentences[i]);

            #region Dialogue Begin Logic & Sentence Transition In Logic
            if (i ==0) //If first Sentence, DialogueIsStarting & Wait for the TransitionIn To Complete
            {
                dialogueTransitionInCompleted = false;
                OnDialogueBegin?.Invoke(this, new OnDialogueEventArgs { dialogueSentence = currentSentence });

                while (!dialogueTransitionInCompleted) yield return null; //Wait for TransitionInCompleted
                dialogueTransitionInCompleted = false;
            }
            else if(dialogueSO.dialogueSentences[i].triggerSentenceTransition) //If current sentence has the triggerSentenceTransition Checked
            {
                sentenceTransitionInCompleted = false;
                OnSentenceBegin?.Invoke(this, new OnDialogueEventArgs { dialogueSentence = currentSentence });

                while (!sentenceTransitionInCompleted) yield return null;
                sentenceTransitionInCompleted = false;
            }
            #endregion

            #region Idle Logic
            //At this point, Sentence Is On Idle
            OnSentenceIdle?.Invoke(this, new OnDialogueEventArgs { dialogueSentence = currentSentence}); //Loads the entire Sentence

            while (!shouldSkipSentence)
            {
                if (shouldSkipDialogue)
                {
                    suddenDialogueEnd = true;
                    break;
                }

                yield return null;
            }

            shouldSkipSentence = false;

            if (suddenDialogueEnd) break;
            #endregion

            #region Transition Sentence Out Logic
            if (i + 1 < dialogueSO.dialogueSentences.Count) //If it is not the last
            {
                if (dialogueSO.dialogueSentences[i + 1].triggerSentenceTransition) //If next sentence has the triggerSentenceTransition Checked
                {
                    sentenceTransitionOutCompleted = false;
                    OnSentenceEnd?.Invoke(this, new OnDialogueEventArgs { dialogueSentence = currentSentence });

                    while(!sentenceTransitionOutCompleted) yield return null;
                    sentenceTransitionOutCompleted = false;
                }
            }
            #endregion
        }

        shouldSkipDialogue = false;

        dialogueTransitionOutCompleted = false;
        OnDialogueEnd?.Invoke(this, new OnDialogueEventArgs { dialogueSentence = currentSentence });

        while(!dialogueTransitionOutCompleted) yield return null;
        dialogueTransitionOutCompleted = false;

        OnNotOnDialogue.Invoke(this, EventArgs.Empty);
        SetDialogueState(DialogueState.NotOnDialogue);

        ClearCurrentDialogue();
        ClearCurrentSentence();
    }
    #endregion

    private bool CanStartDialogue()
    {
        if (dialogueState != DialogueState.NotOnDialogue) return false;
        return true;
    }

    #region States
    private void SetDialogueState(DialogueState dialogueState) => this.dialogueState = dialogueState;


    #endregion

    #region Setters
    private void SetCurrentDialogue(DialogueSO dialogueSO) => currentDialogueSO = dialogueSO;
    private void ClearCurrentDialogue() => currentDialogueSO = null;   

    private void SetCurrentSentence(DialogueSentence sentence) => currentSentence = sentence;
    private void ClearCurrentSentence() => currentSentence = null;
    #endregion
}
