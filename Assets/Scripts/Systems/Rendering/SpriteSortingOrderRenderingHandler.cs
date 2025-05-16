using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortingOrderRenderingHandler : SortingOrderRenderingHandler
{
    [Header("Canvas Components")]
    [SerializeField] private List<SpriteRenderer> spriteRenderers;

    protected override void UpdateSortingOrder(int sortingOrder)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sortingOrder = sortingOrder;
        }
    }
}
