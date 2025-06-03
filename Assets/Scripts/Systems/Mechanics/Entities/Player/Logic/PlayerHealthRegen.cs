using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthRegen : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterIdentifier characterIdentifier;
    [SerializeField] private PlayerHealth playerHealth;

    private void OnEnable()
    {
        GameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= GameManager_OnStateChanged;

    }

    private void HealFromHealthRegen()
    {
        HealData healData = new HealData(HealthRegenStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseHealthRegen), characterIdentifier.CharacterSO);
        playerHealth.Heal(healData);
    }

    private void RestoreShieldFromShieldRegen()
    {
        ShieldData shieldData = new ShieldData(ShieldRegenStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseShieldRegen), characterIdentifier.CharacterSO);
        playerHealth.RestoreShield(shieldData);
    }

    private void GameManager_OnStateChanged(object sender, GameManager.OnStateChangeEventArgs e)
    {
        if (e.newState != GameManager.State.BeginningCombat) return;

        HealFromHealthRegen();
        RestoreShieldFromShieldRegen();
    }
}
