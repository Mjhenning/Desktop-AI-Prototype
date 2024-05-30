using System;
using UnityEngine;

public class Corruption_Manager : MonoBehaviour {

    public static Corruption_Manager instance;
    
    double max = 20;
    double min = 0;
    public double currentCorruption;
    public double corruptionPercentage;

    void OnEnable () {
        instance = this;
    }


    public void Corrupt () { //everytime an item gets discarded increase the current corruption and recalculate the percentage
        currentCorruption++;
        currentCorruption = Math.Clamp(currentCorruption, min, max);
        CalculateCorruptionPercentage ();
    }

    public void Purify () { //everytime an item gets bough decrease current corruption and recalculate the percentage
        currentCorruption--;
        currentCorruption = Math.Clamp(currentCorruption, min, max);
        CalculateCorruptionPercentage ();
    }

    void CalculateCorruptionPercentage () { //calculates current corruption as a percentage
        double _corruptiondecimal = currentCorruption / max;
        corruptionPercentage = _corruptiondecimal * 100;
    }
}
