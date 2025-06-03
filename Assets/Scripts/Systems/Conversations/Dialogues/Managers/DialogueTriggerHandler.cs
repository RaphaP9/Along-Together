using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DialogueTriggerHandler : MonoBehaviour
{
    public static DialogueTriggerHandler Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<CharacterDialogueGroup> characterDialogueGroups;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<CharacterDialogueGroup> CharacterDialogues;

    #region Public Methods
    public bool ExistDialogueForCurrentCharacterAndStage()
    {
        DialogueSO dialogueSO = GetDialogueForCharacterAndStage(PlayerCharacterManager.Instance.CharacterSO, GeneralStagesManager.Instance.CurrentStageNumber);
        if(dialogueSO == null) return false;
        return true;
    }

    public void PlayDialogueForCurrentCharacterAndStage()
    {
        DialogueSO dialogueSO = GetDialogueForCharacterAndStage(PlayerCharacterManager.Instance.CharacterSO, GeneralStagesManager.Instance.CurrentStageNumber);
        if(dialogueSO == null) return;

        DialogueManager.Instance.StartDialogue(dialogueSO);
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

    #region Utility Methods
    private CharacterDialogueGroup GetCharacterDialoguesByCharacterSO(CharacterSO characterSO)
    {
        foreach (CharacterDialogueGroup characterDialogueGroup in characterDialogueGroups)
        {
            if (characterDialogueGroup.characterSO == characterSO) return characterDialogueGroup;
        }

        if (debug) Debug.Log($"Could not find CharacterDialogueGroup for CharacterSO: {characterSO.entityName}. Returning null.");
        return null;
    }

    private DialogueSO GetDialogueByCharacterDialogueGroupAndStage(CharacterDialogueGroup characterDialogueGroup, int stageNumber)
    {
        foreach (StageDialogue stageDialogue in characterDialogueGroup.stageDialogues)
        {
            if (stageDialogue.stageNumber == stageNumber) return stageDialogue.dialogueSO;
        }

        if (debug) Debug.Log($"Could not find DialogueSO for StageNumber: {stageNumber}. Returning null.");
        return null;
    }

    private DialogueSO GetDialogueForCharacterAndStage(CharacterSO characterSO, int stageNumber)
    {
        CharacterDialogueGroup characterDialogueGroup = GetCharacterDialoguesByCharacterSO(characterSO);
        if (characterDialogueGroup == null) return null;
        DialogueSO dialogueSO = GetDialogueByCharacterDialogueGroupAndStage(characterDialogueGroup, stageNumber);
        if (dialogueSO == null) return null;

        return dialogueSO;
    }
    #endregion
}

[System.Serializable]
public class CharacterDialogueGroup
{
    public CharacterSO characterSO;
    public List<StageDialogue> stageDialogues;
}

[System.Serializable]
public class StageDialogue
{
    public int stageNumber;
    public DialogueSO dialogueSO;
}