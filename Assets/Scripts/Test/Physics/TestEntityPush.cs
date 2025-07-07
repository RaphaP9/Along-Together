using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntityPush : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PhysicPushData physicPushData;
    [SerializeField] private LayerMask pushLayerMask;
    [Space]
    [SerializeField] private Transform playerTransform;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            MechanicsUtilities.PushAllEntitiesFromPoint(GeneralUtilities.TransformPositionVector2(playerTransform), physicPushData, pushLayerMask, new List<Transform> { playerTransform});
        }
    }
}
