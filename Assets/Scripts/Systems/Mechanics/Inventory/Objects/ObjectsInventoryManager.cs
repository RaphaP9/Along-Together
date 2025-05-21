using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInventoryManager : MonoBehaviour
{
    public static ObjectsInventoryManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<ObjectInventoryIdentified> objectsInventory;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<ObjectInventoryIdentified> ObjectsInventory => objectsInventory;

    public static event EventHandler<OnObjectsEventArgs> OnObjectsInventoryInitialized;
    public static event EventHandler<OnObjectEventArgs> OnObjectAddedToInventory;
    public static event EventHandler<OnObjectEventArgs> OnObjectRemovedFromInventory;

    public class OnObjectEventArgs : EventArgs
    {
        public ObjectInventoryIdentified @object;
    }
    public class OnObjectsEventArgs : EventArgs
    {
        public List<ObjectInventoryIdentified> objects;
    }

    private void OnEnable()
    {
        //ShopStuff
    }

    private void OnDisable()
    {

    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeObjectsInventory();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ObjectsInventoryManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeObjectsInventory()
    {
        OnObjectsInventoryInitialized?.Invoke(this, new OnObjectsEventArgs { objects = objectsInventory }); 
    }

    #region Add Objects
    private void AddObjectToInventory(ObjectSO objectSO)
    {
        if (objectSO == null)
        {
            if (debug) Debug.Log("ObjectSO is null, addition will be ignored");
            return;
        }

        string objectGUID = GeneralUtilities.GenerateGUID();

        ObjectInventoryIdentified objectToAdd = new ObjectInventoryIdentified { GUID = objectGUID, objectSO = objectSO };

        objectsInventory.Add(objectToAdd);

        OnObjectAddedToInventory?.Invoke(this, new OnObjectEventArgs { @object = objectToAdd });
    }
    #endregion

    #region Remove Objects
    private void RemoveObjectFromInventoryByObjectSO(ObjectSO objectSO)
    {
        if (objectSO == null)
        {
            if (debug) Debug.Log("ObjectSO is null, remotion will be ignored");
            return;
        }

        ObjectInventoryIdentified objectIdentified = FindObjectByObjectSO(objectSO);

        if (objectIdentified == null)
        {
            if (debug) Debug.Log("Could not find object by ObjectSO");
            return;
        }

        objectsInventory.Remove(objectIdentified);

        OnObjectRemovedFromInventory?.Invoke(this, new OnObjectEventArgs { @object = objectIdentified });
    }

    private void RemoveObjectFromInventoryByGUID(string GUID)
    {
        ObjectInventoryIdentified objectIdentified = FindObjectByGUID(GUID);

        if (objectIdentified == null)
        {
            if (debug) Debug.Log("Could not find object by GUID");
            return;
        }

        objectsInventory.Remove(objectIdentified);

        OnObjectRemovedFromInventory?.Invoke(this, new OnObjectEventArgs { @object = objectIdentified });
    }
    #endregion

    #region Find Objects
    private ObjectInventoryIdentified FindObjectByObjectSO(ObjectSO objectSO)
    {
        foreach (ObjectInventoryIdentified @object in objectsInventory)
        {
            if (@object.objectSO == objectSO) return @object;
        }

        if (debug) Debug.Log($"Object with ObjectSO with ID {objectSO.id} could not be found. Proceding to return null");
        return null;
    }

    private ObjectInventoryIdentified FindObjectByGUID(string GUID)
    {
        foreach (ObjectInventoryIdentified @object in objectsInventory)
        {
            if (@object.GUID == GUID) return @object;
        }

        if (debug) Debug.Log($"Object with GUID {GUID} could not be found. Proceding to return null");
        return null;
    }
    #endregion

    public void SetObjectsInventory(List<ObjectInventoryIdentified> setterObjectsInventory) => objectsInventory.AddRange(setterObjectsInventory); //Add, not Replace!
}

[System.Serializable]
public class ObjectInventoryIdentified
{
    public string GUID;
    public ObjectSO objectSO;
}