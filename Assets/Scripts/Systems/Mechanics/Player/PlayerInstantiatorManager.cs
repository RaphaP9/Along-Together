using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstantiatorManager : MonoBehaviour
{
    [Header("Default Settings")]
    [SerializeField] private CharacterSO defaultCharacterSO;
    [SerializeField] private Vector2Int defaultPosition;

    [Header("Settings - Runtime Filled")]
    [SerializeField] private CharacterSO characterSO;
    [SerializeField] private Vector2Int position;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public CharacterSO CharacterSO => characterSO;
    public Vector2Int Position => position;

    private void Start()
    {
        InstantiatePlayer();
    }

    private void InstantiatePlayer()
    {
        if(characterSO == null)
        {
            characterSO = defaultCharacterSO;
            if (debug) Debug.Log("CharacterSO is null. Loading Default Character");
        }

        Transform instantiatedCharacter = Instantiate(characterSO.characterPrefab, GeneralUtilities.Vector2IntToVector3(position), Quaternion.identity); 
    }

    public void SetCharacterSO(CharacterSO setterCharacterSO) => characterSO = setterCharacterSO;
    public void SetPosition(Vector2Int setterPosition) => position = setterPosition;
}
