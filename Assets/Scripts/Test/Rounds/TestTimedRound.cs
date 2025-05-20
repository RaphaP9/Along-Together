using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTimedRound : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TimedRoundSO timedRoundSO;

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            TimedRoundHandler.Instance.StartTimedRound(timedRoundSO);
        }
    }
}
