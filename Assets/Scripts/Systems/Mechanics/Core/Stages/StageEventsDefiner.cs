using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEventsDefiner : MonoBehaviour
{
    public static StageEventsDefiner Instance {  get; private set; }

    private const int SECOND_ROUND = 2;
    private const int FIRST_STAGE = 1;

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one StageEventsDefiner instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public bool OpenShopOnThisRound()
    {
        if (GeneralStagesManager.Instance.CurrentStageAndRoundAreFirsts()) return false; //No Shop On 1-1
        if (GeneralStagesManager.Instance.CurrentStageAndRoundAreValues(FIRST_STAGE, SECOND_ROUND)) return false; //No Shop on 1-2 (Ability Upgrade)
        if (GeneralStagesManager.Instance.CurrentRoundIsFirstFromCurrentStage()) //If X-1 and can generate cards, do not open shop
        {
            if (AbilityUpgradeCardsGenerator.Instance.CanGenerateNextLevelActiveAbilityVariantCards()) return false;
        }

        return true;
    }

    public bool OpenAbilityUpgradeThisRound()
    {
        if (GeneralStagesManager.Instance.CurrentStageAndRoundAreFirsts()) return false; //No AbilityUpgrade On First 1-1

        if (GeneralStagesManager.Instance.CurrentStageAndRoundAreValues(FIRST_STAGE, SECOND_ROUND)) return true; //Ability Upgrade on 1-2
        if (GeneralStagesManager.Instance.CurrentRoundIsFirstFromCurrentStage()) //If X-1 can generate cards, open Ability Upgrade
        {
            if (AbilityUpgradeCardsGenerator.Instance.CanGenerateNextLevelActiveAbilityVariantCards()) return true;
        }

        return false;
    }
}
