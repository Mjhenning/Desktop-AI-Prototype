using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Instance : MonoBehaviour {
    [SerializeField]List<Item_Instance> ShopSelections;
    public List<Item> ShopItems;

    void Start () {
        EventsManager.RandomizeShopItems.AddListener (RandomizeItemsInShop);
        
        ClearShop ();
        RandomizeItemsInShop ();
    }
    
    public void RandomizeItemsInShop () {
        foreach (Item_Instance _item in ShopSelections) {
            _item.assignedItem = ItemGenerator.instance.GrabGeneratedItem ();
            // if (_item.assignedItem.ITEM_Sprite != null) {
            //     _item.image.sprite = _item.assignedItem.ITEM_Sprite;
            // }
        
            _item.Text.text = "Item: " + _item.assignedItem.ItemName + "\n" + "Type: " + _item.assignedItem.ItemType + "\n" + _item.assignedItem.ItemDesc;
            ShopItems.Add (_item.assignedItem);
        }

        EventsManager.PopulateActiveShopList (ShopItems);
    }

    public void ClearShop () {
        ShopItems.Clear ();
    }
}
