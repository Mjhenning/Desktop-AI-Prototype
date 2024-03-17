using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour
{
    string tmpRandomSnippet;
    string Snippet;

    //Scriptables with strings attatched
    [SerializeField]List<DialogueStrings> StringTypes;

    void Start () {
        EventsManager.dialogueEvent.AddListener (SendALine);
    }

    public void SendALine (DialogueType type) {
        EventsManager.DialogueFeed (GrabALine (type)); //TODO: DON'T LIKE THIS NOT MODULAR ENOUGH
    }

    //Assign used dialogue to temp var, double check if picked random.ranger is the same as previous dialogue if it is re re-randomize else use the picked dialogue
    
    public string GrabALine (DialogueType type) {
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
        DialogueStrings Dialogue;
        Dialogue = StringTypes.Find (x => x.type == type);
        return Dialogue.DialogueSnippets[Random.Range (0, Dialogue.DialogueSnippets.Count)];
    }

    string DialogueLoop (DialogueType type) { //loops until snippet and previous snippet isn't the same then feeds it to dialogue system
        while (!CheckSnippetBeforeReturn(tmpRandomSnippet,Snippet)) {
            tmpRandomSnippet = GenerateSnippet(type);
        }

        Snippet = tmpRandomSnippet;
        return Snippet;
    }






}
