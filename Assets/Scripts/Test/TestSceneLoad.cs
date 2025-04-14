using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneLoad : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string scene;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ScenesManager.Instance.FadeLoadTargetScene(scene);
        }
    }
}
