using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStageConfinerHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera CMVCAM;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        GeneralStagesManager.OnStageInitialized += GeneralStagesManager_OnStageInitialized;
        GeneralStagesManager.OnStageChange += GeneralStagesManager_OnStageChange;
    }

    private void OnDisable()
    {
        GeneralStagesManager.OnStageInitialized -= GeneralStagesManager_OnStageInitialized;
        GeneralStagesManager.OnStageChange -= GeneralStagesManager_OnStageChange;
    }

    private void SwitchConfiner(PolygonCollider2D confiner)
    {
        CinemachineConfiner2D cinemachineConfiner2D = CMVCAM.GetComponent<CinemachineConfiner2D>();
        
        if(cinemachineConfiner2D == null)
        {
            if (debug) Debug.Log("CinemachineConfiner2D is null. Confiner switch will be ignored");
            return;
        }

        cinemachineConfiner2D.m_BoundingShape2D = confiner;
        cinemachineConfiner2D.enabled = false; //Force Reinitialization 
        cinemachineConfiner2D.enabled = true;
    }

    private void GeneralStagesManager_OnStageInitialized(object sender, GeneralStagesManager.OnStageChangeEventArgs e)
    {
        SwitchConfiner(e.stageGroup.stageConfiner);
    }

    private void GeneralStagesManager_OnStageChange(object sender, GeneralStagesManager.OnStageChangeEventArgs e)
    {
        SwitchConfiner(e.stageGroup.stageConfiner);
    }
}
