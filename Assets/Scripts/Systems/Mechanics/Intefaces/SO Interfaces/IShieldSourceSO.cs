using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShieldSourceSO
{
    public string GetShieldSourceName();
    public string GetShieldSourceDescription();
    public Sprite GetShieldSourceSprite();
    public Color GetShieldSourceColor();
}
