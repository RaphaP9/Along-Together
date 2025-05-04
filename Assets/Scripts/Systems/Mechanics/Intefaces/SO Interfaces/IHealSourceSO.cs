using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealSourceSO
{
    public string GetHealSourceName();
    public string GetHealSourceDescription();
    public Sprite GetHealSourceSprite();
    public Color GetHealSourceColor();
}
