using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopObjectCardStatUI : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private NumericEmbeddedStat numericEmbeddedStat;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI typeText;

    private void SetNumericEmbededStat(NumericEmbeddedStat numericEmbeddedStat)
    {

    }
}
