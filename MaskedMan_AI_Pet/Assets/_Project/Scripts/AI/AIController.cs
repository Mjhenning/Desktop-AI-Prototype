using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIController : MonoBehaviour {

    public static AIController instance;
    
    public Animator VendorAnimator;
    public Animator TieAnimator;
    
    [SerializeReference]State currentState;
    [SerializeReference] StateType currentStateEnum;

    public State_Idle stateIdle;
    public State_Sleep stateSleep;
    public State_ShopPrompt stateShopPrompt;
    public State_Shopping stateShopping;
    
    public State_Agressive stateAgressive;

    void Awake () {
        instance = this;
    }

    void Start() {
        stateIdle = new State_Idle (this);
        stateSleep = new State_Sleep (this);
        stateShopPrompt = new State_ShopPrompt (this);
        stateShopping = new State_Shopping (this);
        ChangeState (stateIdle);
        EventsManager.StartHourlyTimer ();
    }

    void Update() {
        currentState?.Update();
    }

    public void ChangeState(State newstate) { //Used to change state
        if (newstate == currentState) return; //if newstate is current state ignore rest of function
        
        //else call disable set currenstate to new state then enable
        currentState?.Disable();
        currentState = newstate;
        currentState.Enable();
        
        //after enabling set the enum to this state's enum
        currentStateEnum = currentState.stateName;

        //set integer on animator acordingly to determine state of animators
        VendorAnimator.SetInteger ("StateEnum", (int)currentStateEnum);
        TieAnimator.SetInteger ("StateEnum", (int) currentStateEnum);

    }

    public StateType GetCurrentState () { //used to tell something what the current state is
        return currentStateEnum;
    }
    
    
}







