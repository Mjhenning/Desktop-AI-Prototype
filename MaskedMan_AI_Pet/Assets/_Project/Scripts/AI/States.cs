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
    public State_Idle (AIController Controller) : base (Controller) { } //required

    public override void Enable () {
        base.Enable (); //Fires off the log of the base abstract function (everything already inside abstract class fires when base.enable is called)

        stateName = StateType.Idle;
        
        timeLeftSleep = 2f;
        ResetTextTimer ();
        timeLeftShop = Random.Range (10f, 20f);
        timeLeftAgressive = 1200f;
    }
    

    public override void Update () {
        
        if (timeLeftTalk > 0) { //Timer between dialogue 
            timeLeftTalk -= Time.deltaTime;
        } else {
            EventsManager.instance.DialogueDetermine (DialogueType.Idle);
            ResetTextTimer ();
        }

        // if (timeLeftSleep >0) { //Timer between state change to sleep
        //     timeLeftSleep -= Time.deltaTime;
        // } else {
        //     Controller.ChangeState(Controller.stateSleep);
        // }
        
        if (timeLeftShop > 0) { //Timer between state change to shop (coat open)
            timeLeftShop -= Time.deltaTime;
        } else {
            Controller.ChangeState (Controller.stateShopPrompt);
        }
        
        // if (timeLeftAgressive > 0) { //Timer between state change to aggressive behaviour
        //     timeLeftAgressive -= Time.deltaTime;
        // } else {
        //     Controller.ChangeState (Controller.stateAgressive);
        // }

    }

    void ResetTextTimer () {
        timeLeftTalk = Random.Range (7f, 30f); //NOT A STATE
    }
}
#endregion

//Wake up function coded so sleep state is done
#region Sleep State
public class State_Sleep : State { //Sleep State
    public State_Sleep (AIController Controller) : base (Controller) { } //required

    public override void Enable () {
        base.Enable ();
        
        //adds listeners for any form of interaction towards vendor to wake them up
        EventsManager.instance.BodyClicked.AddListener (Wakeup);
        EventsManager.instance.MaskClicked.AddListener (Wakeup);
        EventsManager.instance.TieClicked.AddListener (Wakeup);
        
        stateName = StateType.Sleep;
    }

    public override void Update () {
    }

    public void Wakeup () { //changes state back to idle (wakes up vendor)
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

        EventsManager.instance.BodyClicked.AddListener (OpenShop); //adds listener to open shop

        stateName = StateType.ShopPrompt;
        EventsManager.instance.DialogueDetermine (DialogueType.ShopPrompt);

        timeLeftIdle = 20f;
    }

    public override void Disable () {
        base.Disable ();
        
        EventsManager.instance.BodyClicked.RemoveListener (OpenShop); //removes listener to open shop
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
        EventsManager.instance.DialogueDetermine (DialogueType.ShopPrompt);
        EventsManager.instance.OpenedShop (); //tells event manager to fire off event to tell main system that the shop can be opened successfully
    }
    
    
}
#endregion

//NEEDS WORKS
#region Shopping State
public class State_Shopping : State { //Open Shop State
    
    float timeLeftIdle;
    public State_Shopping (AIController Controller) : base (Controller) { } //required

    public override void Enable () {
        base.Enable ();
        
        EventsManager.instance.BodyClicked.AddListener (CloseShop); //adds listener to close shop
        
        stateName = StateType.Shopping;
    }

    public override void Disable () {
        base.Disable ();
        EventsManager.instance.BodyClicked.RemoveListener (CloseShop); //removes listener to close shop
    }

    public override void Update () {
    }
    
    public void CloseShop() { //changes state to idle state
        Controller.ChangeState (Controller.stateIdle);
        EventsManager.instance.ClosedShop (); //tells event manager to fire off event to tell main system that the shop can be opened successfully
    }
}
#endregion

#region Agressive State
public class State_Agressive : State { //Open Jacket State
    public State_Agressive (AIController Controller) : base (Controller) { } //required

    public override void Enable () {
        base.Enable ();
        stateName = StateType.Agressive;
    }

    public override void Update () {
    }
}
#endregion

//Corrupted States