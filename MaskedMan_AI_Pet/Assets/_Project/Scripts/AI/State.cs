using UnityEngine;

public enum StateType {
    Idle,
    Sleep,
    ShopPrompt,
    Shopping,
    Agressive
}

/*
FSM Base Class
*/

public abstract class State { //Blueprint for each state
    
    public StateType stateName; //Enum for ui_manager

    public bool stateEnabled = false;
    public AIController Controller; //Connection to call utility functions within states

    protected State (AIController Controller) { //Protected is for all inherited classes, private is protected for that specific instance of the class
        this.Controller = Controller;
    }
   
    public virtual void Enable () {
        stateEnabled = true;
        Debug.Log ($"{GetType()} State Enabled");
    }

    public virtual void Disable () {
        stateEnabled = false;
        Debug.Log ($"{GetType()} State Disabled");
    }

    public abstract void Update ();
}
