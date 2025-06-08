using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUpgradeCardsGenerator : MonoBehaviour
{
    public static AbilityUpgradeCardsGenerator Instance { get; private set; }

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void Awake()
    {
        SetSingleton();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            List<AbilityCardUpgradeInfo> abilityCardUpgradeInfos = GenerateNextLevelActiveAbilityVariantCards();

            foreach(AbilityCardUpgradeInfo abilityCardUpgradeInfo in abilityCardUpgradeInfos)
            {
                Debug.Log($"AbilitySO: {abilityCardUpgradeInfo.abilitySO}, CurrentLevel: {abilityCardUpgradeInfo.currentLevel}, UpgradeLevel: {abilityCardUpgradeInfo.upgradeLevel}");
            }
        }
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<AbilityCardUpgradeInfo> GenerateNextLevelActiveAbilityVariantCards()
    {
        List<AbilityCardUpgradeInfo> activeAbilityVariantCards = new List<AbilityCardUpgradeInfo>();

        if (CharacterAbilitySlotsRegister.Instance == null)
        {
            if (debug) Debug.Log("CharacterAbilitySlotsRegister instance is null. Returning empty list");
            return activeAbilityVariantCards;
        }

        foreach(AbilitySlotHandler abilitySlotHandler in CharacterAbilitySlotsRegister.Instance.AbilitySlotHandlers)
        {
            AbilityCardUpgradeInfo abilityCardUpgradeInfo = GenerateNextLevelCard(abilitySlotHandler.ActiveAbilityVariant);
            if (abilityCardUpgradeInfo == null) continue;

            activeAbilityVariantCards.Add(abilityCardUpgradeInfo);
        }

        return activeAbilityVariantCards;
    }

    public AbilityCardUpgradeInfo GenerateNextLevelCard(Ability ability)
    {
        if(ability == null)
        {
            if (debug) Debug.Log($"Ability is null.");
            return null;
        }

        if (ability.AbilityLevelHandler.IsMaxedOut())
        {
            if (debug) Debug.Log($"Ability with name: {ability.AbilitySO.abilityName} is maxed out.");
            return null;
        }

        AbilityLevel currentLevel = ability.AbilityLevelHandler.AbilityLevel;
        AbilityLevel upgradeLevel = MechanicsUtilities.GetNextAbilityLevel(currentLevel);
        AbilitySO abilitySO = ability.AbilitySO;

        AbilityCardUpgradeInfo abilityCardUpgradeInfo = new AbilityCardUpgradeInfo { currentLevel = currentLevel, upgradeLevel = upgradeLevel, abilitySO = abilitySO };
        return abilityCardUpgradeInfo;
    }
}
