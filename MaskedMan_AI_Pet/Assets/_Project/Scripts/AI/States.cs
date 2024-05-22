using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
FSM States
*/

//less timer based interactions and more influence based interactions?

//TODO: tweak timers
#region Idle State

public class StateIdle : State { //Idle State
    float timeLeftSleep;
    float timeLeftTalk;
    float timeLeftShop;
    float timeLeftAgressive;
    bool isShopOpen;
    
    public StateIdle (AIController controller) : base (controller) { } //required

    public override void Enable () {
        base.Enable (); //Fires off the log of the base abstract function (everything already inside abstract class fires when base.enable is called)
        StateName = StateType.Idle;
        EventsManager.CheckIfShopClosed.AddListener (CheckBeforeResumeUpdate);
        
        timeLeftSleep = Random.Range(30f,60f);
        ResetTextTimer ();
        //timeLeftShop = Random.Range (10f, 300f);
        timeLeftShop = 100f;
        timeLeftAgressive = 1200f; //doesn't fire off ever because timer constantly resets
    }
    

    public override void Update () {

        switch (isShopOpen) {
            case true:
                if (timeLeftTalk > 0) {
                    //Timer between dialogue 
                    timeLeftTalk -= Time.deltaTime;
                } else {
                    EventsManager.DialogueDetermine (DialogueType.Idle);
                    ResetTextTimer ();
                }

                if (timeLeftSleep > 0) {
                    //Timer between state change to sleep
                    timeLeftSleep -= Time.deltaTime;
                } else {
                    Controller.ChangeState (Controller.stateSleep);
                }

                if (timeLeftShop > 0) {
                    //Timer between state change to shop (coat open)
                    timeLeftShop -= Time.deltaTime;
                } else {
                    Controller.ChangeState (Controller.stateShopPrompt);
                }
                
                if (timeLeftAgressive > 0) { //Timer between state change to aggressive behaviour
                    timeLeftAgressive -= Time.deltaTime;
                } else {
                    Controller.ChangeState (Controller.stateAgressive);
                }

                break;
             //if the shop is currently closed
            case false:
                Controller.ChangeState (Controller.stateSleep); //force vendor to fall asleep
                break;
        }
        
       
        

    }

    void CheckBeforeResumeUpdate (bool shopOpen) { //used to set bool determining if shop is open equal to event's boolean determining if shop is open or not
        isShopOpen = shopOpen;
    }

    void ResetTextTimer () { //resets the timer for next dialogue snippet
        //timeLeftTalk = Random.Range (7f, 30f);
        timeLeftTalk = 2f;
    }
}
#endregion


#region Sleep State
public class StateSleep : State { //Sleep State
    bool addedListeners;
    public StateSleep (AIController controller) : base (controller) { } //required

    public override void Enable () {
        base.Enable ();
        EventsManager.CheckIfShopClosed.AddListener (CheckShopState);
        StateName = StateType.Sleep;

        if (EventsManager.ReturnShopState()) { //if shop is open
            AddListeners ();
            EventsManager.DisableMask ();
            
        } else { //else if shop is closed
            EventsManager.DisableMask ();
        }
    }

    void CheckShopState (bool shopOpen) { //TODO: think of ways to fire off shop close and open logic properly currently gets stuck in uninteractable state
        switch (shopOpen) {
            case true:
                if (!addedListeners) { //if never added listeners and shop gets opened during sleep state add wakeup listeners
                    //adds listeners for any form of interaction towards vendor to wake them up
                    AddListeners ();
                }
                break;
            case false:
                if (addedListeners) { //else if listeners have been added and the shop is currently closed remove listeners
                    RemoveListeners ();
                }
                break;
        }
    }

    public override void Update () {
    }

    void AddListeners () { //adds interaction listeners
        EventsManager.BodyClicked.AddListener (Wakeup);
        EventsManager.MaskClicked.AddListener (Wakeup);
        EventsManager.TieClicked.AddListener (Wakeup);
        addedListeners = true;
    }

    void RemoveListeners () { //removes interaction listeners
        EventsManager.BodyClicked.RemoveListener (Wakeup);
        EventsManager.MaskClicked.RemoveListener (Wakeup);
        EventsManager.TieClicked.RemoveListener (Wakeup);
        addedListeners = false;
    }

    public void Wakeup () { //changes state back to idle (wakes up vendor)
        EventsManager.EnableMask ();
        Controller.ChangeState (Controller.stateIdle);
    }
}
#endregion


#region ShopPrompt State
public class StateShopPrompt : State { //Open Jacket State
    
    float timeLeftIdle;
    public StateShopPrompt (AIController controller) : base (controller) { } //required

    public override void Enable () {
        base.Enable ();

        EventsManager.BodyClicked.AddListener (OpenShop); //adds listener to open shop
        EventsManager.DialogueDetermine (DialogueType.ShopPrompt); //tells dialogue system to display a shopprompt snippet
        
        StateName = StateType.ShopPrompt;
        timeLeftIdle = 20f;
    }

    public override void Disable () {
        base.Disable ();
        
        EventsManager.BodyClicked.RemoveListener (OpenShop); //removes listener to open shop
    }

    public override void Update () { //timer before coat closes
        if (timeLeftIdle > 0) { //Timer between state change to idle
            timeLeftIdle -= Time.deltaTime;
        } else { //if timer is done close coat and go back to idle
            Controller.ChangeState(Controller.stateIdle);
        }
    }

    public void OpenShop () { //changes state to shopping state
        Controller.ChangeState (Controller.stateShopping);
        EventsManager.DialogueDetermine (DialogueType.ShopPrompt); //tells dialogue system to display a shopprompt snippet
        EventsManager.OpenedShop (); //tells event manager to fire off event to tell main system that the shop can be opened successfully
        EventsManager.RandomizeShop (); // tells event manager to fire off event to randomize each shop instance in scene
    }
    
    
}
#endregion


#region Shopping State

public class StateShopping : State {
    //Open Shop State

    float timeLeftIdle;
    public StateShopping (AIController controller) : base (controller) { } //required

    public override void Enable () {
        base.Enable ();

        EventsManager.ShopClosed.AddListener (CloseShop); //adds listener to close shop

        StateName = StateType.Shopping;
    }

    public override void Disable () {
        base.Disable ();
        EventsManager.BodyClicked.RemoveListener (CloseShop); //removes listener to close shop
    }

    public override void Update () { }

    public void CloseShop () {
        //changes state to idle state
        Controller.ChangeState (Controller.stateIdle);
    }
}

#endregion

#region Agressive State

    public class StateAgressive : State {
        //Open Jacket State
        public StateAgressive (AIController controller) : base (controller) { } //required

        public override void Enable () {
            base.Enable ();
            StateName = StateType.Agressive;
            EventsManager.OpenedShop (); //open the shop
            EventsManager.RandomizeShop (); //randomizes shops

        }

        public override void Update () { }
    }
#endregion

//Corrupted States