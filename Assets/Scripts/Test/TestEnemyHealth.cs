using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyHealth : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyHealth enemyHealth;

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            enemyHealth.TakeDamage(new DamageData { damage = 4, isCrit = true, damageSource = null });
        }
    }
}
