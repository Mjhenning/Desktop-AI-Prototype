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
    [SerializeField] List<Descriptions> ItemDescriptions;

    [SerializeField]Item tempItem;

    [SerializeField]List<Item> ActiveShopItems;
    [SerializeField]List<ItemSprites> SpriteCollections = new List<ItemSprites> ();


    void Awake () {
        instance = this;
        
        if (!HasGeneratedSets) {
            GenerateSets ();
        } else {
            return;
        }

        EventsManager.RetrieveList.AddListener (ClearAnPopulateShop);
    }

    void GenerateSets () {
        foreach (NewDictItem _setItem in database.Sets.ListOfSets) {
            _setItem.items = _setItem.set.SetItemDatabase;
        }
    }





    public Item GrabGeneratedItem () { //gets called to populate Item_Instances
        GenerateItemSetPair ();
        return tempItem;
    }
    

    void GenerateItemBasedOffItemSetPair (ItemTypes type, SetTypes set) { //Generate a new item from a passed type, set combo

        Item createdItem = ScriptableObject.CreateInstance<Item> ();
        
        if (type == ItemTypes.Artifact) { //if the item is a artifact grab from list of predefined artifacts
            Debug.Log ("Grabbing an artifact");
            for (int i = 0; i < Artifacts.Count; i++) {
                if (Artifacts[i].MainSet == set) {
                    createdItem = Artifacts[i];
                    tempItem = createdItem;
                }
            }
        } else { //else if not a artifact set the set, item type, randomize the name and description and set the Image
            Debug.Log ("Creating a new item");
            createdItem.MainSet = set;
            createdItem.ItemType = type;
            createdItem.ItemName = ItemNameGenerator.GenerateItemName (set, type);
            createdItem.ItemDesc = ItemDescGenerator.GenerateItemDesc (createdItem, ItemDescriptions);
            
            tempItem = createdItem;
        }
    }

    void CheckInDatabase(ItemTypes type, SetTypes set)
    {
        bool itemExistsInDatabase = false;
        bool itemExistsInActiveShop = false;

        // Check if the combination already exists in the database
        foreach (NewDictItem itemSet in database.Sets.ListOfSets)
        {
            foreach (Item item in itemSet.items)
            {
                if (item != null && item.ItemType == type && item.MainSet == set)
                {
                    itemExistsInDatabase = true;
                    break;
                }
            }
        }

        // Check if the combination already exists in the active shop items list
        foreach (Item item in ActiveShopItems)
        {
            if (item.ItemType == type && item.MainSet == set)
            {
                itemExistsInActiveShop = true;
                break;
            }
        }

        if (itemExistsInDatabase || itemExistsInActiveShop)
        {
            // If the item already exists, do not generate a new one
            Debug.Log("Item already exists in the database or active shop");
            GenerateItemSetPair();
        }
        else
        {
            // Generate a new item
            Debug.Log("Generating a new item");
            GenerateItemBasedOffItemSetPair(type, set);
        }
    }

    void GenerateItemSetPair () { //Generates a random itemtype and itemset combo
        SetTypes set;
        ItemTypes itemtype = ItemTypes.MainItem ;

        set = (SetTypes) Random.Range (0, 7);

        while (itemtype == ItemTypes.MainItem) {
            itemtype = (ItemTypes) Random.Range (0, 3);  
        }

        Debug.Log ("Generate item with" + set + " " + itemtype);

        CheckInDatabase (itemtype, set);
    }

    void ClearAnPopulateShop (List<Item> shopitems) {
        ActiveShopItems.Clear ();
        for (int i = 0; i < shopitems.Count; i++) {
            ActiveShopItems.Add (shopitems[i]);
        }

    }
    
}


