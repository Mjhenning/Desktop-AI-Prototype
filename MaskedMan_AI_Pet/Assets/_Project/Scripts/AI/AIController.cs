using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIController : MonoBehaviour {

    public static AIController Instance;
    
    public Animator vendorAnimator;
    public Animator tieAnimator;
    
    [SerializeReference]State currentState;
    [SerializeReference] StateType currentStateEnum;

    public StateIdle stateIdle;
    public StateSleep stateSleep;
    public StateShopPrompt stateShopPrompt;
    public StateShopping stateShopping;
    
    public StateAgressive stateAgressive;

    void Awake () {
        Instance = this;
    }

    void Start() {
        //used to setup each state
        stateIdle = new StateIdle (this);
        stateSleep = new StateSleep (this);
        stateShopPrompt = new StateShopPrompt (this);
        stateShopping = new StateShopping (this);
        stateAgressive = new StateAgressive (this);
        ChangeState (stateIdle);
        //tells the timer to start checking each hour if the shop should be open or closed
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
        currentStateEnum = currentState.StateName;

        //set integer on animator acordingly to determine state of animators
        vendorAnimator.SetInteger ("StateEnum", (int)currentStateEnum);
        tieAnimator.SetInteger ("StateEnum", (int) currentStateEnum);

    }

    public StateType GetCurrentState () { //used to tell something what the current state is
        return currentStateEnum;
    }
    
    
}







