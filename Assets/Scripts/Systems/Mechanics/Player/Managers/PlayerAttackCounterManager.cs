using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCounterManager : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private int attacksPerformed;

    public int AttacksPerformed => attacksPerformed;

    private void OnEnable()
    {
        PlayerAttack.OnAnyPlayerAttack += PlayerAttack_OnAnyPlayerAttack;
    }

    private void OnDisable()
    {
        PlayerAttack.OnAnyPlayerAttack -= PlayerAttack_OnAnyPlayerAttack;
    }

    private void IncreaseAttacksPerformed(int quantity) => attacksPerformed += quantity;

    private void PlayerAttack_OnAnyPlayerAttack(object sender, PlayerAttack.OnPlayerAttackEventArgs e)
    {
        IncreaseAttacksPerformed(1);
    }
}
