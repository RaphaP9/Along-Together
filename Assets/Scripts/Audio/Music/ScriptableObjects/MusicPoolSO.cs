using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSFXPoolSO", menuName = "ScriptableObjects/Audio/MusicPool")]
public class MusicPoolSO : ScriptableObject
{
    [Header("Scenes")]
    public AudioClip menuMusic;
    public AudioClip optionsMusic;
    public AudioClip creditsMusic;

    [Header("Delice")]
    public AudioClip Delice_Stage1;
    public AudioClip Delice_Stage2;
    public AudioClip Delice_Stage3;
    public AudioClip Delice_Stage4;
    public AudioClip Delice_Stage5;
}
