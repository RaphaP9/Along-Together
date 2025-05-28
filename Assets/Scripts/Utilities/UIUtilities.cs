using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public static class UIUtilities
{
   

    public static void SetCanvasGroupAlpha(CanvasGroup canvasGroup, float alpha) => canvasGroup.alpha = alpha;
    public static void SetImageFillRatio(Image image, float fillRatio) => image.fillAmount = fillRatio;
    public static void SetImageColor(Image image, Color color) => image.color = color;
    public static void SetImagesColor(List<Image> images, Color color)
    {
        foreach (Image image in images)
        {
            SetImageColor(image, color);
        }
    }
}
