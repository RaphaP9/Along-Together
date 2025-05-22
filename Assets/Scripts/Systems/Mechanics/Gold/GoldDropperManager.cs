using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoldDropperManager : MonoBehaviour
{
    public static GoldDropperManager Instance { get; private set; }

    public static event EventHandler<OnEntityDropGoldEventArgs> OnEntityDropGold;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public class OnEntityDropGoldEventArgs : EventArgs
    {
        public int goldAmount;
        public Vector2 entityPosition;
    }        

    private void OnEnable()
    {
        EnemyHealth.OnAnyEnemyDeath += EnemyHealth_OnAnyEnemyDeath;
    }

    private void OnDisable()
    {
        EnemyHealth.OnAnyEnemyDeath -= EnemyHealth_OnAnyEnemyDeath;
    }

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
            Debug.LogWarning("There is more than one EntityGoldDropManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void DropEntityGoldAtPosition(int goldAmount, Vector2 entityPosition)
    {
        if (goldAmount <= 0) return;

        int goldDropped = GoldManager.Instance.AddGold(goldAmount);

        OnEntityDropGold?.Invoke(this, new OnEntityDropGoldEventArgs { goldAmount = goldDropped, entityPosition = entityPosition });

        Debug.Log($"{goldDropped} gold dropped at {entityPosition}");
    }

    private bool PlayerIsDamageSource(IDamageSourceSO damageSource)
    {
        if (GetPlayerCharacterSO() == null) return false;
        if(GetPlayerCharacterSO().entityName != damageSource.GetDamageSourceName()) return false;

        return true;
    }

    private CharacterSO GetPlayerCharacterSO()
    {
        if (PlayerCharacterManager.Instance == null || PlayerCharacterManager.Instance.CharacterSO == null)
        {
            if (debug) Debug.Log("Either PlayerCharacterManager or CharacterSO is null. Can not get CharacterSO.");
            return null;
        }

        return PlayerCharacterManager.Instance.CharacterSO;
    }

    #region Subscriptions
    private void EnemyHealth_OnAnyEnemyDeath(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        if (!PlayerIsDamageSource(e.damageSource)) return;

        int goldAmount = (e.entitySO as EnemySO).goldDrop;
        Vector2 position = GeneralUtilities.SupressZComponent((sender as EntityHealth).transform.position);

        DropEntityGoldAtPosition(goldAmount, position);
    }

    #endregion
}
