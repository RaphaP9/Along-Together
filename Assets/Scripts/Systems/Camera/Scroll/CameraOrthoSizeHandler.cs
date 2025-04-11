using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraOrthoSizeHandler : MonoBehaviour
{
    public static CameraOrthoSizeHandler Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera CMVCam;

    public float OrthoSizeDefault { get; private set; }

    public float Distance { get; private set; }

    private float desiredDistance;
    private float smoothInput;

    private float refVelocity;

    public float ScrollFactor { get; private set; }

    private void Awake()
    {
        SetSingleton();
        SetOrthoSizeRefferences();
    }

    private void LateUpdate()
    {
        ApplyDistance();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one CameraScroll instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetOrthoSizeRefferences()
    {
        OrthoSizeDefault = CMVCam.m_Lens.OrthographicSize;
        Distance = OrthoSizeDefault;
    }

    private void ApplyDistance() => CMVCam.m_Lens.OrthographicSize = Distance;

    //For CameraFollowHandler
    public void LerpTowardsTargetDistance(float desiredDistance, float smoothFactor) => Distance = Mathf.Lerp(Distance, desiredDistance, smoothFactor * Time.deltaTime);
    public void SetTargetDistance(float desiredDistance) => Distance = desiredDistance;
}   
