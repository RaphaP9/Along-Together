using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopObjectCardStatUI : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private NumericEmbeddedStat numericEmbeddedStat;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [Header("Settings")]
    [SerializeField] protected Color positiveColor;
    [SerializeField] protected Color neutralColor;
    [SerializeField] protected Color negativeColor;

    private void SetNumericEmbededStat(NumericEmbeddedStat numericEmbeddedStat)
    {

    }
}
