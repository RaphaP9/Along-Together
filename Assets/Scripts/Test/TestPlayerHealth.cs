using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerHealth : MonoBehaviour
{
    [Header("Components - Assign On Runtime!")]
    [SerializeField] private PlayerHealth playerHealth;

    private void Update()
    {
        TestHeal();
    }

    private void TestHeal()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            playerHealth.Heal(new HealData { healAmount = 2, healSource = null});
        }
    }
}
