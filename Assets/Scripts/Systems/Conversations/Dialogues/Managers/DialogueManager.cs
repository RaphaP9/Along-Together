using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("States - Runtime Filled")]
    [SerializeField] private DialogueState dialogueState;

    public DialogueState State => dialogueState;

    public enum DialogueState { NotOnDialogue, DialogueTransitionIn, DialogueTransitionOut, Idle, SentenceTransitionIn, SentenceTransitionOut }

    public class OnDialogueEventArgs : EventArgs
    {

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

    private IEnumerator DialogueCoroutine()
    {
        yield return null;
    }
}
