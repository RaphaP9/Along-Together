using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealFeedbackUI : NumericFeedbackUI
{
    private const string PLUS_CHARACTER = "+";

    public override void SetNumericFeedback(int value, Color textColor)
    {
        SetText(PLUS_CHARACTER + value.ToString());
        SetTextColor(textColor);
    }
}
