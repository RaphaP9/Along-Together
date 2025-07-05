using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitardandoVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform ritardandoVFXPrefab;
    [Space]
    [SerializeField] private Transform facingDirectioner;
    [SerializeField] private Ritardando ritardando;

    private void OnEnable()
    {
        ritardando.OnRitardandoPerformanceEnd += Ritardando_OnRitardandoPerformanceEnd;
    }

    private void OnDisable()
    {
        ritardando.OnRitardandoPerformanceEnd -= Ritardando_OnRitardandoPerformanceEnd;
    }

    private void SpawnVFX()
    {
        Transform VFXTransform = Instantiate(ritardandoVFXPrefab, facingDirectioner.position, facingDirectioner.rotation);
    }

    private void Ritardando_OnRitardandoPerformanceEnd(object sender, System.EventArgs e)
    {
        SpawnVFX();
    }
}
