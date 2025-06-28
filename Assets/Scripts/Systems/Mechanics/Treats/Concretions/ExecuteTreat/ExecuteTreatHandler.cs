using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteTreatHandler : TreatHandler
{
    public static ExecuteTreatHandler Instance { get; private set; }

    private ExecuteTreatConfigSO ExecuteTreatConfigSO => treatConfigSO as ExecuteTreatConfigSO;

    public static event EventHandler<OnExecuteTreatExecutionEventArgs> OnExecuteTreatExecution; 

    public class OnExecuteTreatExecutionEventArgs : EventArgs
    {
        public EnemyHealth enemyHealth;
        public int healthEnemyHadToExecute;
    }

    private void OnEnable()
    {
        EnemyHealth.OnAnyEnemyHealthTakeDamage += EnemyHealth_OnAnyEnemyHealthTakeDamage;
    }

    private void OnDisable()
    {
        EnemyHealth.OnAnyEnemyHealthTakeDamage -= EnemyHealth_OnAnyEnemyHealthTakeDamage;
    }

    protected override void SetSingleton()
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

    private void HandleEnemyExecution(EnemyHealth enemyHealth, int resultingDamageHealth)
    {
        if (!enemyHealth.IsAlive()) return; //Do not execute if already dead
        if (resultingDamageHealth > ExecuteTreatConfigSO.healthExecuteThreshold) return; //Do not execute if above threshold

        ExecuteDamageData executeDamageData = new ExecuteDamageData(true, ExecuteTreatConfigSO, true, true);

        enemyHealth.Execute(executeDamageData);
        OnExecuteTreatExecution?.Invoke(this, new OnExecuteTreatExecutionEventArgs { enemyHealth = enemyHealth, healthEnemyHadToExecute = resultingDamageHealth });
    }

    private void EnemyHealth_OnAnyEnemyHealthTakeDamage(object sender, EntityHealth.OnEntityHealthTakeDamageEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return; //In this treat condition is always true

        HandleEnemyExecution(sender as EnemyHealth, e.newHealth);
    }
}
