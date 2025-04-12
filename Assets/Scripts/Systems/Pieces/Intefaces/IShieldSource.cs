using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShieldSource
{
    public Color GetShieldColor();
    public string GetName();
    public string GetDescription();
    public Sprite GetSprite();
}
