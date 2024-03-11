using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemGenerator : MonoBehaviour {

    public static ItemGenerator instance;

    [SerializeField] List<Item> Artifacts;

    [SerializeField]bool HasGeneratedSets;
    [SerializeField]ItemDatabase database;
    List<Item> ShopItems = new List<Item>();

    [SerializeField]Item tempItem;



    void Awake () {
        instance = this;
        
        if (!HasGeneratedSets) {
            GenerateSets ();
        } else {
            return;
        }
    }

    void GenerateSets () {
        foreach (NewDictItem _setItem in database.Sets.ListOfSets) {
            // _setItem.set.ItemDesc =GenerateSetDesc (_setItem.set.MainSet);
            _setItem.items = _setItem.set.SetItemDatabase;
        }
    }





    public Item GrabGeneratedItem () { //gets called to populate Item_Instances
        GenerateItemSetPair ();
        ShopItems.Add (tempItem);
        return tempItem;
    }
    

    void GenerateItemBasedOffItemSetPair (ItemTypes type, SetTypes set) { //Generate a new item from a passed type, set combo

        Item createdItem = ScriptableObject.CreateInstance<Item> ();
        
        if (type == ItemTypes.Artifact) { //if the item is a artifact grab from list of predefined artifacts
            for (int i = 0; i < Artifacts.Count; i++) {
                if (Artifacts[i].MainSet == set) {
                    createdItem = Artifacts[i];
                    tempItem = createdItem;
                }
            }
        } else { //else if not a artifact set the set, item type, randomize the name and description and set the Image
            createdItem.MainSet = set;
            createdItem.ItemType = type;
            createdItem.ItemName = ItemNameGenerator.GenerateItemName (set, type);
            //createdItem.ItemDesc =
            
            tempItem = createdItem;
        }
    }

    void CheckInDatabase (ItemTypes type, SetTypes set) { //double checks generated combo if it doesn't already exist

        for (int i = 0; i < database.Sets.ListOfSets.Length; i++) {
            for (int j = 0; j < database.Sets.ListOfSets[i].items.Count; j++) { //if it exists regenerate
                if (database.Sets.ListOfSets[i].items[j].ItemType == type && database.Sets.ListOfSets[i].items[j].MainSet == set ) {
                    GenerateItemSetPair ();
                } else { //if it doesn't exist create a new item
                    GenerateItemBasedOffItemSetPair (type, set);
                }
            }
        }
    }

    void GenerateItemSetPair () { //Generates a random itemtype and itemset combo
        SetTypes set;
        ItemTypes itemtype = ItemTypes.MainItem ;

        set = (SetTypes) Random.Range (0, 7);

        while (itemtype == ItemTypes.MainItem) {
            itemtype = (ItemTypes) Random.Range (0, 3);  
        }

        CheckInDatabase (itemtype, set);
    }

    void ClearShopList () {
        ShopItems.Clear ();
    }
}


