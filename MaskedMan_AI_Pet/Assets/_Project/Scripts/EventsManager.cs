using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public static class EventsManager //huge events manager used by whole program to trigger multiple things on multiple scripts
{
    public static readonly UnityEvent BodyClicked = new UnityEvent ();
    public static readonly UnityEvent TieClicked = new UnityEvent ();
    public static readonly UnityEvent MaskClicked = new UnityEvent ();

    public static readonly UnityEvent DisableMaskInteractions = new UnityEvent ();
    public static readonly UnityEvent EnableMaskInteractions = new UnityEvent ();

    public static readonly UnityEvent ShopOpened = new UnityEvent ();
    public static readonly UnityEvent ShopClosed = new UnityEvent ();
    public static readonly UnityEvent<int> AddCurrency = new UnityEvent<int> ();
    public static readonly UnityEvent<int> RemoveCurrency = new UnityEvent<int> ();

    public static readonly UnityEvent<DialogueType> DialogueEvent = new UnityEvent<DialogueType> ();
    public static readonly UnityEvent<string, bool> DialogueStringEvent = new UnityEvent<string, bool> ();

    public static readonly UnityEvent<bool> CheckIfShopClosed = new UnityEvent<bool> ();
    static bool lastShopState = true;

    public static readonly UnityEvent StartHourlyCheck = new UnityEvent ();
    public static readonly UnityEvent RandomizeShopItems = new UnityEvent ();
    public static readonly UnityEvent<List<Item>> RetrieveList = new UnityEvent<List<Item>> ();
    public static readonly UnityEvent<ItemInstance, bool> CheckDatabase = new UnityEvent<ItemInstance, bool> ();
    public static readonly UnityEvent<ItemInstance, ShopRemoveType> RemoveItem = new UnityEvent<ItemInstance, ShopRemoveType> ();
    
    public static readonly UnityEvent<DraggedDirection> OnSwipeDirection = new UnityEvent<DraggedDirection>();

    public static readonly UnityEvent<string, string> StatisticStringEvent = new UnityEvent<string, string> ();


    //Void functions to fire events

    public static void ClickedBody () { BodyClicked.Invoke (); } //used to tell system vendor body was clicked

    public static void ClickedTie () { TieClicked.Invoke (); } //used to tell system vendor tie was clicked

    public static void ClickedMask () { MaskClicked.Invoke (); } //used to tell system vendor mask was clicked

    public static void OpenedShop () { ShopOpened.Invoke (); } //used to tell system shop should be opened

    public static void ClosedShop () { ShopClosed.Invoke (); } //used to tell system shop should be closed

    public static void DialogueDetermine (DialogueType type) { DialogueEvent.Invoke (type); } //used to tell dialogue manager what dialogue type should be passed along, type is determined via states

    public static void DialogueFeed (string text, bool corruptable) { DialogueStringEvent.Invoke (text, corruptable); } //used to tell dialogue manager what string from the type should be shown, string is determined via dialogue manager and fed to ui manager

    public static void DisableMask () { DisableMaskInteractions.Invoke (); } //used to disable mask gameobject

    public static void EnableMask () { EnableMaskInteractions.Invoke (); } //used to enable mask gameobject

    public static void CheckShop (bool open) {
        //used to give a bool determening if the shop should be currently open or not
        lastShopState = open;
        CheckIfShopClosed.Invoke (open);
    }

    public static bool ReturnShopState () { return lastShopState; } //used to determine if shop is closed

    public static void StartHourlyTimer () { StartHourlyCheck.Invoke (); } //used to only fire off time checker for if shop is completely closed or not after the first state change to idle

    public static void RandomizeShop () { RandomizeShopItems.Invoke ();}

    public static void PopulateActiveShopList (List<Item> shopItems) { RetrieveList.Invoke (shopItems); } //populates the list that has all the active shop's items

    public static void BoughtItem (ItemInstance item, bool bought) { CheckDatabase.Invoke (item, bought); } //used to fire off event when an item is bought

    public static void AddGoop (int amount) { AddCurrency.Invoke (amount); } //used to add goop when a goop object is pressed
    public static void RemoveGoop (int amount) { RemoveCurrency.Invoke (amount);} //used to remove goop when something is bought

    public static void RemoveFromLists (ItemInstance instance, ShopRemoveType type) { RemoveItem.Invoke (instance, type); } //removes an item from all possible lists it's on
    
    public static void HandleSwipeDirection(DraggedDirection direction) //Logic to handle swipe directions and pass the correct direction
    {
        // Handle the swipe direction
        Debug.Log("Detected swipe direction: " + direction);

        // Add your logic here based on the swipe direction
        switch (direction)
        {
            case DraggedDirection.Up:
                OnSwipeDirection.Invoke (DraggedDirection.Up);
                break;
            case DraggedDirection.Down:
                Debug.Log("Swipe Down");
                OnSwipeDirection.Invoke (DraggedDirection.Down);
                break;
            case DraggedDirection.Right:
                Debug.Log("Swipe Right");
                OnSwipeDirection.Invoke (DraggedDirection.Right);
                break;
            case DraggedDirection.Left:
                Debug.Log("Swipe Left");
                OnSwipeDirection.Invoke (DraggedDirection.Left);
                break;
        }
    }

    public static void FeedStat (string stat, string keyword) { StatisticStringEvent.Invoke (stat, keyword); }

}
