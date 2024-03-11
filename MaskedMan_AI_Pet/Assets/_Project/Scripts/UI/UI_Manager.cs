using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_Manager : MonoBehaviour {

    [SerializeField]List<Sprite> Masks;
    [SerializeField]AIController activeController;

    [SerializeField]TextMeshProUGUI TextBubble;
    
    [SerializeField]Image Mask;
    [SerializeField]Image Tie;
    [SerializeField]Image Body;

    int currentIndex; //used to go scroll through list of masks

    void Start() {
        currentIndex = 0;
        activeController = AIController.instance; //grabs the only ai controller in scene
        
        EventsManager.instance.ShopOpened.AddListener (OpenShop); //adds listener to know when shop UI should be displayed
        EventsManager.instance.ShopClosed.AddListener (CloseShop); //adds listener to know when shop ui should stop being displayed
        EventsManager.instance.DialogueStringEvent.AddListener (DisplayDialogue); //adds listener that passes along string
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


        EventsManager.instance.ClickedMask (); //tells system mask was clicked

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
        
        EventsManager.instance.ClickedTie (); //tells system tie was clicked
    }
    
    public void ClickedBody () {
        EventsManager.instance.ClickedBody (); //fires off event to tell system body has been clicked
    }

    public void OpenShop () {
        ObjectPool.instance.GetObjectFromPool (); //grabs shop ui from pool
    }

    public void CloseShop () {
        ObjectPool.instance.ReturnAllObjectsToPool (); //returns all shop ui to pool

    }

    void RandomizeTieColor () { //randomizes the tie color
        Tie.color = new Color (
            Random.Range(0f,1f),
            Random.Range(0f,1f),
            Random.Range(0f,1f)
        );
    }

    void ChangeMaskInOrderd () {
        //Used to run through list of masks in order
        if (currentIndex != Masks.Count -1) {
            currentIndex++;
            Mask.sprite = Masks[currentIndex];  
        } else {
            currentIndex = 0;
            Mask.sprite = Masks[currentIndex];
        }
        
        
        //If no mask is selected make it's color clear otherwise keep it white
        Mask.color = Mask.sprite != null ? Color.white : Color.clear;
    }

    void DisplayDialogue (String text) { //used to display the grabbed dialogue and the start a counter beore dialogue stops displaying
        TextBubble.transform.parent.gameObject.SetActive (true);
        TextBubble.text = text;
        Debug.Log ("Displaying text:" + text);

        StartCoroutine (CountTillDisable(TextBubble,6f));
    }

    IEnumerator CountTillDisable (TextMeshProUGUI textObj, float waitTime) {
        yield return new WaitForSeconds (waitTime);
        textObj.transform.parent.gameObject.SetActive (false);
    }

}
