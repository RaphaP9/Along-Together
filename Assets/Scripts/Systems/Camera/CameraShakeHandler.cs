using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeHandler : MonoBehaviour
{
    public static CameraShakeHandler Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [Header("Settings")]
    [SerializeField] private ShakeReplacementCondition shakeReplacementCondition;

    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private float currentShakeAmplitude;

    private enum ShakeReplacementCondition { AnyShake, OnlyGreaterAmplitudes, WaitForCurrentShakeEnd}

    private void Awake()
    {
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        SetSingleton();
    }

    private void Start()
    {
        SetCurrentShakeAmplitude(0f);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one CameraShakeHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void ShakeCamera(float amplitude, float frequency, float shakeTime, float fadeInTime, float fadeOutTime)
    {
        if (shakeReplacementCondition == ShakeReplacementCondition.WaitForCurrentShakeEnd && currentShakeAmplitude != 0) return;
        if (shakeReplacementCondition == ShakeReplacementCondition.OnlyGreaterAmplitudes && currentShakeAmplitude > amplitude) return;

        StopAllCoroutines();
        StartCoroutine(ShakeCameraCoroutine(amplitude, frequency, shakeTime, fadeInTime, fadeOutTime));
    }

    private IEnumerator ShakeCameraCoroutine(float amplitude, float frequency, float shakeTime, float fadeInTime, float fadeOutTime)
    {
        SetCurrentShakeAmplitude(amplitude);

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0f;

        float time = 0f;

        while (time <= fadeInTime)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(0f, amplitude, time / fadeInTime);
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(0f, frequency, time / fadeInTime);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequency;

        yield return new WaitForSecondsRealtime(shakeTime);

        time = 0f;

        while (time <= fadeOutTime)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(amplitude, 0f, time / fadeOutTime);
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(frequency, 0f, time / fadeOutTime);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0f;

        ClearCurrentShakeAmplitude();
    }

    private void SetCurrentShakeAmplitude(float value) => currentShakeAmplitude = value;
    private void ClearCurrentShakeAmplitude() => currentShakeAmplitude = 0f;
}