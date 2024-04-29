using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_Manager : MonoBehaviour {
    public static UI_Manager instance;

    PlayerController input;

    [FormerlySerializedAs ("Masks")] [SerializeField]List<Sprite> masks;
    [SerializeField]AIController activeController;

    [FormerlySerializedAs ("TextBubble")] [SerializeField]TextMeshProUGUI textBubble;
    [FormerlySerializedAs ("OverUI")] [SerializeField] bool overUI;
    
    [FormerlySerializedAs ("Mask")] [SerializeField]Image mask;
    [FormerlySerializedAs ("Tie")] [SerializeField]Image tie;
    [FormerlySerializedAs ("Body")] [SerializeField]Image body;

    int currentIndex; //used to go scroll through list of masks

    void Awake () {
        instance = this;
        input = new PlayerController ();
        input.Enable ();
        input.Player.Click.performed += ClickOnperformed; //if the player clicks
    }

    void ClickOnperformed (InputAction.CallbackContext obj) {
        switch (overUI) {
            case false:
                switch (activeController.GetCurrentState() ) { 
                   case StateType.Shopping: //if the player is currently shopping and not over ui when they click
                       EventsManager.ClosedShop (); //close the shop
                       break;
                   case StateType.Agressive: //if the vendor is currently agressive and the player clicks while not over ui
                       ObjectPool.Instance.DuplicateObjectFromPool (ObjectPool.Instance.pooledObjects[0]); //duplicate the current object from the pool of shop windows
                       break;
                }
                break;
        }
    }
    

    void Start() {
        currentIndex = 0;
        activeController = AIController.Instance; //grabs the only ai controller in scene
        
        EventsManager.ShopOpened.AddListener (OpenShop); //adds listener to know when shop UI should be displayed
        EventsManager.ShopClosed.AddListener (CloseShop); //adds listener to know when shop ui should stop being displayed
        EventsManager.DialogueStringEvent.AddListener (DisplayDialogue); //adds listener that passes along string
        EventsManager.DisableMaskInteractions.AddListener (DisableMask); //adds listener that disables the mask go
        EventsManager.EnableMaskInteractions.AddListener (EnableMask); //adds listener that enables the mask go
    }

    void Update () {
        overUI = EventSystem.current.IsPointerOverGameObject ();
    }


    public void ClickedMask () { //Used to randomize the mask if the player clicks on it

        switch (activeController.GetCurrentState ()) {
            case StateType.Idle:
                ChangeMaskInOrderd ();
                break;
            case StateType.ShopPrompt:
                ChangeMaskInOrderd ();
                break;
            case StateType.Shopping:
                ChangeMaskInOrderd ();
                break;
        }


        EventsManager.ClickedMask (); //tells system mask was clicked

    }
    
    public void ClickedTie () { //used to randomize the mask's color if the player clicks on it

        switch (activeController.GetCurrentState()) {
            case StateType.Idle:
                RandomizeTieColor ();
                break;
            case StateType.ShopPrompt:
                RandomizeTieColor ();
                break;
            case StateType.Shopping:
                RandomizeTieColor ();
                break;
        }
        
        EventsManager.ClickedTie (); //tells system tie was clicked
    }
    
    public void ClickedBody () {
        EventsManager.ClickedBody (); //fires off event to tell system body has been clicked
    }

    public void OpenShop () {
        ObjectPool.Instance.GetObjectFromPool (); //grabs shop ui from pool
    }

    public void CloseShop () {
        ObjectPool.Instance.ReturnAllObjectsToPool (); //returns all shop ui to pool

    }

    void RandomizeTieColor () { //randomizes the tie color
        tie.color = new Color (
            Random.Range(0f,1f),
            Random.Range(0f,1f),
            Random.Range(0f,1f)
        );
    }

    void ChangeMaskInOrderd () {
        //Used to run through list of masks in order
        if (currentIndex != masks.Count -1) {
            currentIndex++;
            mask.sprite = masks[currentIndex];  
        } else {
            currentIndex = 0;
            mask.sprite = masks[currentIndex];
        }
        
        
        //If no mask is selected make it's color clear otherwise keep it white
        mask.color = mask.sprite != null ? Color.white : Color.clear;
    }

    void DisplayDialogue (String text) { //used to display the grabbed dialogue and the start a counter beore dialogue stops displaying
        textBubble.transform.parent.gameObject.SetActive (true);
        textBubble.text = text;
        Debug.Log ("Displaying text:" + text);

        StartCoroutine (CountTillDisable(textBubble,6f));
    }

    IEnumerator CountTillDisable (TextMeshProUGUI textObj, float waitTime) { //used to wait 6 seconds before disabling text bubble
        yield return new WaitForSeconds (waitTime);
        textObj.transform.parent.gameObject.SetActive (false);
    }

    void DisableMask () { //used to disable mask gameobject
        mask.gameObject.SetActive (false);
    }

    void EnableMask () { //used to enable mask gameobject
        StartCoroutine (WaitBeforeMaskEnable ());

    }

    IEnumerator WaitBeforeMaskEnable () { //used to wait 1 second before enabling mask gameobject
        yield return new WaitForSeconds (1f);
        mask.gameObject.SetActive (true);
    }

}
