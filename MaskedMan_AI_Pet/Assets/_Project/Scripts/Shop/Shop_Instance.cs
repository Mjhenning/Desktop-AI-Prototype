using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShopInstance : MonoBehaviour {
    public List<ItemInstance> shopSelections;
    public List<Item> shopItems;
    public List<ItemSprite_Instance> shopItemDisplays;
    
    public bool randomizedShop = true; //used to determine if a shop is supposed to be randomized or if it's a duplicate window
    public GameObject carousel;

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
        PopulateDisplays ();
        CloseCarousel ();
    }

    void ClearShop () { //clears the shop of items
        shopItems.Clear ();
    }

    public void AddListener () {
        EventsManager.RandomizeShopItems.AddListener (RandomizeItemsInShop); //add a listener to determine when shops should be randomized
    }

    public void SetText () { //sets the text for the item
        foreach (ItemInstance _item in shopSelections) {
            _item.descriptionText.text = "Item: " + _item.assignedItem.itemName + "\n" + "Type: " + _item.assignedItem.itemType + "\n" + "\n" + _item.assignedItem.itemDesc;
            _item.costText.text = _item.assignedItem.itemGoopCost.ToString ();
        }
    }

    public void ResetBool () { //used to reset bool so that pooled objects that were duplicated over can be randomized once again
        if (!randomizedShop) {
            randomizedShop = true;  
        }
    }

    public void ShowCarousel () {
        carousel.SetActive (true);
    }

    public void CloseCarousel () {
        carousel.SetActive (false);
    }

    public void PopulateDisplays () {
        for (int i = 0; i < shopItemDisplays.Count; i++) {
            shopItemDisplays[i].textObj.text = shopItems[i].itemGoopCost.ToString();
            if (shopItems[i].itemSprite != null) {
                shopItemDisplays[i].spriteObj.sprite = shopItems[i].itemSprite;  
            }
            
        }
    }
}
