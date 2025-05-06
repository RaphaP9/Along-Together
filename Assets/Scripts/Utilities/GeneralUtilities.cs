using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GeneralUtilities
{
    private const bool DEBUG = true;

    #region Vectors
    public static Vector2 SupressZComponent(Vector3 vector3) => new Vector2(vector3.x, vector3.y);

    public static Vector2Int Vector2ToVector2Int(Vector2 vector2)
    {
        Vector2Int vector2Int = new Vector2Int(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
        return vector2Int;
    }

    public static float GetVector2AngleDegrees(Vector2 vector2) => Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;
    public static Vector2 GetAngleDegreesVector2(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        Vector2 vector =  new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        vector.Normalize();
        return vector;
    }

    public static Vector3 Vector2ToVector3(Vector2 vector2) => new Vector3(vector2.x, vector2.y, 0f );

    public static Vector2 RotateVector2ByAngleDegrees(Vector2 vector, float angleDegrees)
    {
        float magnitude = vector.magnitude;

        float angleRadians = angleDegrees * Mathf.Deg2Rad;

        float rotationSin = Mathf.Sin(angleRadians);
        float rotationCos = Mathf.Cos(angleRadians);

        Vector2 rotatedVector = new Vector2(vector.x * rotationCos - vector.y * rotationSin, vector.x * rotationSin + vector.y * rotationCos);
        rotatedVector.Normalize();

        Vector2 finalVector = rotatedVector * magnitude;

        return finalVector;

    }
    #endregion

    #region VectorInts

    public static Vector3 Vector2IntToVector3(Vector2Int vector2) => new Vector3(vector2.x, vector2.y, 0f);

    public static bool CheckVectorIntsAreSameDirection(Vector2Int vectorA, Vector2Int vectorB)
    {
        if(vectorA == Vector2Int.zero) return false;
        if(vectorB == Vector2Int.zero) return false;

        if(!CheckVectorIntsCollinear(vectorA, vectorB)) return false;
        if(!CheckVectorIntsSameOrientation(vectorA, vectorB)) return false;

        return true;
    }

    public static bool CheckVectorIntsCollinear(Vector2Int vectorA, Vector2Int vectorB)
    {
        if (vectorA.x * vectorB.y == vectorA.y * vectorB.x) return true;
        
        return false;
    }

    public static bool CheckVectorIntsSameOrientation(Vector2Int vectorA, Vector2Int vectorB) //Same sing in X and Y
    {
        if (vectorA.x * vectorB.x + vectorA.y * vectorB.y > 0) return true;

        return false;
    }
    #endregion

    #region Floats
    public static float RoundToNDecimalPlaces(float number, int decimalPlaces) => Mathf.Round(number * Mathf.Pow(10, decimalPlaces)) / Mathf.Pow(10, decimalPlaces);

    public static float ClampNumber01(float number) => Mathf.Clamp01(number);

    #endregion

    #region Transforms
    public static Vector2 TransformPositionVector2(Transform transform) => new Vector2(transform.position.x, transform.position.y);

    public static void RotateTransformTowardsVector2(Transform transform, Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public static List<Vector2> TransformPositionVector2List(List<Transform> transforms)
    {
        List<Vector2> vectorList = new List<Vector2>();

        foreach (Transform transform in transforms)
        {
            vectorList.Add(TransformPositionVector2(transform));
        }

        return vectorList;
    }

    public static List<Transform> GetTransformsByColliders(Collider2D[] colliders)
    {
        List<Transform> transforms = new List<Transform>();

        foreach (Collider2D collider in colliders)
        {
            Transform transform = GetTransformByCollider(collider);

            transforms.Add(transform);
        }

        return transforms;
    }

    public static Transform GetTransformByCollider(Collider2D collider) => collider.transform;
    #endregion

    #region LayerMasks
    public static bool CheckGameObjectInLayerMask(GameObject gameObject, LayerMask layerMask) => ((1 << gameObject.layer) & layerMask) != 0;
    #endregion

    #region Lists

    public static T ChooseRandomElementFromList<T>(List<T> elementsList) where T : class
    {
        if (elementsList.Count <= 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, elementsList.Count);
        return elementsList[randomIndex];
    }

    public static List<T> FisherYatesShuffle<T>(List<T> list)
    {
        List<T> shuffledList = list;

        System.Random random = new System.Random();

        int n = shuffledList.Count;

        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (shuffledList[n], shuffledList[k]) = (shuffledList[k], shuffledList[n]);
        }

        return shuffledList;
    }

    public static List<T> AppendListsOfLists<T>(List<List<T>> listOfLists)
    {
        List<T> appendedList = new List<T>();

        foreach (List<T> list in listOfLists)
        {
            appendedList.AddRange(list);
        }

        return appendedList;
    }

    public static bool ListsAreExactlyEqual<T>(List<T> listA, List<T> listB)
    {
        return listA.SequenceEqual(listB);
    }

    #endregion

    #region Generics

    public static List<T> GetGenericsFromComponents<T>(List<Component> components)
    {
        List<T> genericList = new List<T>();

        foreach (Component component in components)
        {      
            if(component.TryGetComponent(out T generic))
            {
                genericList.Add(generic);
            }
        }

        return genericList;
    }
    #endregion

    #region Interfaces
    public static List<T> GetInterfacesFromComponents<T>(List<Component> components)
    {
        List<T> interfaceList = new List<T>();

        if (!typeof(T).IsInterface)
        {
            if (DEBUG) Debug.Log("T is not an Interface!. Returning empty list.");
            return interfaceList;
        }

        foreach (Component component in components)
        {
            if (component is T @interface)
            {
                interfaceList.Add(@interface);
            }
        }

        return interfaceList;
    }
    #endregion
}
