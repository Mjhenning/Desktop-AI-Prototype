using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ItemGenerator : MonoBehaviour {

    public static ItemGenerator Instance;

    [FormerlySerializedAs ("Artifacts")] [SerializeField] List<Item> artifacts; //list of predetermined sets

    [FormerlySerializedAs ("HasGeneratedSets")] [SerializeField]bool hasGeneratedSets;
    [SerializeField]ItemDatabase database;
    [FormerlySerializedAs ("ItemDescriptions")] [SerializeField] List<Descriptions> itemDescriptions;

    [SerializeField]Item tempItem;

    [FormerlySerializedAs ("ActiveShopItems")] [SerializeField]List<Item> activeShopItems; //used to reference current items in generated shop
    [FormerlySerializedAs ("SpriteCollections")] [SerializeField]List<ItemSprites> spriteCollections = new List<ItemSprites> (); //used to store type/set combos and their corresponding sprites

    int itemGeneratedCount = 0; //index for shop list


    void Awake () {
        Instance = this;
        
        if (!hasGeneratedSets) { //if sets haven't been populated yet
            GenerateSets (); //populate them
        } else {
            return;
        }

        EventsManager.RetrieveList.AddListener (ClearAnPopulateShop);
    }

    void GenerateSets () { //populates sets with their items from set scriptables to database
        foreach (NewDictItem _setItem in database.sets.listOfSets) {
            _setItem.items = _setItem.set.setItemDatabase;
        }
    }





    public Item GrabGeneratedItem () { //gets called to populate Item_Instances
        GenerateItemSetPair ();
        return tempItem;
    }
    

    void GenerateItemBasedOffItemSetPair (ItemTypes type, SetTypes set) { //Generate a new item from a passed type, set combo

        Item _createdItem = ScriptableObject.CreateInstance<Item> ();
        
        if (type == ItemTypes.Artifact) { //if the item is a artifact grab from list of predefined artifacts
            Debug.Log ("Grabbing an artifact");
            for (int _i = 0; _i < artifacts.Count; _i++) {
                if (artifacts[_i].mainSet == set) {
                    _createdItem = artifacts[_i];
                    tempItem = _createdItem;
                }
            }
        } else { //else if not a artifact set the set, item type, randomize the name and description and set the Image
            Debug.Log ("Creating a new item");
            _createdItem.mainSet = set;
            _createdItem.itemType = type;
            _createdItem.itemName = ItemNameGenerator.GenerateItemName (set, type);
            _createdItem.itemDesc = ItemDescGenerator.GenerateItemDesc (_createdItem, itemDescriptions);
            
            tempItem = _createdItem;
        }

		//Used to only populate the shop list 4 items at a time
        itemGeneratedCount++;
        if (itemGeneratedCount > 4) {
            activeShopItems.Clear ();
            activeShopItems.Add (tempItem);
            itemGeneratedCount = 1;
        } else {activeShopItems.Add (tempItem);}
    }

    void CheckInDatabase(ItemTypes type, SetTypes set) //doubles checks if item exists in database / current shop list before generating item
    {
        bool _itemExistsInDatabase = false;
        bool _itemExistsInActiveShop = false;

        // Check if the combination already exists in the database
        foreach (NewDictItem _itemSet in database.sets.listOfSets)
        {
            foreach (Item _item in _itemSet.items)
            {
                if (_item != null && _item.itemType == type && _item.mainSet == set)
                {
                    _itemExistsInDatabase = true;
                    break;
                }
            }
        }

        // Check if the combination already exists in the active shop items list
        foreach (Item _item in activeShopItems)
        {
            if (_item.itemType == type && _item.mainSet == set)
            {
                _itemExistsInActiveShop = true;
                break;
            }
        }

        if (_itemExistsInDatabase || _itemExistsInActiveShop)
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
        SetTypes _set;
        ItemTypes _itemtype = ItemTypes.MainItem ;

        _set = (SetTypes) Random.Range (0, 7);

        while (_itemtype == ItemTypes.MainItem) {
            _itemtype = (ItemTypes) Random.Range (0, 3);  
        }

        Debug.Log ("Generate item with" + _set + " " + _itemtype);

        CheckInDatabase (_itemtype, _set);
    }

    void ClearAnPopulateShop (List<Item> shopitems) { //used to repopulate shops everytime the vendor opens a new instance of the shop
        activeShopItems.Clear ();
        activeShopItems = shopitems;

    }
    
}


