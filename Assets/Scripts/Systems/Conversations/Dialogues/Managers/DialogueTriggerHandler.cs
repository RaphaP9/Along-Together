using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerHandler : MonoBehaviour
{
    public static DialogueTriggerHandler Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<DialogueGroup> dialogueGroups;

    [Header("Debug")]
    [SerializeField] private bool debug;

    #region Public Methods
    public bool ExistDialogueWithConditions(CharacterSO characterSO, int stageNumber, int roundNumber, DialogueChronology dialogueChronology)
    {
        foreach(DialogueGroup dialogueGroup in dialogueGroups)
        {
            if (!dialogueGroup.enabled) continue;
            if (dialogueGroup.onlyTutorializedRun && !GameManager.Instance.TutorializedRun) continue;

            if (dialogueGroup.characterSO != characterSO) continue;
            if (dialogueGroup.stageNumber != stageNumber) continue;
            if (dialogueGroup.roundNumber != roundNumber) continue;
            if (dialogueGroup.dialogueChronology != dialogueChronology) continue;

            return true;
        }

        return false;
    }

    public void PlayDialogueWithConditions(CharacterSO characterSO, int stageNumber, int roundNumber, DialogueChronology dialogueChronology)
    {
        DialogueSO dialogueSO = FindDialogueWithConditions(characterSO,stageNumber, roundNumber,dialogueChronology);

        if(dialogueSO == null)
        {
            if (debug) Debug.Log("DialogueSO was not found. Dialogue Play will be ignored.");
            return;
        }

        DialogueManager.Instance.StartDialogue(dialogueSO);
    }

    private DialogueSO FindDialogueWithConditions(CharacterSO characterSO, int stageNumber, int roundNumber, DialogueChronology dialogueChronology)
    {
        foreach (DialogueGroup dialogueGroup in dialogueGroups)
        {
            if (!dialogueGroup.enabled) continue;

            if (dialogueGroup.characterSO != characterSO) continue;
            if (dialogueGroup.stageNumber != stageNumber) continue;
            if (dialogueGroup.roundNumber != roundNumber) continue;
            if (dialogueGroup.dialogueChronology != dialogueChronology) continue;

            return dialogueGroup.dialogueSO;
        }

        return null;
    }
    #endregion

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one GameManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public class DialogueGroup
{
    public CharacterSO characterSO;
    public int stageNumber;
    public int roundNumber;
    public DialogueChronology dialogueChronology;
    public DialogueSO dialogueSO;
    [Space]
    public bool onlyTutorializedRun;
    public bool enabled;
}
