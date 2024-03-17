using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public static class EventsManager
{
    public static UnityEvent BodyClicked = new UnityEvent();
    public static UnityEvent TieClicked = new UnityEvent();
    public static UnityEvent MaskClicked = new UnityEvent();

    public static UnityEvent DisableMaskInteractions = new UnityEvent();
    public static UnityEvent EnableMaskInteractions = new UnityEvent();

    public static UnityEvent ShopOpened = new UnityEvent();
    public static UnityEvent ShopClosed = new UnityEvent();

    public static UnityEvent<DialogueType> dialogueEvent = new UnityEvent<DialogueType>();
    public static UnityEvent<string> DialogueStringEvent = new UnityEvent<string>();

    public static UnityEvent<bool> CheckIfShopClosed = new UnityEvent<bool> ();
    static bool lastShopState = true;
    
    public static UnityEvent StartHourlyCheck = new UnityEvent ();
    public static UnityEvent RandomizeShopItems = new UnityEvent ();
    public static UnityEvent<List<Item>> RetrieveList = new UnityEvent<List<Item>> ();

    public static void ClickedBody() { BodyClicked.Invoke(); } //used to tell system vendor body was clicked

    public static void ClickedTie() { TieClicked.Invoke(); } //used to tell system vendor tie was clicked

    public static void ClickedMask() { MaskClicked.Invoke(); } //used to tell system vendor mask was clicked

    public static void OpenedShop() { ShopOpened.Invoke(); } //used to tell system shop should be opened

    public static void ClosedShop() { ShopClosed.Invoke(); } //used to tell system shop should be closed

    public static void DialogueDetermine(DialogueType type) { dialogueEvent.Invoke(type); } //used to tell dialogue manager what dialogue type should be passed along

    public static void DialogueFeed(String text) { DialogueStringEvent.Invoke(text); } //used to tell dialogue manager what string from the type should be shown

    public static void DisableMask() { DisableMaskInteractions.Invoke(); } //used to disable mask gameobject

    public static void EnableMask() { EnableMaskInteractions.Invoke(); } //used to enable mask gameobject

    public static void CheckShop (bool open) { //used to give a bool determening if the shop should be currently open or not
        lastShopState = open;
        CheckIfShopClosed.Invoke (open);
    }

    public static bool ReturnShopState () { return lastShopState;} //used to determine if shop is closed

    public static void StartHourlyTimer () { StartHourlyCheck.Invoke (); } //used to only fire off time checker for if shop is completely closed or not after the first state change to idle

    public static void RandomizeShop () { RandomizeShopItems.Invoke (); } ///used to randomize the inventory of the shop

    public static void PopulateActiveShopList (List<Item> shopItems) { RetrieveList.Invoke (shopItems); }

}
