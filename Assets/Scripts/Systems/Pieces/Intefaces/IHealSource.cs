using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealSource
{
    public Color GetHealColor();
    public string GetName();
    public string GetDescription();
    public Sprite GetSprite();
}
