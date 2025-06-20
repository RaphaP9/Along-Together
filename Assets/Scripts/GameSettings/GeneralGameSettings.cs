using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralGameSettings : MonoBehaviour
{
    public static GeneralGameSettings Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private List<CharacterSO> startingUnlockedCharacters;
    [Space]
    [SerializeField] private int startingStage;
    [SerializeField] private int startingRound;
    [Space]
    [SerializeField] private int startingGoldQuantity;
    [Space]
    [SerializeField] private CharacterSO defaultCharacter;

    #region Initialization
    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Debug.LogWarning("There is more than one GeneralGameSettings instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    #endregion

    public int GetStartingGoldQuantity() => startingGoldQuantity;
    public int GetStartingStage() => startingStage;
    public int GetStartingRound() => startingRound;
    public int GetDefaultCharacterID() => defaultCharacter.id;

    public List<int> GetStartingUnlockedCharacterIDs()
    {
        List<int> IDs = new List<int>();

        foreach (CharacterSO character in startingUnlockedCharacters)
        {
            IDs.Add(character.id);
        }

        return IDs;
    }
}
