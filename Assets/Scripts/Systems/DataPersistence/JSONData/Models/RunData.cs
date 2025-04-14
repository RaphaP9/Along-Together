using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunData
{
    public int roomID;

    public int characterID;

    public int currentHealth;
    public int currentShield;
    public int armor;
    public float dodgeChance;

    public RunData()
    {
        roomID = 1;

        currentHealth = 0;

        currentShield = 0;

        armor = 0;
        dodgeChance = 0f;
    }
}
