using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public static readonly UnityEvent<string> DialogueStringEvent = new UnityEvent<string> ();

    public static readonly UnityEvent<bool> CheckIfShopClosed = new UnityEvent<bool> ();
    static bool lastShopState = true;

    public static readonly UnityEvent StartHourlyCheck = new UnityEvent ();
    public static readonly UnityEvent RandomizeShopItems = new UnityEvent ();
    public static readonly UnityEvent<List<Item>> RetrieveList = new UnityEvent<List<Item>> ();
    public static readonly UnityEvent<ItemInstance, bool> CheckDatabase = new UnityEvent<ItemInstance, bool> ();
    public static readonly UnityEvent<ItemInstance> RemoveItem = new UnityEvent<ItemInstance> ();


    //Void functions to fire events

    public static void ClickedBody () { BodyClicked.Invoke (); } //used to tell system vendor body was clicked

    public static void ClickedTie () { TieClicked.Invoke (); } //used to tell system vendor tie was clicked

    public static void ClickedMask () { MaskClicked.Invoke (); } //used to tell system vendor mask was clicked

    public static void OpenedShop () { ShopOpened.Invoke (); } //used to tell system shop should be opened

    public static void ClosedShop () { ShopClosed.Invoke (); } //used to tell system shop should be closed

    public static void DialogueDetermine (DialogueType type) { DialogueEvent.Invoke (type); } //used to tell dialogue manager what dialogue type should be passed along, type is determined via states

    public static void DialogueFeed (String text) { DialogueStringEvent.Invoke (text); } //used to tell dialogue manager what string from the type should be shown, string is determined via dialogue manager and fed to ui manager

    public static void DisableMask () { DisableMaskInteractions.Invoke (); } //used to disable mask gameobject

    public static void EnableMask () { EnableMaskInteractions.Invoke (); } //used to enable mask gameobject

    public static void CheckShop (bool open) {
        //used to give a bool determening if the shop should be currently open or not
        lastShopState = open;
        CheckIfShopClosed.Invoke (open);
    }

    public static bool ReturnShopState () { return lastShopState; } //used to determine if shop is closed

    public static void StartHourlyTimer () { StartHourlyCheck.Invoke (); } //used to only fire off time checker for if shop is completely closed or not after the first state change to idle

    public static void RandomizeShop () { RandomizeShopItems.Invoke (); }

    ///used to randomize the inventory of the shop

    public static void PopulateActiveShopList (List<Item> shopItems) { RetrieveList.Invoke (shopItems); }

    public static void BoughtItem (ItemInstance item, bool bought) { CheckDatabase.Invoke (item, bought); }

    public static void AddGoop (int amount) { AddCurrency.Invoke (amount); }
    public static void RemoveGoop (int amount) { RemoveCurrency.Invoke (amount);}

    public static void RemoveFromLists (ItemInstance instance) { RemoveItem.Invoke (instance); }

}
