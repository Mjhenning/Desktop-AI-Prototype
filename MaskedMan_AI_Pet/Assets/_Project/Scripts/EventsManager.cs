using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTypeEvent : UnityEvent<DialogueType>{};
public class StringEvent : UnityEvent<String> { };

public class EventsManager : MonoBehaviour
{
    public static EventsManager instance;
    
    public UnityEvent BodyClicked;
    public UnityEvent TieClicked;
    public UnityEvent MaskClicked;
    
    
    public UnityEvent ShopOpened;
    public UnityEvent ShopClosed;
    
    public DialogueTypeEvent dialogueEvent;
    public StringEvent DialogueStringEvent;

    void Awake () {
        instance = this;
        dialogueEvent = new DialogueTypeEvent ();
        DialogueStringEvent = new StringEvent ();
    }
    
    public void ClickedBody() {BodyClicked.Invoke ();} //Invokes event to tell system body was clicked
    
    public void ClickedTie () {TieClicked.Invoke ();} //Invokes event to tell system tie was clicked
    
    public void ClickedMask () {MaskClicked.Invoke ();} //Invokes event to tell system mask was clicked
    
    public void OpenedShop () {ShopOpened.Invoke ();} //Invokes event to tell system shop has to be opened
    
    public void ClosedShop () {ShopClosed.Invoke ();} //Invokes event to tell system shop has to be closed

    public void DialogueDetermine(DialogueType type) { dialogueEvent.Invoke (type); } //invokes event to determine which dialogue snippet will be used

    public void DialogueFeed (String text) { DialogueStringEvent.Invoke (text); } //invokes an event to feed apropriate snippet

}
