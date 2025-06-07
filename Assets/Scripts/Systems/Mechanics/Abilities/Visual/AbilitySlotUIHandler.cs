using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilitySlotUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbilitySlotHandler abilitySlotHandler;

    [Header("UIComponents")]
    [SerializeField] private Transform cooldownPanelTransform;
    [SerializeField] private TextMeshProUGUI cooldownText;
}
