using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Text_Corruption : MonoBehaviour {
    [SerializeField]List<TextMeshProUGUI> textComponents;

    void OnEnable () { //On enable calls the stringcorrupter to randomly corrupt characters inside the assigned string components;
        foreach (var _component in textComponents) {
            _component.text = StringCorrupter.CorruptString (_component.text, Corruption_Manager.instance.corruptionPercentage, "<color=red>");
            Debug.Log ("Corrupted " + Corruption_Manager.instance.corruptionPercentage + " of the dialogue text");
        }
    }
}
