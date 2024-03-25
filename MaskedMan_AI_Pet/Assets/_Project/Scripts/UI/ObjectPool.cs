using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour { //basic object pooling script with added logic (most of added logic is currently broken)
    public static ObjectPool Instance;

    public GameObject prefab; 
    public int maxPoolSize = 5;

    public List<GameObject> pooledObjects = new List<GameObject>();

    void Awake () {
        Instance = this;
    }

    private void Start() {
        InitializePool();
    }

    private void InitializePool() {
        for (int _i = 0; _i < maxPoolSize; _i++) {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject() {
        GameObject _obj = Instantiate(prefab, this.transform);
        if (_obj.GetComponent<ShopInstance>()) { //if obj has a shop instance script
            _obj.GetComponent<ShopInstance> ().AddListener(); //For each instantiated shop object add a listener   
        }
        _obj.SetActive(false);
        pooledObjects.Add(_obj);
        return _obj;
    }

    public GameObject GetObjectFromPool() {
        foreach (GameObject _obj in pooledObjects) {
            if (!_obj.activeInHierarchy) {
                _obj.SetActive(true);
                // Move the object to the front of the list
                pooledObjects.Remove(_obj);
                pooledObjects.Insert(0, _obj);
                return _obj;
            }
        }

        // If no inactive object is found, create a new one
        if (pooledObjects.Count < maxPoolSize) {
            GameObject _newObj = CreateNewObject();
            _newObj.SetActive(true);
            return _newObj;
        }

        return null; // Pool is full and no inactive objects available
    }

    public void ReturnAllObjectsToPool() {
        // Create a copy of the pooledObjects list to iterate over
        List<GameObject> _objectsToReturn = new List<GameObject>(pooledObjects);

        // Iterate over the copied list and deactivate objects
        foreach (GameObject _obj in _objectsToReturn) {
            _obj.SetActive(false);
        }

        // Clear the original pooledObjects list
        pooledObjects.Clear();

        // Add all objects back to the original pooledObjects list
        pooledObjects.AddRange(_objectsToReturn);
    }

    //Method to duplicate an object from the pool
    public GameObject DuplicateObjectFromPool(GameObject originalObj) {
        GameObject _obj = GetObjectFromPool(); // Get an inactive object from the pool

        ShopInstance _newObj = _obj.GetComponent<ShopInstance>(); //set new obj shop instance to set data to
        ShopInstance _ogObj = originalObj.GetComponent<ShopInstance>(); //set original object to grab data from
        _newObj.randomizedShop = false;

        _newObj.shopItems.Clear (); //clear new shop's item list
        
        //for every instance of shop_item
        for (int _i = 0; _i < _ogObj.shopSelections.Count; _i++) {
            _newObj.shopSelections[_i].assignedItem = _ogObj.shopItems[_i]; //assign the item from shop items list
            
            _newObj.shopItems.Add(_ogObj.shopItems[_i]); //adds items from og list to new list
        }

        _newObj.SetText (); //sets the text descriptions of the new shop
        
        _obj.SetActive(true); // Activate the new copy
        return _obj;
    }
}
