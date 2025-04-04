using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goop_Instance : MonoBehaviour {
    public GoopSize size;

    void Update () { //used to move goop gradually down the screen when spawned;
        Vector3 translateDir = Vector3.down * Time.deltaTime;
        transform.Translate (translateDir * 60f);
    }

    public void CollectGoop () { //used to add goop to currency amount based off of size if goop is clicked
        switch (size) {
            case GoopSize.Small:
                EventsManager.AddGoop (5);
                break;
            case GoopSize.Medium:
                EventsManager.AddGoop (10);
                break;
            case GoopSize.Big:
                EventsManager.AddGoop (20);
                break;
        }

        Destroy (gameObject);
    }

    void OnBecameInvisible () {
        Destroy (this);
    }
}
