using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currentHealth;
    public int maxHealth;

    public int currentShield;
    public int maxShield;

    public int armor;
    public float dodgeChance;

    public PlayerData()
    {
        currentHealth = 0;
        maxHealth = 0;

        currentShield = 0;
        maxShield = 0;

        armor = 0;
        dodgeChance = 0f;
    }
}
