using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    [SerializeField] ItemDatabase inventory;

    void Awake () {
        EventsManager.CheckDatabase.AddListener (EvaluateBool);
    }

 

    void EvaluateBool (ItemInstance instance, bool buy) {
        switch (buy) {
            case true:
                AddToDatabase (instance);
                break;
            case false:
                RemoveFromShop (instance);
                break;
        }
    }

    void AddToDatabase(ItemInstance instance) {
        for (int i = 0; i < inventory.sets.listOfSets.Length; i++) {
            if (instance.assignedItem.mainSet == inventory.sets.listOfSets[i].set.mainSet && instance.assignedItem.itemType != ItemTypes.Artifact) {
                // Ensure that the folder path does not end with a slash
                string folderPath = "Assets/_Project/Scripts/Scriptables/Shop/Bought_Items";
                if (!folderPath.EndsWith("/"))
                    folderPath += "/";

                // Sanitize the item name to remove any potential issues
                string sanitizedItemName = SanitizeFileName(instance.assignedItem.itemName);

                // Generate a unique asset path within the specified folder using interpolation
                string assetPath = $"{folderPath}{sanitizedItemName}.asset";
                // Create the asset at the generated path
                AssetDatabase.CreateAsset(instance.assignedItem, assetPath);
                AssetDatabase.SaveAssets();

                inventory.sets.listOfSets[i].items.Add(instance.assignedItem);
                EventsManager.RemoveGoop(instance.assignedItem.itemGoopCost);
                EventsManager.RemoveFromLists(instance);
            }
            else if (instance.assignedItem.mainSet == inventory.sets.listOfSets[i].set.mainSet && instance.assignedItem.itemType == ItemTypes.Artifact) {
                inventory.sets.listOfSets[i].items.Add(instance.assignedItem);
                EventsManager.RemoveGoop(instance.assignedItem.itemGoopCost);
                EventsManager.RemoveFromLists(instance);
            }
        }
    }
    
    string SanitizeFileName(string fileName) {
        foreach (char c in System.IO.Path.GetInvalidFileNameChars()) {
            fileName = fileName.Replace(c.ToString(), "");
        }
        return fileName;
    }

    void RemoveFromShop (ItemInstance instance) {
        EventsManager.RemoveFromLists (instance);
    }
    
    
}
