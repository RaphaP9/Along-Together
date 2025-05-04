using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOreSourceSO
{
    public Color GetOreSourceColor();
    public string GetOreSourceName();
    public string GetOreSourceDescription();
    public Sprite GetOreSourceSprite();
}
