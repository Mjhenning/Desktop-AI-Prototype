using System.Collections.Generic;
using CarouselUI;
using UnityEngine;
using UnityEngine.Serialization;

public enum ShopRemoveType {
    Buy,
    Discard,
    Corruption
}


public class ShopInstance : MonoBehaviour {
    public List<ItemInstance> shopSelections;
    public List<Item> shopItems;
    [FormerlySerializedAs ("parent")] public DisplayParent spritesparent;

    public bool randomizedShop = true; //used to determine if a shop is supposed to be randomized or if it's a duplicate window
    public GameObject carousel;
    

    void OnEnable () {
        EventsManager.ShopClosed.AddListener (ResetBool);
        EventsManager.RemoveItem.AddListener(RemoveItemFromShops);

        if (randomizedShop) {
            EventsManager.RandomizeShopItems.AddListener (RandomizeItemsInShop);
        }
        
        //populates carousel
        carousel.GetComponent<CarouselUIElement> ().optionsObjects.Clear ();
        for (int i = 0; i < shopSelections.Count; i++) {
            carousel.GetComponent<CarouselUIElement> ().optionsObjects.Add (shopSelections[i].gameObject);
        }
    }

    void OnDisable () {
        EventsManager.ShopClosed.RemoveListener (ResetBool);
        EventsManager.RemoveItem.RemoveListener(RemoveItemFromShops);
        EventsManager.RandomizeShopItems.RemoveListener (RandomizeItemsInShop);
        shopItems.Clear ();
        spritesparent.gameObject.SetActive (false);
    }
    
    internal void RandomizeItemsInShop () { //randomizes the generated items in shop by calling itemgenerator script
        shopItems.Clear ();
        foreach (ItemInstance _item in shopSelections) {
            _item.assignedItem = ItemGenerator.Instance.GrabGeneratedItem ();
            // if (_item.assignedItem.ITEM_Sprite != null) {
            //     _item.image.sprite = _item.assignedItem.ITEM_Sprite;
            // }
            
            shopItems.Add (_item.assignedItem);
        }

        SetItem ();
        CloseCarousel ();
        DisableItemsBasedOffCorruption ();
        spritesparent.gameObject.SetActive (true);
    }
    
    public void DisableItemsBasedOffCorruption () { //used to limit amount of items player can buy based off of the ai's current corruption percentage
        if (Corruption_Manager.instance.corruptionPercentage < 25) {
            
        } else if (Corruption_Manager.instance.corruptionPercentage >= 25 && Corruption_Manager.instance.corruptionPercentage < 50  ) { //removes last item
            RemoveItemFromShops (shopSelections[3], ShopRemoveType.Corruption);
        } else if (Corruption_Manager.instance.corruptionPercentage >= 50 && Corruption_Manager.instance.corruptionPercentage < 75) { //removes last 2 items
            RemoveItemFromShops (shopSelections[3], ShopRemoveType.Corruption);
            RemoveItemFromShops (shopSelections[2], ShopRemoveType.Corruption);
        } else if (Corruption_Manager.instance.corruptionPercentage >= 75) { //removes last 3 items
            RemoveItemFromShops (shopSelections[3], ShopRemoveType.Corruption);
            RemoveItemFromShops (shopSelections[2], ShopRemoveType.Corruption);
            RemoveItemFromShops (shopSelections[1], ShopRemoveType.Corruption);
        }
    }
    

    public void AddListener () {
        EventsManager.RandomizeShopItems.AddListener (RandomizeItemsInShop); //add a listener to determine when shops should be randomized
    }

    public void SetItem () { //sets the text and sprite for the item
        foreach (ItemInstance _item in shopSelections) {

            if (_item.assignedItem == null) {
                continue;
            }
            
            if (_item.assignedItem.itemSprite != null) {
                _item.image.sprite = _item.assignedItem.itemSprite;
            }
            
            _item.descriptionText.text = "Item: " + _item.assignedItem.itemName + "\n" + "Type: " + _item.assignedItem.itemType + "\n" + "\n" + _item.assignedItem.itemDesc;
            _item.costText.text = _item.assignedItem.itemGoopCost.ToString ();
        }
    }

    public void ResetBool () { //used to reset bool so that pooled objects that were duplicated over can be randomized once again
        if (!randomizedShop) {
            randomizedShop = true;  
        }
    }

    public void ShowCarousel (ItemSprite_Instance display) { //shows item shop carousel and specifically change index to selected sprite
        
        CarouselUIElement _carouselscript = carousel.GetComponent<CarouselUIElement> ();

        _carouselscript.CurrentIndex = spritesparent.shopItemDisplays.IndexOf (display);
        carousel.SetActive (true); 
        
    }

    public void CloseCarousel () { //hides item shop carousel
        carousel.SetActive (false);
    }
    

    public void RemoveItemFromShops (ItemInstance instance, ShopRemoveType type) { //used to remove a item from every possible location it is currently displayed or accessed
        
        //Removes item from Carousel lists
        
        CarouselUIElement _carouselscript = carousel.GetComponent<CarouselUIElement> ();

        if (AIController.Instance.GetCurrentState() != StateType.Agressive) { //if not aggressive state handle shop go to next item as follows
            if (carousel.activeSelf) { //Goes to next item automatically
                if (_carouselscript.optionsObjects.Count > 1) {
                    _carouselscript.PressNext ();  
                } else if (_carouselscript.optionsObjects.Count < 2) { //if there is only one object and it gets bought / discarded close the shop
                    EventsManager.ShopClosed.Invoke ();
                }
            }
        } else {
            if (type == ShopRemoveType.Buy) {
                EventsManager.ShopClosed.Invoke ();
            } else {
                if (carousel.activeSelf) { //Goes to next item automatically
                    if (_carouselscript.optionsObjects.Count > 1) {
                        _carouselscript.PressNext ();  
                    } else if (_carouselscript.optionsObjects.Count < 2) { //if there is only one object and it gets bought / discarded close the shop
                        EventsManager.ShopClosed.Invoke ();
                    }
                }
            }
            
        }
      
        _carouselscript.optionsObjects.Remove (instance.gameObject);

        //Remove item from ShopItems list
        shopItems.Remove (instance.assignedItem);
        
        
        //Removes item from sprite displays
        Debug.Log (instance);

        if (shopSelections.Contains (instance)) { //stops issues during aggressive state where multiple windows listens to the same event
            int _indexselection = shopSelections.IndexOf (instance);
            Debug.Log ("Index " +_indexselection);
            spritesparent.shopItemDisplays[_indexselection].gameObject.SetActive (false);
        }
       

    }
}
