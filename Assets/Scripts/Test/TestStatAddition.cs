using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStatAddition : MonoBehaviour, IHasEmbeddedStats
{
    [Header("Components")]
    [SerializeField] private NumericStatModifierManager numericStatModifierManager;

    [Header("Settings")]
    [SerializeField] private List<NumericEmbeddedStat> numericEmbeddedStats;

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            numericStatModifierManager.AddStatModifiers("TestGUID", this);
        }
    }

    public List<NumericEmbeddedStat> GetNumericEmbeddedStats()
    {
        return numericEmbeddedStats;
    }

    public List<AssetEmbeddedStat> GetAssetEmbeddedStats()
    {
        return new List<AssetEmbeddedStat>();
    }
}
