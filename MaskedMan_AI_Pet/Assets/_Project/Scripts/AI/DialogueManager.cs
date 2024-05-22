using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour
{
    string tmpRandomSnippet;
    string dialogueSnippet;

    //Scriptables with strings attatched
    [SerializeField]List<DialogueStrings> stringTypes;

    void Start () {
        EventsManager.DialogueEvent.AddListener (SendALine);
    }

    void SendALine (DialogueType type) { //setup in this manner so api can be used when anything besides a shop prompt is generated
        if (type != DialogueType.ShopPrompt) {
            int chance;
            chance = Random.Range (0, 2);
            switch (chance) {
                case 0:
                    EventsManager.DialogueFeed (GrabALine (type));
                    Debug.Log ("Displaying dialogue string");
                    break;
                case 1:
                    API_Manager.instance.CallRandomFact(OnFactReceived);
                    Debug.Log ("Displaying fact");
                    break;
            }  
        } else {
            EventsManager.DialogueFeed (GrabALine (type));
        }
       
    }

    void OnFactReceived (string fact) {
        if (fact != null) {
            EventsManager.DialogueFeed(fact);
        } else {
            Debug.LogError("Failed to retrieve a random fact.");
        }
    }

    //Assign used dialogue to temp var, double check if picked random.ranger is the same as previous dialogue if it is re re-randomize else use the picked dialogue
    
    string GrabALine (DialogueType type) {
        switch (type) {
            case DialogueType.Idle:
                return (DialogueLoop (type));
            case DialogueType.ShopPrompt:
                return (DialogueLoop (type));
            default:
                return null;
        }
    }
    
    
    //Checks to make sure if a idle chatter and or shop prompt is asked for twice in a row make sure that the randomized string isn't the same as the previous then only use that string
    
    bool CheckSnippetBeforeReturn (string previous, string current) { //checks snippet before allowing it to be fed as a usable string
        return current != previous;
    }

    string GenerateSnippet (DialogueType type) { //generates a new snippet
        DialogueStrings _dialogue = stringTypes.Find (x => x.type == type);
        return _dialogue.dialogueSnippets[Random.Range (0, _dialogue.dialogueSnippets.Count)];
    }

    string DialogueLoop (DialogueType type) { //loops until snippet and previous snippet isn't the same then feeds it to dialogue system
        while (!CheckSnippetBeforeReturn(tmpRandomSnippet,dialogueSnippet)) {
            tmpRandomSnippet = GenerateSnippet(type);
        }

        dialogueSnippet = tmpRandomSnippet;
        return dialogueSnippet;
    }






}
