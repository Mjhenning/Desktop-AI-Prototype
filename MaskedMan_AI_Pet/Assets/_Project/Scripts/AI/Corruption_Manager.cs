using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corruption_Manager : MonoBehaviour {

    public static Corruption_Manager instance;
    
    int maxCorruption = 20;
    int currentCorruption;
    public double corruptionPercentage;

    void Awake () {
        instance = this;
        EventsManager.RemoveItem.AddListener (Corrupt);
        EventsManager.CheckDatabase.AddListener (Purify);
    }


    void Corrupt (ItemInstance _item) { //everytime an item gets discarded increase the current corruption and recalculate the percentage
        currentCorruption++;
        CalculateCorruptionPercentage ();
    }

    void Purify (ItemInstance _item, bool _bool) { //everytime an item gets bough decrease current corruption and recalculate the percentage
        currentCorruption--;
        CalculateCorruptionPercentage ();
    }

    void CalculateCorruptionPercentage () { //calculates current corruption as a percentage
        corruptionPercentage = (currentCorruption / maxCorruption) * 100;
    }
}
