using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleVideoCinematicTransitionUINotifier : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VideoCinematicUI videoCinematicUI;

    public void NotifyTransitionInEnd() => videoCinematicUI.TransitionInEnd();
    public void NotifyTransitionOutEnd() => videoCinematicUI.TransitionOutEnd();
}
