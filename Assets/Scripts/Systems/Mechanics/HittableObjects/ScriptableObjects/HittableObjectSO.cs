using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHittableObjectSO", menuName = "ScriptableObjects/HittableObjects/HittableObject")]
public class HittableObjectSO : ScriptableObject
{
    [Header("Hittable Object Identifiers")]
    public int id;
    public string hittableObjectName;
    [TextArea(3, 10)] public string description;
    public Sprite sprite;

    [Header("Entity Stats")]
    [Range(0, 5)] public int health;
    [Range(0, 5)] public int shield;
}
