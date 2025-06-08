using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUpgradeCardsUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Transform abilityUpgradeCardsContainer;
    [SerializeField] private Transform abilityUpgradeCardPrefab;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        AbilityUpgradeManager.OnAbilityUpgradesGenerated += AbilityUpgradeManager_OnAbilityUpgradesGenerated;
    }

    private void OnDisable()
    {
        AbilityUpgradeManager.OnAbilityUpgradesGenerated -= AbilityUpgradeManager_OnAbilityUpgradesGenerated;
    }

    private void GenerateNewAbilityUpgradeCards(List<AbilityUpgradeCardInfo> abilityUpgradeCardInfos)
    {
        ClearAbilityUpgradeCardsContainer();

        foreach (AbilityUpgradeCardInfo abilityUpgradeCardInfo in abilityUpgradeCardInfos)
        {
            CreateAbilityUpgradeCard(abilityUpgradeCardInfo);
        }
    }

    private void ClearAbilityUpgradeCardsContainer()
    {
        foreach (Transform child in abilityUpgradeCardsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateAbilityUpgradeCard(AbilityUpgradeCardInfo abilityUpgradeCardInfo)
    {
        Transform abilityUpgradeCardUITransform = Instantiate(abilityUpgradeCardPrefab, abilityUpgradeCardsContainer);

        AbilityUpgradeCardUI abilityUpgradeCardUI = abilityUpgradeCardUITransform.GetComponent<AbilityUpgradeCardUI>();

        if (abilityUpgradeCardUI == null)
        {
            if (debug) Debug.Log("Instantiated Ability Upgrade Card does not contain a AbilityUpgradeCardUI component. Set will be ignored.");
            return;
        }

        abilityUpgradeCardUI.SetAbilityUpgradeCardInfo(abilityUpgradeCardInfo);
    }

    private void AbilityUpgradeManager_OnAbilityUpgradesGenerated(object sender, AbilityUpgradeManager.OnAbilityUpgradesEventArgs e)
    {
        GenerateNewAbilityUpgradeCards(e.abilityUpgradeCardInfos);
    }
}
