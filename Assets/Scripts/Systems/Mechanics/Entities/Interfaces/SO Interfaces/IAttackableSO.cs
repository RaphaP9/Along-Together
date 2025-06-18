using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackableSO
{
    public Color GetAttackableColor();
    public string GetAttackableName();
    public string GetAttackableDescription();
    public Sprite GetAttackableSprite();
}
