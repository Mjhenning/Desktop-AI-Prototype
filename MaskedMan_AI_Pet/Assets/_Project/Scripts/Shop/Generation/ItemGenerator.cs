using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour { 
    [SerializeField]bool HasGeneratedSets;
    ItemDatabase database;

    void Awake () {
        if (!HasGeneratedSets) {
            GenerateSets ();
        } else {
            return;
        }
    }

    void GenerateSets () {
        foreach (NewDictItem _setItem in database.Sets.ListOfSets) {
            _setItem.set.ItemName =GenerateSetName (_setItem.set.MainSet);
            _setItem.set.ItemDesc =GenerateSetDesc (_setItem.set.MainSet);
            _setItem.items = _setItem.set.SetItemDatabase;
        }
    }

   
    
    // void GenerateItems 
}

public static class MainItemNameGenerator {
    private static readonly List<string> Prefixes = new List<string> {
        "Ancient",
        "Cursed",
        "Enchanted"
    };
    

    private static readonly List<string> Suffixes = new List<string> {
        "of Flames",
        "the Swift",
        "of Shadows"
    };

    // Method to generate an item name
    public static string GenerateItemName () {
        string prefix = Prefixes[Random.Range (0, Prefixes.Count)];
        string rootWord = RootWords[Random.Range (0, RootWords.Count)];
        string suffix = Suffixes[Random.Range (0, Suffixes.Count)];

        // Combine the selected elements to form the item name
        string itemName = $"{prefix} {rootWord} {suffix}";
        return itemName;
    }
}
