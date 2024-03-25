using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum DialogueType { 
    Idle,
    ShopPrompt,
    Advertise,
    Positive,
    Negative
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogueSO", order = 1)]
public class DialogueStrings : ScriptableObject { //scriptable used to store dialogue
    public DialogueType type;
    [FormerlySerializedAs ("DialogueSnippets")] public List<string> dialogueSnippets;

}
