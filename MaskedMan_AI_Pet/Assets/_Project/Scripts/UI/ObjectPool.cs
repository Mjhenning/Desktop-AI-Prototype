using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    public static ObjectPool instance;
    
    public GameObject prefab; 
    public int maxPoolSize = 5; 

    private List<GameObject> pooledObjects = new List<GameObject>();

    void Awake () {
        instance = this;
    }

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < maxPoolSize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab, this.transform);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }

    public GameObject GetObjectFromPool()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                // Move the object to the front of the list
                pooledObjects.Remove(obj);
                pooledObjects.Insert(0, obj);
                return obj;
            }
        }

        // If no inactive object is found, create a new one
        if (pooledObjects.Count < maxPoolSize)
        {
            GameObject newObj = CreateNewObject();
            newObj.SetActive(true);
            return newObj;
        }

        return null; // Pool is full and no inactive objects available
    }
    
    public void ReturnAllObjectsToPool()
    {
        foreach (GameObject obj in pooledObjects)
        {
            obj.SetActive(false);
            // Move the object to the back of the list
            pooledObjects.Remove(obj);
            pooledObjects.Add(obj);
        }
    }
}