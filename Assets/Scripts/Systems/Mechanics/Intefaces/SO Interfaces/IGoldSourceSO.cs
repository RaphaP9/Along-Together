using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoldSourceSO
{
    public Color GetGoldSourceColor();
    public string GetGoldSourceName();
    public string GetGoldSourceDescription();
    public Sprite GetGoldSourceSprite();
}
