using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthStatModificationManager : StatModificationManager
{
    [Header("Lists")]
    [SerializeField] private List<MaxHealthStatModifier> valueStatModifiers;
    [SerializeField] private List<MaxHealthStatModifier> percentageStatModifiers;
    [SerializeField] private List<MaxHealthStatModifier> replacementStatModifiers;

    public List<MaxHealthStatModifier> ValueStatModifiers => valueStatModifiers;
    public List<MaxHealthStatModifier> PercentageStatModifiers => percentageStatModifiers;
    public List<MaxHealthStatModifier> ReplacementStatModifiers => replacementStatModifiers;
}
