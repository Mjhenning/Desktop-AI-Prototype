using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Text_Corruption : MonoBehaviour {
    [SerializeField]List<TextMeshProUGUI> textComponents;
    
    public void Corrupt () { //wrote as a function so it can be called by outside scripts
        foreach (var _component in textComponents) {
            _component.text = StringCorrupter.CorruptString (_component.text, Corruption_Manager.instance.corruptionPercentage, "<color=red>");
            Debug.Log ("Corrupted " + Corruption_Manager.instance.corruptionPercentage + " of the dialogue text");
        }
    }
}
