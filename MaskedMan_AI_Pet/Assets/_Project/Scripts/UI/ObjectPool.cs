using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour { //basic object pooling script with added logic (most of added logic is currently broken)
    public static ObjectPool instance;

    public GameObject prefab; 
    public int maxPoolSize = 5;

    public List<GameObject> pooledObjects = new List<GameObject>();

    void Awake () {
        instance = this;
    }

    private void Start() {
        InitializePool();
    }

    private void InitializePool() {
        for (int i = 0; i < maxPoolSize; i++) {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject() {
        GameObject obj = Instantiate(prefab, this.transform);
        if (obj.GetComponent<Shop_Instance>()) { //if obj has a shop instance script
            obj.GetComponent<Shop_Instance> ().AddListener(); //For each instantiated shop object add a listener   
        }
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }

    public GameObject GetObjectFromPool() {
        foreach (GameObject obj in pooledObjects) {
            if (!obj.activeInHierarchy) {
                obj.SetActive(true);
                // Move the object to the front of the list
                pooledObjects.Remove(obj);
                pooledObjects.Insert(0, obj);
                return obj;
            }
        }

        // If no inactive object is found, create a new one
        if (pooledObjects.Count < maxPoolSize) {
            GameObject newObj = CreateNewObject();
            newObj.SetActive(true);
            return newObj;
        }

        return null; // Pool is full and no inactive objects available
    }

    public void ReturnAllObjectsToPool() {
        // Create a copy of the pooledObjects list to iterate over
        List<GameObject> objectsToReturn = new List<GameObject>(pooledObjects);

        // Iterate over the copied list and deactivate objects
        foreach (GameObject obj in objectsToReturn) {
            obj.SetActive(false);
        }

        // Clear the original pooledObjects list
        pooledObjects.Clear();

        // Add all objects back to the original pooledObjects list
        pooledObjects.AddRange(objectsToReturn);
    }

    //Method to duplicate an object from the pool
    public GameObject DuplicateObjectFromPool(GameObject origonal_obj) {
        GameObject obj = GetObjectFromPool(); // Get an inactive object from the pool

        Shop_Instance newObj = obj.GetComponent<Shop_Instance>(); //set new obj shop instnace to set data to
        Shop_Instance og_obj = origonal_obj.GetComponent<Shop_Instance>(); //set origonal object to grab data from

        newObj.ShopItems.Clear (); //clear new shop's item list
        
        //for every instance of shop_item
        for (int i = 0; i < og_obj.ShopSelections.Count; i++) {
            newObj.ShopSelections[i].assignedItem = og_obj.ShopItems[i]; //assign the item from shop items list
            
            newObj.ShopItems.Add(og_obj.ShopItems[i]); //adds items from og list to new list
        }

        newObj.setText (); //sets the text descriptions of the new shop
        
        obj.SetActive(true); // Activate the new copy
        return obj;
    }
}
