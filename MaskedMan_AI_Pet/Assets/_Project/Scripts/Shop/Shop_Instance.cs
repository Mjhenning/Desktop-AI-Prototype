using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Instance : MonoBehaviour {
    public List<Item_Instance> ShopSelections;
    public List<Item> ShopItems;

    void Start () {
        EventsManager.PopulateActiveShopList (ShopItems); //populates the active shop list on the item generator
    }

    public void RandomizeItemsInShop () { //randomizes the generated items in shop by calling itemgenerator script
        ClearShop ();
        foreach (Item_Instance _item in ShopSelections) {
            _item.assignedItem = ItemGenerator.instance.GrabGeneratedItem ();
            // if (_item.assignedItem.ITEM_Sprite != null) {
            //     _item.image.sprite = _item.assignedItem.ITEM_Sprite;
            // }
            
            ShopItems.Add (_item.assignedItem);
        }

        setText ();
    }

    public void ClearShop () { //clears the shop of items
        ShopItems.Clear ();
    }

    public void AddListener () {
        EventsManager.RandomizeShopItems.AddListener (RandomizeItemsInShop); //add a listener to determine when shops should be randomized
    }

    public void setText () { //sets the text for the item
        foreach (Item_Instance _item in ShopSelections) {
            _item.Text.text = "Item: " + _item.assignedItem.ItemName + "\n" + "Type: " + _item.assignedItem.ItemType + "\n" + _item.assignedItem.ItemDesc;
        }
    }
}
