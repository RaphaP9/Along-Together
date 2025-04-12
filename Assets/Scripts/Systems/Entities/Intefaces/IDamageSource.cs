using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageSource 
{
    public Color GetDamageColor();
    public string GetName();
    public string GetDescription();
    public Sprite GetSprite();
}
