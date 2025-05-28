using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "NewVideoCinematicSO", menuName = "ScriptableObjects/Cinematics/VideoCinematic")]

public class VideCinematicSO : ScriptableObject
{
    public int id;
    public string cinematicName;
    public VideoClip videoClip;
}
