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
    
    public StateType StateName; //Enum for ui_manager
    
    public AIController Controller; //Connection to call utility functions within states

    protected State (AIController controller) { //Protected is for all inherited classes, private is protected for that specific instance of the class
        this.Controller = controller;
    }
   
    public virtual void Enable () {
       // Debug.Log ($"{GetType()} State Enabled");
    }

    public virtual void Disable () {
        //Debug.Log ($"{GetType()} State Disabled");
    }

    public abstract void Update ();
}
