using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUpgradeCardContentsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbilityUpgradeCardUI abilityUpgradeCardUI;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI abilityNameText;
    [SerializeField] private Image abilityImage;
    [SerializeField] private TextMeshProUGUI upgradeLevelText;

    private void OnEnable()
    {
        abilityUpgradeCardUI.OnAbilityUpgradeCardSet += AbilityUpgradeCardUI_OnAbilityUpgradeCardSet;
    }

    private void OnDisable()
    {
        abilityUpgradeCardUI.OnAbilityUpgradeCardSet -= AbilityUpgradeCardUI_OnAbilityUpgradeCardSet;
    }

    private void SetAbilityNameText(string text) => abilityNameText.text = text;
    private void SetUpgradeLevelText(string text) => upgradeLevelText.text = text;
    private void SetAbilitySprite(Sprite sprite) => abilityImage.sprite = sprite;

    private void CompleteSetUI(AbilityUpgradeCardInfo abilityUpgradeCardInfo)
    {
        SetAbilityNameText(abilityUpgradeCardInfo.abilitySO.abilityName);
        SetUpgradeLevelText(MappingUtilities.MapAbilityLevel(abilityUpgradeCardInfo.upgradeLevel));
        SetAbilitySprite(abilityUpgradeCardInfo.abilitySO.sprite);
    }

    private void AbilityUpgradeCardUI_OnAbilityUpgradeCardSet(object sender, AbilityUpgradeCardUI.OnAbilityUpgradeCardEventArgs e)
    {
        CompleteSetUI(e.abilityUpgradeCardInfo);
    }
}
