using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public partial class SaveLoadManager : MonoBehaviour {

    public ItemDatabase inventory;
    
    public static SaveLoadManager instance;

    public Savedata tempSavedata = new Savedata {
        items = new List<ItemData>(), player = new Playerdata(), vendor = new VendorData()
    };

    private string saveFilePath;
    
    void Awake () {
        if (instance != null) {
            // Used to destroy this manager if one exists in scene (put in place to stop duplicates)
            Debug.Log("Found more than one Save/Load Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        } else if (instance == null) {
            // Else create a new one
            Debug.Log("Creating a new Save/Load Manager");
            instance = this;
        }
        

        // Set the path to save the file
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");

        CallOnLoad();

        DontDestroyOnLoad(gameObject); // Tells the system to not destroy this manager
    }

    void CallOnLoad() {
        if (File.Exists(saveFilePath)) {
            string savedDataJson = File.ReadAllText(saveFilePath);
            if (!string.IsNullOrEmpty(savedDataJson)) {
                tempSavedata = JsonUtility.FromJson<Savedata>(savedDataJson);
                if (tempSavedata == null) {
                    Debug.LogWarning("Failed to deserialize saved data.");
                    tempSavedata = new Savedata {
                        items = new List<ItemData>(), player = new Playerdata(), vendor = new VendorData()
                    };
                }
            } else {
                Debug.LogWarning("No saved data found.");
                tempSavedata = new Savedata {
                    items = new List<ItemData>(), player = new Playerdata(), vendor = new VendorData()
                };
            }
        } else {
            Debug.LogWarning("Save file not found.");
            tempSavedata = new Savedata {
                items = new List<ItemData>(), player = new Playerdata(), vendor = new VendorData()
            };
        }

        LoadGame();
    }
    
    public void LoadGame() {
        
        // Inventory load
        for (int i = 0; i < instance.inventory.setDB.setList.Length; i++) {
            inventory.setDB.setList[i].ClearItems();
        }

        if (tempSavedata != null) {
            foreach (ItemData _item in tempSavedata.items) {
                for (int j = 0; j < inventory.setDB.setList.Length; j++) {
                    if (_item.mainSet == inventory.setDB.setList[j].set.mainSet) {
                        Item _createdItem = ScriptableObject.CreateInstance<Item>();
                        _createdItem.mainSet = _item.mainSet;
                        _createdItem.itemType = _item.itemType;
                        _createdItem.itemName = _item.itemName;
                        _createdItem.itemDesc = _item.itemDesc;
                        _createdItem.itemGoopCost = _item.itemGoopCost;
                        _createdItem.itemSprite = _item.itemSprite;

                        inventory.setDB.setList[j].items.Add(_createdItem);
                    }
                }
            }
            
            //Load vendor data
            UI_Manager.instance.CallOnLoad (tempSavedata.vendor.currentTieColor, tempSavedata.vendor.currentVendorMaskIndex);
        
            //Load corruption data
            Corruption_Manager.instance.currentCorruption = tempSavedata.player.currentCorruption;
            Corruption_Manager.instance.corruptionPercentage = tempSavedata.player.currentCPercentage;
        }
        
      
    }
    
    public void SaveGame() {
        // Clear tempSavedata before saving new data
        tempSavedata.items.Clear();
        

        // Debug log to check if tempSavedata is null
        Debug.Log("tempSavedata is null: " + (tempSavedata == null));
        
        //Saving of Items

        for (int i = 0; i < inventory.setDB.setList.Length; i++) {
            // Check if the items list is not null and not empty
            if (inventory.setDB.setList[i].items != null &&
                inventory.setDB.setList[i].items.Count > 0) {
                for (int j = 0; j < inventory.setDB.setList[i].items.Count; j++) {
                    // Check if the current item is not null
                    if (inventory.setDB.setList[i].items[j] != null) {
                        // Create a new ItemData object
                        ItemData _newItem = new ItemData {
                            // Assign values to the new ItemData object from the current item
                            mainSet = inventory.setDB.setList[i].items[j].mainSet,
                            itemType = inventory.setDB.setList[i].items[j].itemType,
                            itemSprite = inventory.setDB.setList[i].items[j].itemSprite,
                            itemName = inventory.setDB.setList[i].items[j].itemName,
                            itemDesc = inventory.setDB.setList[i].items[j].itemDesc,
                            itemGoopCost = instance.inventory.setDB.setList[i].items[j].itemGoopCost
                        };

                        Debug.Log(_newItem.itemName);

                        // Add the new ItemData object to tempSavedata
                        // Check if tempSavedata is not null before adding items
                        if (tempSavedata != null && tempSavedata.items != null) {
                            tempSavedata.items.Add(_newItem);
                        } else {
                            Debug.LogWarning("tempSavedata or tempSavedata.items is null");
                        }
                    }
                }
            }
        }
        
        //Saving of Vendor Data
        tempSavedata.vendor.currentTieColor = UI_Manager.instance.tie.color;
        tempSavedata.vendor.currentVendorMaskIndex = UI_Manager.instance.maskIndex;
        
        //Saving Corruption Status;
        tempSavedata.player.currentCPercentage = Corruption_Manager.instance.corruptionPercentage;
        tempSavedata.player.currentCorruption = Corruption_Manager.instance.currentCorruption;

        // Save tempSavedata to a file
        string json = JsonUtility.ToJson(tempSavedata);
        File.WriteAllText(saveFilePath, json);
    }

    public void OnDestroy() {
        SaveGame();
    }
}
