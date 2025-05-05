using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterManager : MonoBehaviour
{
    public static PlayerCharacterManager Instance {  get; private set; }

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

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InstantiatePlayer();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InstantiatePlayer()
    {
        if(characterSO == null)
        {
            if (debug) Debug.Log("CharacterSO is null. Can not instantiate character.");
            return;
        }

        Transform instantiatedCharacter = Instantiate(characterSO.prefab, GeneralUtilities.Vector2IntToVector3(position), Quaternion.identity); 
    }

    public void SetCharacterSO(CharacterSO setterCharacterSO)
    {
        if(setterCharacterSO == null)
        {
            characterSO = defaultCharacterSO;
            if (debug) Debug.Log("CharacterSO is null. Setting Default Character.");
            return;
        }

        characterSO = setterCharacterSO;
        if (debug) Debug.Log($"CharacterSO set as: {characterSO.name}");
    }

    public void SetPosition(Vector2Int setterPosition) => position = setterPosition;
}
