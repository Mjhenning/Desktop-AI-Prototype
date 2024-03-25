using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShopInstance : MonoBehaviour {
    public List<ItemInstance> shopSelections;
    public List<Item> shopItems;
    public bool randomizedShop = true; //used to determine if a shop is supposed to be randomized or if it's a duplicate window

    void OnEnable () {
        EventsManager.ShopClosed.AddListener (ResetBool);
    }

    void OnDisable () {
        EventsManager.ShopClosed.RemoveListener (ResetBool);
    }

    void Start () {
        if (randomizedShop) 
            EventsManager.PopulateActiveShopList (shopItems); //populates the active shop list on the item generator
    }

    internal void RandomizeItemsInShop () { //randomizes the generated items in shop by calling itemgenerator script
        ClearShop ();
        foreach (ItemInstance _item in shopSelections) {
            _item.assignedItem = ItemGenerator.Instance.GrabGeneratedItem ();
            // if (_item.assignedItem.ITEM_Sprite != null) {
            //     _item.image.sprite = _item.assignedItem.ITEM_Sprite;
            // }
            
            shopItems.Add (_item.assignedItem);
        }

        SetText ();
    }

    void ClearShop () { //clears the shop of items
        shopItems.Clear ();
    }

    public void AddListener () {
        EventsManager.RandomizeShopItems.AddListener (RandomizeItemsInShop); //add a listener to determine when shops should be randomized
    }

    public void SetText () { //sets the text for the item
        foreach (ItemInstance _item in shopSelections) {
            _item.text.text = "Item: " + _item.assignedItem.itemName + "\n" + "Type: " + _item.assignedItem.itemType + "\n" + _item.assignedItem.itemDesc;
        }
    }

    public void ResetBool () { //used to reset bool so that pooled objects that were duplicated over can be randomized once again
        if (!randomizedShop) {
            randomizedShop = true;  
        }
    }
}
