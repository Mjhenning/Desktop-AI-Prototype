using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



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

    public static void ClickedBody() { BodyClicked.Invoke(); }

    public static void ClickedTie() { TieClicked.Invoke(); }

    public static void ClickedMask() { MaskClicked.Invoke(); }

    public static void OpenedShop() { ShopOpened.Invoke(); }

    public static void ClosedShop() { ShopClosed.Invoke(); }

    public static void DialogueDetermine(DialogueType type) { dialogueEvent.Invoke(type); }

    public static void DialogueFeed(String text) { DialogueStringEvent.Invoke(text); }

    public static void DisableMask() { DisableMaskInteractions.Invoke(); }

    public static void EnableMask() { EnableMaskInteractions.Invoke(); }

    public static void CheckShop (bool open) {
        lastShopState = open;
        CheckIfShopClosed.Invoke (open);
    }

    public static bool ReturnShopState () { return lastShopState;}

    public static void StartHourlyTimer () { StartHourlyCheck.Invoke (); } //used to only fire off time checker for if shop is completely closed or not after the first state change to idle

}
