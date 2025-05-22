using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntityGoldDropManager : MonoBehaviour
{
    public static EntityGoldDropManager Instance { get; private set; }

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

    private bool PlayerKilledEnemy(IDamageSourceSO damageSourceSO)
    {
        if(PlayerCharacterManager.Instance == null|| PlayerCharacterManager.Instance.CharacterSO)
        {
            if (debug) Debug.Log("Either PlayerCharacterManager or CharacterSO is null. Can not determine whether the enemy was killed by the player.");
            return false;
        }

        return false;
    }

    #region Subscriptions
    private void EnemyHealth_OnAnyEnemyDeath(object sender, EventArgs e)
    {

    }
    #endregion
}
