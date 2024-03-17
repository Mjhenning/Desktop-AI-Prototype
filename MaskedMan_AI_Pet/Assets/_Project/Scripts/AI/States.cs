using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
FSM States
*/

//All transitions from idle state done just tweak timers
#region Idle State

public class State_Idle : State { //Idle State
    float timeLeftSleep;
    float timeLeftTalk;
    float timeLeftShop;
    float timeLeftAgressive;
    bool IsShopOpen;
    
    public State_Idle (AIController Controller) : base (Controller) { } //required

    public override void Enable () {
        base.Enable (); //Fires off the log of the base abstract function (everything already inside abstract class fires when base.enable is called)
        stateName = StateType.Idle;
        EventsManager.CheckIfShopClosed.AddListener (CheckBeforeResumeUpdate);
        
        timeLeftSleep = Random.Range(30f,60f);
        ResetTextTimer ();
        //timeLeftShop = Random.Range (10f, 300f);
        timeLeftShop = 10f;
        timeLeftAgressive = 1200f;
    }
    

    public override void Update () {

        switch (IsShopOpen) {
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
                break;
            case false:
                Controller.ChangeState (Controller.stateSleep);
                break;
        }
        
       
        
        // if (timeLeftAgressive > 0) { //Timer between state change to aggressive behaviour
        //     timeLeftAgressive -= Time.deltaTime;
        // } else {
        //     Controller.ChangeState (Controller.stateAgressive);
        // }

    }

    void CheckBeforeResumeUpdate (bool ShopOpen) {
        IsShopOpen = ShopOpen;
    }

    void ResetTextTimer () {
        timeLeftTalk = Random.Range (7f, 30f); //NOT A STATE
    }
}
#endregion

//Wake up function coded so sleep state is done
#region Sleep State
public class State_Sleep : State { //Sleep State
    bool AddedListeners;
    public State_Sleep (AIController Controller) : base (Controller) { } //required

    public override void Enable () {
        base.Enable ();
        EventsManager.CheckIfShopClosed.AddListener (CheckShopState);
        stateName = StateType.Sleep;

        if (EventsManager.ReturnShopState()) { //if shop is open
            AddListeners ();
            EventsManager.DisableMask ();
            
        } else { //else if shop is closed
            EventsManager.DisableMask ();
        }
    }

    void CheckShopState (bool ShopOpen) { //TODO: think of ways to fire off shop close and open logic properly currently gets stuck in uninteractable state
        switch (ShopOpen) {
            case true:
                if (!AddedListeners) { //if never added listeners and shop gets opened during sleep state add wakeup listeners
                    //adds listeners for any form of interaction towards vendor to wake them up
                    AddListeners ();
                }
                break;
            case false:
                if (AddedListeners) { //else if listeners have been added and the shop is currently closed remove listeners
                    RemoveListeners ();
                }
                break;
        }
    }

    public override void Update () {
    }

    void AddListeners () {
        EventsManager.BodyClicked.AddListener (Wakeup);
        EventsManager.MaskClicked.AddListener (Wakeup);
        EventsManager.TieClicked.AddListener (Wakeup);
        AddedListeners = true;
    }

    void RemoveListeners () {
        EventsManager.BodyClicked.RemoveListener (Wakeup);
        EventsManager.MaskClicked.RemoveListener (Wakeup);
        EventsManager.TieClicked.RemoveListener (Wakeup);
        AddedListeners = false;
    }

    public void Wakeup () { //changes state back to idle (wakes up vendor)
        EventsManager.EnableMask ();
        Controller.ChangeState (Controller.stateIdle);
    }
}
#endregion

//Transition to shopping state coded and open shop event coded + Added dialogue when first opens coat and when player clicks on vendor : DONE
#region ShopPrompt State
public class State_ShopPrompt : State { //Open Jacket State
    
    float timeLeftIdle;
    public State_ShopPrompt (AIController Controller) : base (Controller) { } //required

    public override void Enable () {
        base.Enable ();

        EventsManager.BodyClicked.AddListener (OpenShop); //adds listener to open shop

        stateName = StateType.ShopPrompt;
        EventsManager.DialogueDetermine (DialogueType.ShopPrompt);

        timeLeftIdle = 20f;
    }

    public override void Disable () {
        base.Disable ();
        
        EventsManager.BodyClicked.RemoveListener (OpenShop); //removes listener to open shop
    }

    public override void Update () { //timer before coat closes
        if (timeLeftIdle > 0) { //Timer between state change to idle
            timeLeftIdle -= Time.deltaTime;
        } else {
            Controller.ChangeState(Controller.stateIdle);
        }
    }

    public void OpenShop () { //changes state to shopping state
        Controller.ChangeState (Controller.stateShopping);
        EventsManager.DialogueDetermine (DialogueType.ShopPrompt);
        EventsManager.OpenedShop (); //tells event manager to fire off event to tell main system that the shop can be opened successfully
    }
    
    
}
#endregion

//NEEDS WORKS
#region Shopping State

public class State_Shopping : State {
    //Open Shop State

    float timeLeftIdle;
    public State_Shopping (AIController Controller) : base (Controller) { } //required

    public override void Enable () {
        base.Enable ();

        EventsManager.ShopClosed.AddListener (CloseShop); //adds listener to close shop

        stateName = StateType.Shopping;
    }

    public override void Disable () {
        base.Disable ();
        EventsManager.BodyClicked.RemoveListener (CloseShop); //removes listener to close shop
    }

    public override void Update () { }

    public void CloseShop () {
        //changes state to idle state
        Controller.ChangeState (Controller.stateIdle);
        EventsManager.ClosedShop (); //tells event manager to fire off event to tell main system that the shop can be closed
    }
}

#endregion

#region Agressive State

    public class State_Agressive : State {
        //Open Jacket State
        public State_Agressive (AIController Controller) : base (Controller) { } //required

        public override void Enable () {
            base.Enable ();
            stateName = StateType.Agressive;
        }

        public override void Update () { }
    }
#endregion

//Corrupted States