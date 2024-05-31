using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_Manager : MonoBehaviour {
    public static UI_Manager instance;

    public PlayerController input;


    [FormerlySerializedAs ("Masks")] [SerializeField]List<Sprite> masks;
    [SerializeField]AIController activeController;

    [FormerlySerializedAs ("TextBubble")] [SerializeField]TextMeshProUGUI textBubble;

    [FormerlySerializedAs ("Mask")] [SerializeField]Image mask;
    [FormerlySerializedAs ("Tie")] public Image tie;
    [FormerlySerializedAs ("Body")] [SerializeField]Image body;
    [SerializeField] GameObject listObj;
    [SerializeField] GameObject listBtn;

    public int maskIndex; //used to go scroll through list of masks

    void Awake () {
        instance = this;
        input = new PlayerController ();
        input.Enable ();
        input.Player.Click.performed += ClickOnperformed; //if the player clicks
        input.Player.Escape.performed += EscapeOnperformed;
    }

    
    void EscapeOnperformed (InputAction.CallbackContext obj) {
        Application.Quit ();
    }
    
    void ClickOnperformed (InputAction.CallbackContext obj) {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        switch (CheckOverUIWin()) {
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
#elif UNITY_ANDROID
        Vector2 touchPosition = Pointer.current.position.ReadValue();
        
        switch (CheckOverUIAnd(touchPosition)) {
            case false:
                switch (activeController.GetCurrentState() ) { 
                    case StateType.Shopping: //if the player is currently shopping and not over ui when they click
                        EventsManager.ClosedShop (); //close the shop
                        break;
                    case StateType.Agressive: //if the vendor is currently aggressive and the player clicks while not over ui
                        ObjectPool.Instance.DuplicateObjectFromPool (ObjectPool.Instance.pooledObjects[0]); //duplicate the current object from the pool of shop windows
                        break;
                }
                break;
        }
#endif

       
    }
    

    void Start() {
        activeController = AIController.Instance; //grabs the only ai controller in scene
        
        EventsManager.ShopOpened.AddListener (OpenShop); //adds listener to know when shop UI should be displayed
        EventsManager.ShopClosed.AddListener (CloseShop); //adds listener to know when shop ui should stop being displayed
        EventsManager.DialogueStringEvent.AddListener (DisplayDialogue); //adds listener that passes along string
        EventsManager.DisableMaskInteractions.AddListener (DisableMask); //adds listener that disables the mask go
        EventsManager.EnableMaskInteractions.AddListener (EnableMask); //adds listener that enables the mask go
    }


    bool CheckOverUIWin () {
        return EventSystem.current.IsPointerOverGameObject();
    }

    bool CheckOverUIAnd (Vector2 touchPos) {
        // Create a pointer event data with the touch position
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = touchPos;

        // Create a list to store raycast results
        var raycastResults = new List<RaycastResult>();

        // Perform a raycast to check if the touch is over a UI element
        EventSystem.current.RaycastAll(eventData, raycastResults);

        // Return true if any raycast result hits a UI object
        return raycastResults.Count > 0;
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
        if (maskIndex != masks.Count -1) {
            maskIndex++;
            mask.sprite = masks[maskIndex];  
        } else {
            maskIndex = 0;
            mask.sprite = masks[maskIndex];
        }
        
        
        //If no mask is selected make it's color clear otherwise keep it white
        mask.color = mask.sprite != null ? Color.white : Color.clear;
    }

    public void CallOnLoad (Color color, int index) { //called by save load manager to load custom data
        tie.color = color;
        maskIndex = index;
        if (index != 0) {
            mask.sprite = masks[index];
            mask.color = Color.white;  
        }

    }

    void DisplayDialogue (String text, bool Corruptable) { //used to display the grabbed dialogue and the start a counter before dialogue stops displaying
        textBubble.text = text;
        textBubble.transform.parent.gameObject.SetActive (true);
        if (Corruptable) {
            textBubble.GetComponent<Text_Corruption> ().Corrupt ();   
        }
       

        StartCoroutine (CountTillDisable(textBubble,10f));
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

    public void ShowList () { //shows bought items (inventory)
        listObj.SetActive (true);
        listBtn.SetActive (false);
    }

    public void CloseList () { //stops showing bought items (inventory)
        listObj.SetActive (false);
        listBtn.SetActive (true);
    }

}
